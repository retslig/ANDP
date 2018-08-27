
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using ANDP.API.Rest.Models;
using Common.Lib.Common.Enums;
using Common.Lib.Extensions;
using Common.Lib.Interfaces;
using Common.Lib.Security;
using Common.NewNet.Factories;
using Common.ProcessMakerService;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.API.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/newnet")]
    public class NewNetWorkflowController : ApiController
    {
        private readonly ILogger _logger;
        private Guid _tenantId;
        private string _user;
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewNetWorkflowController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public NewNetWorkflowController(ILogger logger)
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

        }

        /// <summary>
        /// Routes the case.
        /// </summary>
        /// <param name="workFlowCaseDto">The work flow case dto.</param>
        /// <returns></returns>
        [Route("workflow/routecase")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage RouteCase([FromBody] WorkFlowCaseDto workFlowCaseDto)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { workFlowCaseDto },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Info);

                var service = ProcessMakerServiceFactory.Create(_tenantId, workFlowCaseDto.EquipmentId);
                service.RouteCase(workFlowCaseDto.CaseId, workFlowCaseDto.Index);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { workFlowCaseDto },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Error,
                    ex.GetInnerMostException());
                throw;
            }
        }

        /// <summary>
        /// Creates the case note.
        /// </summary>
        /// <param name="workFlowNoteDto">The work flow note dto.</param>
        /// <returns></returns>
        [Route("workflow/note")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateCaseNote([FromBody] WorkFlowNoteDto workFlowNoteDto)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> {workFlowNoteDto},
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Info);

                var service = ProcessMakerServiceFactory.Create(_tenantId, workFlowNoteDto.EquipmentId);
                service.CreateCaseNote(workFlowNoteDto.CaseId, workFlowNoteDto.ProcessId, workFlowNoteDto.TaskId, workFlowNoteDto.UserId, workFlowNoteDto.Note);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> {workFlowNoteDto},
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Error,
                    ex.GetInnerMostException());
                throw;
            }
        }

        /// <summary>
        /// Creates the case note.
        /// </summary>
        /// <param name="workFlowResponseDto">The work flow response dto.</param>
        /// <returns></returns>
        [Route("workflow/response")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateCaseResponse([FromBody] WorkFlowResponseDto workFlowResponseDto)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { workFlowResponseDto },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Info);

                var service = ProcessMakerServiceFactory.Create(_tenantId, workFlowResponseDto.EquipmentId);
                service.CreateResponse(workFlowResponseDto.CaseId, workFlowResponseDto.ResponseMessage,
                    workFlowResponseDto.ResponseCode,
                    workFlowResponseDto.Dictionary.Select(dic => new variableListStruct
                    {
                        value = dic.Value,
                        name = dic.Key
                    }).ToList());
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { workFlowResponseDto },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Error,
                    ex.GetInnerMostException());
                throw;
            }
        }

        /// <summary>
        /// Creates the case document.
        /// </summary>
        /// <param name="workFlowDocumentDto">The work flow note dto.</param>
        /// <returns></returns>
        [Route("workflow/document")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateCaseDocument([FromBody] WorkFlowDocumentDto workFlowDocumentDto)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { workFlowDocumentDto },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Info);

                var service = ProcessMakerServiceFactory.Create(_tenantId, workFlowDocumentDto.EquipmentId);
                service.AttachDocument();
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { workFlowDocumentDto },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Error,
                    ex.GetInnerMostException());
                throw;
            }
        }

        /// <summary>
        /// Executes the trigger.
        /// </summary>
        /// <param name="workFlowTriggerDto">The work flow trigger dto.</param>
        /// <returns></returns>
        [Route("workflow/trigger")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage ExecuteTrigger([FromBody] WorkFlowTriggerDto workFlowTriggerDto)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { workFlowTriggerDto },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Info);

                var service = ProcessMakerServiceFactory.Create(_tenantId, workFlowTriggerDto.EquipmentId);
                service.ExecuteTrigger(workFlowTriggerDto.CaseId, workFlowTriggerDto.TriggerId, workFlowTriggerDto.Index);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { workFlowTriggerDto },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Error,
                    ex.GetInnerMostException());
                throw;
            }
        }

        /// <summary>
        /// Retrieves the cases.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("workflow/cases/equipmentId/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveCases(int equipmentId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { equipmentId },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Info);

                var service = ProcessMakerServiceFactory.Create(_tenantId, equipmentId);
                var list = service.RetrieveCaseList().SerializeObjectToString();
                return this.Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { equipmentId },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Error,
                    ex.GetInnerMostException());
                throw;
            }
        }
    }
}
