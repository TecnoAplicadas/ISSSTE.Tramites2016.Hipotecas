#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Common.Web;
using ISSSTE.Tramites2016.Hipotecas.Domian;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using ISSSTE.Tramites2016.Hipotecas.Domians;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Controllers
{
    /// <summary>
    ///     Controllador para las peticiones de solicitud
    /// </summary>
    [RoutePrefix("api/Request")]
    public class RequestApiController : BaseApiController
    {
        #region Constants

        /// <summary>
        ///     Nombre del encabezado utilizado para enviar la identidad del usuario
        /// </summary>
        private const string UserIdentityHeader = "Issste-Tramites2016-UserIdentity";

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor del controllador
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="requestDomainService"></param>
        /// <param name="entitleDomainService"></param>
        /// <param name="calendarDomainService"></param>
        /// <param name="commonDomainService"></param>
        public RequestApiController(ILogger logger, IRequestDomainService requestDomainService,
            IEntitleDomainService entitleDomainService, ICalendarDomainService calendarDomainService,
            ICommonDomainService commonDomainService) : base(logger)
        {
            _requestDomainService = requestDomainService;
            _entitleDomainService = entitleDomainService;
            _calendarDomainService = calendarDomainService;
            _commonDomainService = commonDomainService;
        }

        #endregion

        #region Fields

        /// <summary>
        ///     Dominio de solicitudes
        /// </summary>
        private readonly IRequestDomainService _requestDomainService;

        /// <summary>
        ///     Dominio de Derechohabiente
        /// </summary>
        private readonly IEntitleDomainService _entitleDomainService;

        /// <summary>
        ///     Dominio de Calendario
        /// </summary>
        private readonly ICalendarDomainService _calendarDomainService;

        /// <summary>
        ///     Dominio Comun
        /// </summary>
        private readonly ICommonDomainService _commonDomainService;

        /// <summary>
        ///     Identidad del usuario enviado en la petición
        /// </summary>
        private string _userIdentity;

        #endregion

        #region Properties

        /// <summary>
        ///     Valida si en la petición se envío la identidad del usuario
        /// </summary>
        private bool HasUserIdentityInRequest
        {
            get
            {
                if (String.IsNullOrEmpty(_userIdentity))
                    return false;
                return true;
            }
        }

        /// <summary>
        ///     Se obtiene la identidad del usuario que realizo la petición
        /// </summary>
        private string UserIdentity
        {
            get { return _userIdentity; }
        }

        #endregion

        #region Actions

        /// <summary>
        ///     Obtiene las solicitudes por su id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Request))]
        [HttpGet, Route("Request/{requestId}")]
        public async Task<HttpResponseMessage> GetRequestById(Guid requestId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetRequestByRequestId(requestId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la solicitud por Id con informacion del derechohabiente
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [ResponseType(typeof(RequestGeneric))]
        [HttpGet, Route("RequestAllEntitle/{requestId}/{state}")]
        public async Task<HttpResponseMessage> GetRequestAllEntitleById(Guid requestId, string state)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetRequestCompleteEntitleByRequestId(requestId, state);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Guarda una nueva solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ResponseType(typeof(Request))]
        [HttpPost, Route("Save")]
        public async Task<HttpResponseMessage> SaveRequest(Request request)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                int? status = 0;
                //Validation val = null;
                TimeContribution tc = null;
                var ent = await _entitleDomainService.GetEntitleById(UserIdentity);

                var req = new Request();
                if (request.RequestId == Guid.Empty)
                {
                    //  req.Date = DateTime.Now;
                    req.Folio = "PEN" + ent.NoISSSTE.PadLeft(10, '0') + DateTime.Now.ToString("yyMMddhhmmss");
                }
                else
                {
                    req = await _requestDomainService.GetRequestByRequestId(request.RequestId);

                }


                //  req.Description = request.Description;
                if (request.EntitleId != null)
                {
                    req.EntitleId = request.EntitleId;
                }
                else
                {
                    req.EntitleId = ent.CURP;
                }


                //   req.IsAssigned = request.IsAssigned;
                request.Folio = req.Folio;

                req.IsComplete = status == 100 ? true : false;
                var result = await _requestDomainService.SaveRequest(req);
                var res = await _requestDomainService.SaveStatusRequestByEntitle(req, true, status, ent.CURP);
                if (!(bool)req.IsComplete) //0 en BD
                    await _requestDomainService.SaveStatusRequestByEntitle(req, true, (int)StatusEnum.EnesperadeAgendarCiraDer, ent.CURP);
                //if (val != null); MFP 10-01-2017
                //req.Validation = val; 
                return Request.CreateResponse(HttpStatusCode.OK, req);
            });
        }

        /// <summary>
        ///     Obtiene las solicitudes por el id del derechohabiente
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(RequestGeneric))]
        [HttpGet, Route("RequestEntitled")]
        public async Task<HttpResponseMessage> GetRequestByEntitledId()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetRequestsByEntitleId(UserIdentity, true);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtienen las notificaciones para el derechohabiente
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(RequestGeneric))]
        [HttpGet, Route("Notifications")]
        public async Task<HttpResponseMessage> GetLastNotification()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var config = await _commonDomainService.GetConfiguration("NotificationsNUmber");
                var result = await _requestDomainService.GetRequestsByEntitleId(UserIdentity, true);
                if (config != null)
                    result = result.Take(int.Parse(config.Value)).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene las Solicitudes Actuales
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(int))]
        [HttpGet, Route("CurrentRequest")]
        public async Task<HttpResponseMessage> GetCurrentRequestByEntitledId()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetRequestsByEntitleId(UserIdentity, true);
                return Request.CreateResponse(HttpStatusCode.OK, result.Count);
            });
        }

        /// <summary>
        ///     Obtiene las solicitudes pasadas
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<RequestGeneric>))]
        [HttpGet, Route("PastRequestEntitled")]
        public async Task<HttpResponseMessage> GetPastRequestByEntitledId()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetRequestsByEntitleId(UserIdentity, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene las Citas no atendidas
        /// </summary>
        /// <param name="requestid"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<Appoinment>))]
        [HttpGet, Route("AppointmentsNotAttended")]
        public async Task<HttpResponseMessage> GetAppointmentsByRequestIdNotAttended(Guid requestid)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetAppointmentsByRequestId(requestid);
                result = result.Where(r => r.Date <= DateTime.Now).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene las citas actuales
        /// </summary>
        /// <param name="requestid"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<Appoinment>))]
        [HttpGet, Route("CurrentAppointment")]
        public async Task<HttpResponseMessage> GetAppointmentsByRequestId(Guid requestid)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetAppointmentsByRequestId(requestid);
                var res = result.FirstOrDefault(r => r.Date >= DateTime.Now);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }

        /// <summary>
        ///     Obtiene el diagnostico y la solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [ResponseType(typeof(OpinionRequestApi))]
        [HttpGet, Route("OpinionAndRequest/{requestId}/{state}")]
        public async Task<HttpResponseMessage> GetOpinionAndRequest(Guid requestId, string state)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var opinionReq = new OpinionRequestApi();
                opinionReq.Request = await _requestDomainService.GetRequestCompleteEntitleByRequestId(requestId, state);
                opinionReq.Opinion = await _requestDomainService.GetOpinionByRequest(requestId);
                return Request.CreateResponse(HttpStatusCode.OK, opinionReq);
            });
        }

        /// <summary>
        ///     Guarda el status de la solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="idStatus"></param>
        /// <param name="happy"></param>
        /// <returns></returns>
        [ResponseType(typeof(RequestGeneric))]
        [HttpGet, Route("SaveStatusRequest/{requestId}/{idStatus}/{happy}")]
        public async Task<HttpResponseMessage> SaveStatusRequest(Guid requestId, int? idStatus, bool happy)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var req = await _requestDomainService.GetRequestByRequestId(requestId);
                var result = await _requestDomainService.SaveStatusRequestByEntitle(req, happy, idStatus, UserIdentity);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        // Archivo: RequestApiController.cs
        /// <summary>
        ///     Obtiene las Solicitudes Actuales
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(int))]
        [HttpGet, Route("CurpRequest")]
        public async Task<HttpResponseMessage> GetCurpRequest()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetCurpRequest(UserIdentity, false);
                return Request.CreateResponse(HttpStatusCode.OK, result.Count);
            });
        }

        /// <summary>
        /// Obtiene la lista de documentos para el solicitante
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(RelationshipDocument))]
        [HttpGet, Route("DocumentsByRelationship/{Pensions}/{KeyRelationships}")]
        public async Task<HttpResponseMessage> GetDocumentsByRelationship(int Pensions, string KeyRelationships)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetDocumentsByRelationship(Pensions, KeyRelationships);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        /// Obtiene la modalidad de la pensión
        /// </summary>
        /// <param name="KeyRelationships"></param>
        /// <returns></returns>
        [ResponseType(typeof(RelationshipDocument))]
        [HttpGet, Route("ModalidadPension/{KeyRelationships}")]
        public async Task<HttpResponseMessage> GetModalidadPension(string KeyRelationships)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetModaliadPension(KeyRelationships);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        /// Obtiene todas la solicitudes creadas por el Deudo 1.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(RequestGeneric))]
        [HttpGet, Route("RequestDebtors")]
        public async Task<HttpResponseMessage> GetRequestDebtors()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetRequestsDebtor(UserIdentity, true);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        /// Obtiene todas la solicitudes creadas por el Deudo.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(RequestGeneric))]
        [HttpGet, Route("RequestDebtorsRequestId/{requestId}")]
        public async Task<HttpResponseMessage> GetRequestDebtorsRequestId(Guid requestId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetRequestDebtorsRequestId(requestId, true);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        #endregion

        #region Helper Methods

        /// <summary>
        ///     Valida que la petición contenga la identidad del usuario, a la vez que ejecuta cóigo asíncrono para manejar
        ///     excepciones no controladas
        /// </summary>
        /// <param name="operationBody">Cuerpo a ejecutar si se valido la petición</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        private async Task<HttpResponseMessage> ValidateAndHandleOperationExecutionAsync(
            Func<Task<HttpResponseMessage>> operationBody)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                HttpResponseMessage response = null;

                //Se obtiene la identidad del usuario desde el header
                if (Request != null && Request.Headers.Any(h => h.Key == UserIdentityHeader))
                    _userIdentity = Request.Headers.GetValues(UserIdentityHeader).First();

                if (!HasUserIdentityInRequest)
                    response = CreateStringResponseMessage(HttpStatusCode.Unauthorized, "Usuario no válido.");
                else
                    response = await operationBody();

                return response;
            });
        }

        /// <summary>
        ///     Valida que la petición contenga la identidad del usuario, a la vez que ejecuta cóigo para manejar excepciones no
        ///     controladas
        /// </summary>
        /// <param name="operationBody">Cuerpo a ejecutar si se valido la petición</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        private HttpResponseMessage ValidateAndHandleOperationExecution(Func<HttpResponseMessage> operationBody)
        {
            var validationTask = ValidateAndHandleOperationExecutionAsync(async () => operationBody());

            validationTask.Wait();

            return validationTask.Result;
        }

        /// <summary>
        ///     Valida que la petición contenga la identidad del usuario, a la vez que ejecuta cóigo asíncrono para manejar
        ///     excepciones no controladas y envía al código a ejecutar los valores obtenidos del multipart
        /// </summary>
        /// <param name="operationBody">Cuerpo a ejecutar si se valido la petición</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        protected async Task<HttpResponseMessage> ValidateAndHandleMultipartOperationExecutionAsync(
            Func<HttpPostedData, Task<HttpResponseMessage>> operationBody)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                HttpResponseMessage response = null;

                //Se obtiene el Expediente del multipart
                var multipartData = await Request.Content.ParseMultipartAsync();

                _userIdentity = multipartData.Fields[UserIdentityHeader].Value;

                if (!HasUserIdentityInRequest)
                {
                    response = CreateStringResponseMessage(HttpStatusCode.Unauthorized, "Usuario no válido.");

                    return response;
                }
                response = await operationBody(multipartData);

                return response;
            });
        }

        #endregion
    }
}