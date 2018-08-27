using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using ANDP.Provisioning.API.Rest.Models.TcpIp;
using Common.Lib.Common;
using Common.Lib.Common.Caching;
using Common.Lib.Common.Enums;
using Common.Lib.Domain.Common.Services;
using Common.Lib.Domain.Common.Services.ConnectionManager;
using Common.Lib.Domain.Common.Services.ConnectionManager.Socket;
using Common.Lib.Extensions;
using Common.Lib.Interfaces;
using Common.Lib.Security;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.Provisioning.API.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/tcpip")]
    public class TcpIpController : ApiController
    {
        private string _name;
        private readonly Oauth2AuthenticationSettings _oauth2AuthenticationSettings;
        private readonly ILogger _logger;
        private Guid _tenantId;
        private readonly TimeSpan _connectionCacheTimeSpan = new TimeSpan(0, 0, Convert.ToInt32(ConfigurationManager.AppSettings["HoldTcpIpConnectioninMemory"]), 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="ApMaxController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="oauth2AuthenticationSettings">The oauth2 authentication settings.</param>
        public TcpIpController(ILogger logger, Oauth2AuthenticationSettings oauth2AuthenticationSettings)
        {
            _name = Assembly.GetAssembly(GetType()).GetName().Name + " " +
                    Assembly.GetAssembly(GetType()).GetName().Version;
            _logger = logger;
            _oauth2AuthenticationSettings = oauth2AuthenticationSettings;
        }

        private MemoryCacheProvider CachingServiceFactory()
        {
            //Cache Service in memory
            return new MemoryCacheProvider(
                //Tells caching service how the connection should disconnect and logs when we close connections.
                delegate (CacheEntryRemovedArguments arguments)
                {
                    if (arguments != null)
                    {
                        try
                        {
                            _logger.WriteLogEntry(new List<object> { arguments }, "Connection removed from cache.", LogLevelType.Info);
                            var connection = arguments.CacheItem.Value as ConnectionInfo;
                            if (connection != null && connection.ConnectionManagerService.IsConnected)
                            {
                                connection.ConnectionManagerService.Disconnect();
                                //Clear Cache Service in memory
                                var provider = new MemoryCacheProvider();
                                provider.ClearCache(connection.SessionId);
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                });
        }

        /// <summary>
        /// The reason this is not in the constructor is solely for the purpose of exception handling.
        /// If you leave this in the controller and someone who is not authenticated calls the API you will not get a tenantId not found error.
        /// The error will be ugly and be hard to figure out you are not authorized.
        /// This way if the all methods have the ClaimsAuthorize attribute on them they will first be authenticated if not get a nice error message of not authorized.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">No Tenant Id Found.</exception>
        private ConnectionInfo Setup(int equipmentId, string sessionId)
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var tenant = identity.Claims.Where(c => c.Type == ClaimsConstants.TenantIdClaimType).Select(c => c.Value).SingleOrDefault();
            _oauth2AuthenticationSettings.Username = identity.Claims.Where(c => c.Type == ClaimsConstants.UserNameWithoutTenant).Select(c => c.Value).SingleOrDefault();
            _oauth2AuthenticationSettings.TenantName = identity.Claims.Where(c => c.Type == ClaimsConstants.TenantNameClaimType).Select(c => c.Value).SingleOrDefault();

            if (string.IsNullOrEmpty(tenant))
                throw new Exception("No Tenant Id Found.");

            _tenantId = Guid.Parse(tenant);

            if (string.IsNullOrEmpty(sessionId))
                sessionId = Guid.NewGuid().ToString();

            //Cache Service in memory
            var memoryCachingService = CachingServiceFactory();
            return memoryCachingService.FetchAndCache(sessionId,
                //Tells caching service how to cache and retrieve.
                delegate
                {
                    var equipmentConnectionSetting =
                        EquipmentConnectionSettingsService.RetrieveProvisioningEquipmentSettings(equipmentId,
                            _oauth2AuthenticationSettings).EquipmentConnectionSettings;

                    if (memoryCachingService.Count<ConnectionInfo>() >= equipmentConnectionSetting.MaxConcurrentConnections)
                    {
                        throw new Exception("No TcpIp connection available, max concurrent connections: " + equipmentConnectionSetting.MaxConcurrentConnections);
                    }

                    _logger.WriteLogEntry(new List<object> { equipmentConnectionSetting }, "Connection created in cache.", LogLevelType.Info);

                    return new ConnectionInfo
                    {
                        ConnectionManagerService = new ConnectionManagerService(equipmentConnectionSetting, _logger),
                        SessionId = sessionId
                    };
                },
                _connectionCacheTimeSpan
                );
        }

        /// <summary>
        /// Tests this instance.
        /// </summary>
        /// <returns></returns>
        [Route("test")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage Test()
        {
            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

            const string result = "OK";

            _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { result }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response(" + HttpStatusCode.OK + ")."), LogLevelType.Info);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Sendcommands the specified json.
        /// </summary>
        /// <param name="commandViewModel">The command view model.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Did not receive an json message in the post body.
        /// or
        /// EquipmentId is a required parameter.
        /// or
        /// Could not find any connection settings for the equipmentId.</exception>
        [Route("sendcommand/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage Sendcommand([FromBody]CommandViewModel commandViewModel)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                if (commandViewModel == null)
                    throw new Exception("Did not receive any json message in the post body.");

                if (commandViewModel.EquipmentId < 1)
                    throw new Exception("EquipmentId is a required parameter.");

                var connectionInfo = Setup(commandViewModel.EquipmentId, commandViewModel.SessionId);
                if (connectionInfo == null)
                    throw new Exception("Could not find any connection settings for the equipmentId.");

                var response = new CommandResponseViewModel
                {
                    SessionId = connectionInfo.SessionId
                };

                //Now lock the thread so it cannot be used by any other thread.
                using (var tlh = new ThreadLockHelper())
                {
                    try
                    {
                        tlh.Lock(connectionInfo.LockObj, commandViewModel.Timeout);
                        // Do thread safe work here
                        SocketResponse sr;
                        if (!connectionInfo.ConnectionManagerService.IsConnected)
                        {
                            sr = connectionInfo.ConnectionManagerService.Connect();
                            response.Data = sr.Data;
                            response.TimeoutOccurred = sr.TimeoutOccurred;
                        }

                        
                        if (string.IsNullOrEmpty(commandViewModel.ExpectedResponseRegEx))
                        {
                            sr = connectionInfo.ConnectionManagerService.SendCommandAndWaitForResponse(
                                commandViewModel.Command,
                                commandViewModel.ExpectedResponse,
                                new TimeSpan(0, 0, 0, commandViewModel.Timeout));
                        }
                        else
                        {
                            sr = connectionInfo.ConnectionManagerService.SendCommandAndWaitForResponse(
                                commandViewModel.Command,
                                new Regex(commandViewModel.ExpectedResponseRegEx),
                                new TimeSpan(0, 0, 0, commandViewModel.Timeout));
                        }

                        response.Data += sr.Data;
                        response.TimeoutOccurred = sr.TimeoutOccurred || response.TimeoutOccurred;

                        //connectionInfo.ConnectionManagerService.Disconnect();
                    }
                    catch (Exception)
                    {
                        if (connectionInfo.ConnectionManagerService != null && connectionInfo.ConnectionManagerService.IsConnected)
                            connectionInfo.ConnectionManagerService.Disconnect();

                        //Clear Cache Service in memory
                        var memoryCachingService = new MemoryCacheProvider();
                        memoryCachingService.ClearCache(connectionInfo.SessionId);

                        throw;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());

                throw;
            }
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        /// <param name="commandViewModel">The command view model.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Did not receive any json message in the post body.
        /// or
        /// EquipmentId is a required parameter.
        /// or
        /// Could not find any connection settings for the equipmentId.
        /// </exception>
        [Route("disconnect/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage Disconnect([FromBody]CommandViewModel commandViewModel)
        {
            try
            {
                var response = new CommandResponseViewModel();
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                if (commandViewModel == null)
                    throw new Exception("Did not receive any json message in the post body.");

                if (commandViewModel.EquipmentId < 1)
                    throw new Exception("EquipmentId is a required parameter.");

                var connectionInfo = Setup(commandViewModel.EquipmentId, commandViewModel.SessionId);
                if (connectionInfo == null)
                    throw new Exception("Could not find any connection settings for the equipmentId.");

                //Now lock the thread so it cannot be used by any other thread.
                using (var tlh = new ThreadLockHelper())
                {
                    tlh.Lock(connectionInfo.LockObj, commandViewModel.Timeout);
                    // Do thread safe work here
                    if (connectionInfo.ConnectionManagerService.IsConnected)
                    {
                        var sr = connectionInfo.ConnectionManagerService.Disconnect();
                        response = new CommandResponseViewModel
                        {
                            Data = sr.Data,
                            SessionId = connectionInfo.SessionId,
                            TimeoutOccurred = sr.TimeoutOccurred
                        };
                    }
                }

                //Clear Cache Service in memory
                var memoryCachingService = new MemoryCacheProvider();
                memoryCachingService.ClearCache(connectionInfo.SessionId);

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());

                throw;
            }
        }
        
        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        /// <param name="commandViewModel">The command view model.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Did not receive any json message in the post body.
        /// or
        /// EquipmentId is a required parameter.
        /// or
        /// Could not find any connection settings for the equipmentId.
        /// </exception>
        [Route("disconnectall/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage DisconnectAll([FromBody]CommandViewModel commandViewModel)
        {
            try
            {
                var response = new CommandResponseViewModel();
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                if (commandViewModel == null)
                    throw new Exception("Did not receive any json message in the post body.");

                if (commandViewModel.EquipmentId < 1)
                    throw new Exception("EquipmentId is a required parameter.");

                var memoryCachingService = CachingServiceFactory();
                foreach (var connectionInfo in memoryCachingService.FetchAll<ConnectionInfo>().Where(p=>p.ConnectionManagerService.EquipmentConnectionSetting.EquipmentId == commandViewModel.EquipmentId))
                {
                    //Now lock the thread so it cannot be used by any other thread.
                    using (var tlh = new ThreadLockHelper())
                    {
                        tlh.Lock(connectionInfo.LockObj, commandViewModel.Timeout);
                        // Do thread safe work here
                        if (connectionInfo.ConnectionManagerService.IsConnected)
                        {
                            var sr = connectionInfo.ConnectionManagerService.Disconnect();
                            response = new CommandResponseViewModel
                            {
                                Data = sr.Data,
                                SessionId = connectionInfo.SessionId,
                                TimeoutOccurred = sr.TimeoutOccurred
                            };
                        }
                    }

                    //Clear Cache Service in memory
                    memoryCachingService.ClearCache(connectionInfo.SessionId);
                }
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());

                throw;
            }
        }

        /// <summary>
        /// Retrieve all connections stored in Cache
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Did not receive any json message in the post body.
        /// or
        /// EquipmentId is a required parameter.</exception>
        [Route("connections/")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage CachedConnections(int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                if (equipmentId < 1)
                    throw new Exception("EquipmentId is a required parameter.");

                var memoryCachingService = CachingServiceFactory();
                var response = memoryCachingService.FetchAll<ConnectionInfo>()
                    .Where(p => p.ConnectionManagerService.EquipmentConnectionSetting.EquipmentId == equipmentId)
                    .Select(connectionInfo => new CommandResponseViewModel
                    {
                        SessionId = connectionInfo.SessionId
                    })
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());

                throw;
            }
        }
    }
}
