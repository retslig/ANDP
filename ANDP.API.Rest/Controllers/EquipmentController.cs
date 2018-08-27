using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using ANDP.API.Rest.Models;
using ANDP.Lib.Domain.Factories;
using ANDP.Lib.Domain.Interfaces;
using ANDP.Lib.Domain.Models;
using Common.Lib.Common.Email;
using Common.Lib.Common.Enums;
using Common.Lib.Extensions;
using Common.Lib.Interfaces;
using Common.Lib.MVC.Security.Claims;
using Common.Lib.Security;
using Thinktecture.IdentityModel.Authorization;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.API.Rest.Controllers
{
    /// <summary>
    /// This controller handles all requests for Usocs Translations, Dictionary Data, Equipment Settings and Company Settings.
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/equipment")]
    public class EquipmentController : ApiController
    {
        private IEquipmentService _equipmentService;
        private readonly ILogger _logger;
        private Guid _tenantId;
        private string _user;
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public EquipmentController(ILogger logger)
        {
            _name = Assembly.GetAssembly(this.GetType()).GetName().Name + " " +
                    Assembly.GetAssembly(this.GetType()).GetName().Version;
            _logger = logger;
        }

        /// <summary>
        /// The reason this is not in the constructor is solely for the purpose of exception handling.
        /// If you leave this in the controller and someone who is not authenticated calls the API you will not get a tenantId not found error.
        /// The error will be ugly and be hard to figure out you are not authorized.
        /// This way if the all methods have the ClaimsAuthorize attribute on them they will first be authenticated if not get a nice error message of not authorized.
        /// </summary>
        /// <exception cref="System.Exception">No Tenant Id Found.</exception>
        private void Setup()
        {
            //var isAllowed = ClaimsAuthorization.CheckAccess("Get", "CustomerId", "00");
            //isAllowed = true;
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var tenant = identity.Claims.Where(c => c.Type == ClaimsConstants.TenantIdClaimType).Select(c => c.Value).SingleOrDefault();

            if (string.IsNullOrEmpty(tenant))
                throw new Exception("No Tenant Id Found.");

            _tenantId = Guid.Parse(tenant);
            _user = identity.Identity.Name;

            _equipmentService = EquipmentServiceFactory.Create(_tenantId);
        }

        /// <summary>
        /// Retrieves the company identifier by external company identifier.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        [Route("~/api/company/companyid/{companyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveCompanyByCompanyId(int companyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.RetrieveCompanyByCompanyId(companyId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the company identifier by external company identifier.
        /// </summary>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("~/api/company/externalcompanyid/{externalCompanyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveCompanyIdByExternalCompanyId(string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the equipment details.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("equipmentid/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveEquipmentDetails(int equipmentId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var equipment = _equipmentService.RetrieveEquipmentByEquipmentId(equipmentId);

                //ConnectionInfo can contain sensitive information so adding extra layer of security here.
                var am = ClaimsAuthorization.AuthorizationManager as ApiAuthorizationManager;
                if (am == null || !am.CheckAccess(new List<string> { "ShowEquipmentConnectionSettings" }, new List<string> { "Equipment" }))
                {
                    equipment.EquipmentConnectionSettings = null;
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, equipment);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }


        /// <summary>
        /// Retrieves all equipment.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        [Route("company/{companyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveEquipmentByCompanyId(int companyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var equipment = _equipmentService.RetrieveEquipmentByCompanyId(companyId).ToList();

                //ConnectionInfo can contain sensitive information so adding extra layer of security here.
                var am = ClaimsAuthorization.AuthorizationManager as ApiAuthorizationManager;
                if (am == null || !am.CheckAccess(new List<string> { "ShowEquipmentConnectionSettings" }, new List<string> { "Equipment" }))
                {
                    foreach (var temp in equipment)
                    {
                        temp.EquipmentConnectionSettings = null;
                    }
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, equipment);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        #region USOC Translations

        /// <summary>
        /// Retrieves the usoc add translation.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="usocName">Name of the usoc.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("usoc/{usocName}/add/company/{companyId}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveUsocAddTranslation(int companyId, string usocName, int equipmentId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.RetrieveUsocAddTranslation(companyId, equipmentId, usocName);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the usoc remove translation.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="usocName">Name of the usoc.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("usoc/{usocName}/remove/company/{companyId}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveUsocRemoveTranslation(int companyId, string usocName, int equipmentId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.RetrieveUsocRemoveTranslation(companyId, equipmentId, usocName);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all usoc translations.
        /// equipmentId can be left out and it will return all entries regardless of value.
        /// active can be left out and it will return all entries regardless of value.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="active">The active.</param>
        /// <returns></returns>
        [Route("usoc/company/{companyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveUsocTranslations(int companyId, int? equipmentId = null, bool? active = null)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.RetrieveUsocTranslations(companyId, equipmentId, active);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the usoc translation.
        /// </summary>
        /// <param name="usocName">Name of the usoc.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("usoc/{usocName}/company/{companyId}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveUsocTranslation( string usocName, int companyId, int equipmentId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.RetrieveUsocTranslation(companyId, equipmentId, usocName);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the usoc translation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("usoc/id/{id}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveUsocTranslationById(int id)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.RetrieveUsocTranslationById(id);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Creates or updates the usoc translation.
        /// </summary>
        /// <param name="usocToCommandTranslation">The usoc to command translation.</param>
        /// <returns></returns>
        [Route("usoc/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateOrUpdateUsocTranslation([FromBody]UsocToCommandTranslation usocToCommandTranslation)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.CreateOrUpdateUsocTranslation(usocToCommandTranslation, _user);
                return this.Request.CreateResponse(HttpStatusCode.Created, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Deactivates the usoc translation by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("usoc/deactivate/id/{id}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeactivateUsocTranslationById(int id)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                _equipmentService.DeactivateUsocTranslationById(id, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Deletes the usoc translation by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("usoc/id/{id}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteUsocTranslationById(int id)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                _equipmentService.DeleteUsocTranslationById(id, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        #endregion

        #region DictionaryData

        /// <summary>
        /// Retrieves all dictionary data translations. 
        /// equipmentId can be left out and it will return all entries regardless of value.
        /// active can be left out and it will return all entries regardless of value.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="active">The active.</param>
        /// <returns></returns>
        [Route("datadictionary/company/{companyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveDictionaryDataTranslations(int companyId, int? equipmentId = null, bool? active = null)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.RetrieveDictionaryDataTranslations(companyId, equipmentId, active);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the data from data dictionary table.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="dataDictionaryDto">The data dictionary dto.</param>
        /// <returns></returns>
        [Route("datadictionary/company/{companyId}/equipmentid/{equipmentid}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveDictionaryDataTranslation(int companyId, int equipmentId, [FromUri]DataDictionaryDto dataDictionaryDto)
        {
            var dataDictionary = new DataDictionary
            {
                Active = dataDictionaryDto.Active,
                CompanyId = companyId,
                EquipmentId = equipmentId,
                Key1 = dataDictionaryDto.Key1,
                Key2 = dataDictionaryDto.Key2,
                Key3 = dataDictionaryDto.Key3,
                Key4 = dataDictionaryDto.Key4,
                Key5 = dataDictionaryDto.Key5,
                Key6 = dataDictionaryDto.Key6,
                Key7 = dataDictionaryDto.Key7,
                Key8 = dataDictionaryDto.Key8,
                Key9 = dataDictionaryDto.Key9
            };

            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.RetrieveDictionaryDataTranslation(dataDictionary);
                return this.Request.CreateResponse(HttpStatusCode.Created, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { dataDictionary }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the data from data dictionary table.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("datadictionary/id/{id}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveDictionaryDataTranslationById(int id)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.RetrieveDictionaryDataTranslationById(id);
                return this.Request.CreateResponse(HttpStatusCode.Created, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Creates  or updates the dictionary data translation.
        /// </summary>
        /// <param name="dataDictionary">The data dictionary.</param>
        /// <returns></returns>
        [Route("datadictionary/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateOrUpdateDictionaryDataTranslation([FromBody]DataDictionary dataDictionary)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var data = _equipmentService.CreateOrUpdateDictionaryDataTranslation(dataDictionary, _user);
                return this.Request.CreateResponse(HttpStatusCode.Created, data);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Deactivates the dictionary data translation by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("datadictionary/deactivate/id/{id}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeactivateDictionaryDataTranslationById(int id)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                _equipmentService.DeactivateDictionaryDataTranslationById(id, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Deletes the dictionary data translation by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("datadictionary/id/{id}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteDictionaryDataTranslationById (int id)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                _equipmentService.DeleteDictionaryDataTranslationById(id, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        #endregion

        /// <summary>
        /// Sends an email to the gmail support email.
        /// </summary>
        [Route("email/gmail/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateGmailEmail([FromBody] GmailEmaillDto gmailEmaillDto)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                var toEmailsList = new MailAddressCollection();
                foreach (var email in gmailEmaillDto.ToEmail.Split(new []{';', ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    toEmailsList.Add(new MailAddress(email));
                }

                Emailer.SendGmailSupportMessage(toEmailsList, gmailEmaillDto.Subject, gmailEmaillDto.Body);

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in WebAPI. Response {0}", HttpStatusCode.Created), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), 
                    new List<object> { ex.GetInnerMostExceptionWithEntityValidationInfo(), gmailEmaillDto.SerializeObjectToJsonString() }, 
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a log entry.
        /// </summary>
        [Route("log/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateLogEntry([FromBody] LogEntryDto logEntryDto)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, logEntryDto.Entry, logEntryDto.LogLevelType);
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
