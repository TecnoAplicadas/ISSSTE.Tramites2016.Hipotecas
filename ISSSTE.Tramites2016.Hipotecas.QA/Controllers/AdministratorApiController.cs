//####################################################################
//      ## Fecha de creación: 18-03-2016
//      ## Fecha de última modificación: 18-03-2016
//      ## Responsable: Emanuel De la Isla Vértiz
//      ## Módulos asociados: Información general, Deudos, Beneficiarios, Historial Laboral.
//      ## Id Tickets asociados al cambio: R-013042
//####################################################################
#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ISSSTE.Tramites2016.Common.Model;
using ISSSTE.Tramites2016.Common.Security.Core;
using ISSSTE.Tramites2016.Common.Security.Helpers;
using ISSSTE.Tramites2016.Common.Security.Model;
using ISSSTE.Tramites2016.Common.Security.ServiceAgents.Implementation;
using ISSSTE.Tramites2016.Common.ServiceAgents;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Common.Web;
using ISSSTE.Tramites2016.Common.Web.Http;
using ISSSTE.Tramites2016.Hipotecas.Domian;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;
using ISSSTE.Tramites2016.Hipotecas.Model.Enums;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using ISSSTE.Tramites2016.Hipotecas.Domian.Resources;
using ISSSTE.Tramites2016.Hipotecas.Domians;
using Newtonsoft.Json;
using ISSSTE.Tramites2016.Common.Catalogs;
using System.Data.Entity.Infrastructure;
using ISSSTE.Tramites2016.Hipotecas.Resources;
using System.Web;
//using ISSSTE.Tramites2016.Estancias.Model.Api;
#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Controllers
{
    /// <summary>
    ///     Controllador del Api para el Administrador
    /// </summary>
    [AuthorizeByConfig("AllAdminRoles")]
    //[Authorize(Roles = "Del. Jefatura de Departamento de Hipotecas")]
    [RoutePrefix("api/Administrator")]

    public class AdministratorApiController : BaseApiController
    {
        #region Constants

        /// <summary>
        ///     Nombre del encabezado utilizado para enviar la identidad del usuario
        /// </summary>
        private const string UserIdentityHeader = "Issste-Tramites2016-UserIdentity";
        private readonly ISipeAvDataServiceAgent _sipeAv;
        private const string MexicoDateFormat = "dd/MM/yyyy";
        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor del Controllador
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="entitleDomainService"></param>
        /// <param name="requestDomainService"></param>
        /// <param name="calendarDomainService"></param>








        public AdministratorApiController(ILogger logger, IEntitleDomainService entitleDomainService,
            IRequestDomainService requestDomainService, ICalendarDomainService calendarDomainService,
            ICommonDomainService commonDomainService, ISipeAvDataServiceAgent sipeAv, ICatalogReflexionHelper catalogReflexionHelper,
            ICatalogRepository catalogRepository) : base(logger)
        {
            _entitleDomainService = entitleDomainService;
            _requestDomainService = requestDomainService;
            _calendarDomainService = calendarDomainService;
            _commonDomainService = commonDomainService;
            _sipeAv = sipeAv;
            _catalogReflexionHelper = catalogReflexionHelper;
            _catalogRepository = catalogRepository;
        }

        #endregion

        #region Fields

        /// <summary>
        ///     Dominio de solicitudes
        /// </summary>
        private readonly IRequestDomainService _requestDomainService;

        /// <summary>
        ///     Dominiio de derechohabiente
        /// </summary>
        private readonly IEntitleDomainService _entitleDomainService;

        /// <summary>
        ///     Dominio del calendario
        /// </summary>
        private readonly ICalendarDomainService _calendarDomainService;

        private readonly ICommonDomainService _commonDomainService;
        /// <summary>
        /// Contiene las rutinas para utilizar reflexion sobre los catalogos
        /// </summary>
        private ICatalogReflexionHelper _catalogReflexionHelper;
        /// <summary>
        /// Contiene las rutinas para el manejo generico de catálogos
        /// </summary>
        private ICatalogRepository _catalogRepository;


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
        ///     Se utiliza para validar que el token de autenticación sigue siendo válido
        /// </summary>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(void))]
        [HttpGet, Route("Token/Validate")]
        public HttpResponseMessage ValidateLogin()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Obtiene los reles a los que pertenece el usuario
        /// </summary>
        /// <returns>Roles</returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<string>))]
        [HttpGet, Route("User/Roles")]
        public HttpResponseMessage GetUserRoles()
        {
            return HandleOperationExecution(() =>
            {
                var owinContext = Request.GetOwinContext();

                var roles = owinContext.GetAuthenticatedUserRoles();
                var rolesNames = roles.Select(r => r.Name).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, rolesNames);
            });
        }

        /// <summary>
        /// Obtiene la lista de estados a los cuales se puede mover la solicitud, para el rol que tenga el usuario
        /// </summary>
        /// <returns>lista de estados a los cuales se puede mover la solicitud, para el rol que tenga el usuario</returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<Status>))]
        [HttpPost, Route("GetAvailableNextStatus")]
        public async Task<HttpResponseMessage> GetAvailableNextStatus(Models.CheckNextStatus par)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                List<String> role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name).ToList();
                var result = await _requestDomainService.GetAvailableNextStatus(par.requestId, role);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(void))]
        [HttpPost, Route("SaveTempObservations")]
        public HttpResponseMessage TempObservations(String[] Observations)
        {
            return  this.HandleOperationExecution( () => 
            {
                HttpContext context = HttpContext.Current;
                context.Session["Observations"] = Observations;
                context.Session["StateObservations"] = true;

              return Request.CreateResponse(HttpStatusCode.OK);
          });

        }

        //Cambia el estatus del request
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(void))]
        [HttpPost, Route("ChangeRequestStatus")]
        public async Task<HttpResponseMessage> ChangeRequestStatus([FromBody] Models.UpdateRequestStatus data)
        {
            return await this.HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var user = owinContext.GetAuthenticatedUser();
                List<String> role = owinContext.GetAuthenticatedUserAuthorizedRoles().Select(x => x.Name).ToList();
                var result = await _requestDomainService.ChangeRequestStatusAsync(data.requestId, data.newStatus, role);

                if (!result.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                if (data.newStatus == (int)StatusEnum.CitaatendidacotejodedocumentosOP)
                {
                    var resultApp = await _calendarDomainService.GetAppointmentsByRequestId(data.requestId);
                    foreach (Appoinment app in resultApp)
                        // await ChangueStatusAppointment(app.AppoinmentId, true, data.requestId);
                        await _requestDomainService.ChangueStatusAppointment(app.AppoinmentId, true);
                }



                //if (data.newStatus == (int)StatusEnum.CitaNoAsistidaOP)
                //{
                //    var resultApp = await _calendarDomainService.GetAppointmentsByRequestId(data.requestId);
                //    foreach (Appoinment app in resultApp)
                //        ChangueStatusAppointment(app.AppoinmentId, false, data.requestId);
                //}
                return Request.CreateResponse(HttpStatusCode.OK);
            });
        }
        /// <summary>
        ///     Asigna la solicitud a un usuario
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost, Route("AsingRequest")]
        public async Task<HttpResponseMessage> AsingRequest(UserRequestApi userrequest)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var user = owinContext.GetAuthenticatedUser();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var res = await _requestDomainService.AsingRequest(userrequest.RequestId, Guid.Parse(userrequest.User));
                var req = await _requestDomainService.GetRequestByRequestId(userrequest.RequestId);

                //var result =
                //    await
                //        _requestDomainService.SaveStatusRequest(req, true, (int)StatusEnum.SolicitudPendiente,
                //            user.UserName, role);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="Observations"></param>
        /// <param name="IsValid"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(bool))]
        [HttpPost, Route("Documents")]
        //public async Task<HttpResponseMessage> UpdateDocuments(string RequestId, string Observations, bool IsValid)
        public async Task<HttpResponseMessage> UpdateDocuments(DocumentData documentData)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.UpdateDocumentByRequestId(documentData);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la lista de solicitudes paginando y filtrando por un texto y un estatus
        /// </summary>
        /// <param name="pageSize">Cantidad de registros por página</param>
        /// <param name="page">Página solicitada</param>
        /// <param name="query">Texto de busqueda</param>
        /// <param name="statusId">Ide del estatus por el cual filtrar</param>

        /// <param name="orden"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<PagedRequestsResult>))]
        [HttpGet, Route("Requests")]
        public async Task<HttpResponseMessage> GetRequests(int pageSize, int page, bool orden, string query = null,
            int? statusId = null)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                HttpContext context = HttpContext.Current;

                context.Session["StateObservations"] = false;
                context.Session["Observations"] = null;

                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var user = owinContext.GetAuthenticatedUser();
                var delegation = user.Properties.Exists(r => r.Name == IsssteUserPropertyTypes.Delegation)
                    ? int.Parse(user.Properties.First(r => r.Name == IsssteUserPropertyTypes.Delegation).Value)
                    : 0;
                var delegations = user.GetDelegations();

                var result =
                    await
                        _requestDomainService.GetPagedRequestsAsync(pageSize, page, role, delegations, user.UserName, orden,
                            statusId, query);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        /// Obtiene la lista de citas, utilizando paginacion y filtro de busqueda.
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="query"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(PagedInformation<AppointmentsResult>))]
        [HttpGet, Route("Appointments")]
        public async Task<HttpResponseMessage> GetAppointments(int pageSize, int page, string query = null,
            DateTime? date = null, TimeSpan? time = null, int? statusId = null)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var user = owinContext.GetAuthenticatedUser();
                var delegation = user.Properties.Exists(r => r.Name == IsssteUserPropertyTypes.Delegation)
                    ? int.Parse(user.Properties.First(r => r.Name == IsssteUserPropertyTypes.Delegation).Value)
                    : 0;
                var delegations = user.GetDelegations();

                var result = await _calendarDomainService.GetPagetAppointmentsAsync(pageSize, page, role, delegations, user.UserName, date, query, time, statusId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(PagedInformation<AppointmentsResult>))]
        [HttpGet, Route("AppointmentsRequest")]
        public async Task<HttpResponseMessage> GetAppointmentsByRequest(Guid RequestId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var user = owinContext.GetAuthenticatedUser();
                var delegation = user.Properties.Exists(r => r.Name == IsssteUserPropertyTypes.Delegation)
                    ? int.Parse(user.Properties.First(r => r.Name == IsssteUserPropertyTypes.Delegation).Value)
                    : 0;
                var delegations = user.GetDelegations();

                var result = await _calendarDomainService.GetPagetAppointmentsByRequestAsync(role, delegations, RequestId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        ///     Obtiene toda la informacion de la solicitud y derechohabiente
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(EntitledData))]
        [HttpGet, Route("AllInformation/{requestId}")]
        public async Task<HttpResponseMessage> GetAllDataFromEntitleNewRequest(Guid requestId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = new EntitledData();
                var request = await _requestDomainService.GetRequestByRequestId(requestId);


                result.Entitle = await _entitleDomainService.GetEntitleByCurp(request.EntitleId.ToString());
                var regimen = await _sipeAv.GetRegimenByNoIsssteAsync(result.Entitle.NoISSSTE);

                // result.Validation = await _entitleDomainService.GetValidationByRequest(requestId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la solicitud por id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(RequestGeneric))]
        [HttpGet, Route("Request")]
        public async Task<HttpResponseMessage> GetRequest(Guid requestId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var user = owinContext.GetAuthenticatedUser();
                var delegation = user.Properties.Exists(r => r.Name == IsssteUserPropertyTypes.Delegation)
                    ? int.Parse(user.Properties.First(r => r.Name == IsssteUserPropertyTypes.Delegation).Value)
                    : 0;

                var result = await _requestDomainService.GetRequestCompleteByRequestId(requestId, role);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la lista de Diagnositocs por id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<OpinionMessage>))]
        [HttpGet, Route("OpinionMessages")]
        public async Task<HttpResponseMessage> GetOpinionMessages(Guid requestId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var user = owinContext.GetAuthenticatedUser();
                var delegations = user.GetDelegations();
                var delegation = user.Properties.Exists(r => r.Name == IsssteUserPropertyTypes.Delegation)
                    ? int.Parse(user.Properties.First(r => r.Name == IsssteUserPropertyTypes.Delegation).Value)
                    : 0;

                var result = await _requestDomainService.GetOpinionMessages();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obitnene el diagnostico y la informaciond ela solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(OpinionRequestApi))]
        [HttpGet, Route("OpinionAndRequest/{requestId}")]
        public async Task<HttpResponseMessage> GetOpinionAndRequest(Guid requestId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var opinionReq = new OpinionRequestApi();
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                opinionReq.Request = await _requestDomainService.GetRequestCompleteByRequestId(requestId, role);
                opinionReq.Opinion = await _requestDomainService.GetOpinionByRequest(requestId);
                return Request.CreateResponse(HttpStatusCode.OK, opinionReq);
            });
        }

        /// <summary>
        ///     Guarda el Estatus de la solicitud
        /// </summary>
        /// <param name="staOpinion"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost, Route("SaveStatusRequest")]
        public async Task<HttpResponseMessage> SaveStatusRequest(StatusOpinionApi staOpinion)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var rol = await _requestDomainService.GetRole(role);
                //if (staOpinion.idStatus == 1 && (int)RolEnum.JefeOperadores == rol.RoleId)
                //    staOpinion.idStatus = (int)StatusEnum.EnEsperadeAprobacióndeDictamenJop;
                //if (staOpinion.idStatus == 1 && (int)RolEnum.Operador == rol.RoleId)
                //    staOpinion.idStatus = (int)StatusEnum.EnEsperadeAprobacióndeDictamen;
                //if (staOpinion.idStatus == 2 && (int)RolEnum.JefeOperadores == rol.RoleId)
                //    staOpinion.idStatus = (int)StatusEnum.SolicitudRechazadaJop;
                //if (staOpinion.idStatus == 2 && (int)RolEnum.Operador == rol.RoleId)
                //    staOpinion.idStatus = (int)StatusEnum.SolicitudRechazada;
                //if (staOpinion.idStatus == 3 && (int)RolEnum.JefeOperadores == rol.RoleId)
                //    staOpinion.idStatus = (int)StatusEnum.SolicitudRechazadaJop;
                //if (staOpinion.idStatus == 3 && (int)RolEnum.Operador == rol.RoleId)
                //    staOpinion.idStatus = (int)StatusEnum.SolicitudRechazada;

                var user = owinContext.GetAuthenticatedUser();
                var delegation = user.Properties.Exists(r => r.Name == IsssteUserPropertyTypes.Delegation)
                    ? int.Parse(user.Properties.First(r => r.Name == IsssteUserPropertyTypes.Delegation).Value)
                    : 0;
                var req = await _requestDomainService.GetRequestByRequestId(staOpinion.requestId);
                var op =
                    await
                        _requestDomainService.SaveOpinion(new Opinion
                        {
                            Opinion1 = staOpinion.opinion,
                            RequestId = staOpinion.requestId
                        });
                var result =
                    await
                        _requestDomainService.SaveStatusRequest(req, staOpinion.happy, staOpinion.idStatus,
                            user.UserName, role);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la lista de citas para la delegacion
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<PagedRequestsResult>))]
        [HttpGet, Route("Dates")]
        public async Task<HttpResponseMessage> GetDates(int pageSize, int page, string query = null,
           DateTime? date = null, TimeSpan? time = null)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var user = owinContext.GetAuthenticatedUser();
                var delegations = user.GetDelegations();
                //var delegation = user.Properties.Exists(r => r.Name == IsssteUserPropertyTypes.Delegation)
                //    ? int.Parse(user.Properties.First(r => r.Name == IsssteUserPropertyTypes.Delegation).Value)
                //    : 0;


                var result =
                    await
                        _requestDomainService.GetPagedDatesAsync(pageSize, page, delegations,
                            user.UserName, date, time, role, query);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Cambia el estatus de la cita
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="isAttended"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<int>))]
        [HttpGet, Route("Changue")]
        public async Task<HttpResponseMessage> ChangueStatusAppointment(Guid appointmentId, bool isAttended,
            Guid requestId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var user = owinContext.GetAuthenticatedUser();
                var delegation = user.Properties.Exists(r => r.Name == IsssteUserPropertyTypes.Delegation)
                    ? int.Parse(user.Properties.First(r => r.Name == IsssteUserPropertyTypes.Delegation).Value)
                    : 0;
                var statusId = 0;
                var result = await _requestDomainService.ChangueStatusAppointment(appointmentId, isAttended);
                var req = await _requestDomainService.GetRequestByRequestId(requestId);
                var rol = await _requestDomainService.GetRole(role);
                if (rol.RoleId == (int)RolEnum.JefeOperadores && isAttended)
                    statusId = (int)StatusEnum.CitacerradaOP;
                if (rol.RoleId == (int)RolEnum.Operador && isAttended)
                    statusId = (int)StatusEnum.CitacerradaOP;


                if (rol.RoleId == (int)RolEnum.JefeOperadores && !isAttended)
                    statusId = (int)StatusEnum.EnesperadeagendarcitaOP;
                if (rol.RoleId == (int)RolEnum.Operador && !isAttended)
                    statusId = (int)StatusEnum.EnesperadeagendarcitaOP;
                var res = await _requestDomainService.SaveStatusRequest(req, isAttended, statusId, user.UserName, role);
                if (isAttended)
                {
                    await _requestDomainService.SendEmailQuiz(requestId);
                }

                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }


        /// <summary>
        ///     Obtiene la lista de estatus
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<Status>))]
        [HttpGet, Route("Status")]
        public async Task<HttpResponseMessage> GetStatusByRole()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var user = owinContext
                .GetAuthenticatedUser();

                var result = await _requestDomainService.GetStatusByRole(role);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la lista de estatus
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<Status>))]
        [HttpGet, Route("DateStatus")]
        public async Task<HttpResponseMessage> GetDateStatusByRole()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var user = owinContext
                .GetAuthenticatedUser();

                var result = await _requestDomainService.GetDateStatusByRole(role);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la lista de dlegaciones
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<Delegation>))]
        [HttpGet, Route("AllDelegation")]
        public async Task<HttpResponseMessage> GetAllDelegation()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetAllDelegation();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }



        /// <summary>
        ///     Obtiene a los usuarios operador
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<SimpleUser>))]
        [HttpGet, Route("UsersOperator")]
        public async Task<HttpResponseMessage> GetUserOperator()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var users = new List<SimpleUser>();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var rsa = new SecurityRolesServiceAgent();
                var usa = new SecurityUsersServiceAgent();
                if (role == ConfigurationManager.AppSettings["roloperador"])
                {
                    var roleId =
                        await rsa.GetRoleIdByNameAsync(owinContext, ConfigurationManager.AppSettings["roloperador"]);

                    if (roleId != null)
                    {
                        users =
                            await
                                usa.GetUsersByProcedureAndRoleAsync(owinContext, Guid.Parse(Startup.ProcedureId),
                                    roleId.Value);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, users);
            });
        }

        /// <summary>
        ///     Regresa las listas de configuracion para el calendario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(CalendarList))]
        [HttpGet, Route("AdmninistratorSchedule/{id}")]
        public async Task<HttpResponseMessage> GetSheduleList(int id)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = new CalendarList();
                result.Schedules = await _calendarDomainService.GetScheduleByAdministrator(id);
                result.NonLaborableDays = await _calendarDomainService.GetNonLaborableDaysAdministrator(id);
                result.SpecialSchedules = await _calendarDomainService.GetSpecialDaysDateAdministrator(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Borra la configuracion por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpGet, Route("DeleteSchedule/{id}")]
        public async Task<HttpResponseMessage> DeleteSchedule(Guid id)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.DeleteSchedule(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        /// Borra los horarios asignados al día especial
        /// </summary>
        /// <param name="specialScheduleDays"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpGet, Route("DeleteSpecialScheduleDays/{specialScheduleDays}")]
        public async Task<HttpResponseMessage> DeleteSpecialScheduleDays(Guid specialScheduleDays)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.DeleteSpecialScheduleDays(specialScheduleDays);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        /// Elimina el horario por medio del Id de la lista de días especiales.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<Appoinment>))]
        [HttpGet, Route("DeleteSpecialDaysSchedules/{id}")]
        public async Task<HttpResponseMessage> DeleteSpecialDaysSchedules(Guid id)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.DeleteSpecialDaysSchedules(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///Obtiene el total de solicitudes Exitosas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<Appoinment>))]
        [HttpGet, Route("successfulRequest")]
        public async Task<HttpResponseMessage> successfulRequest(Guid id)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.DeleteSpecialDaysSchedules(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Borra el dia no laborable
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost, Route("DeleteNonLaboraleDays")]
        public async Task<HttpResponseMessage> DeleteNonLaboraleDays(IntDateApi config)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.DeleteNonLaboraleDays(config.Id, config.Date);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Guarda la lista de configuraciones de citas
        /// </summary>
        /// <param name="saves"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost, Route("SaveSchedules")]
        public async Task<HttpResponseMessage> SaveSchedules(List<Schedule> saves)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.SaveSchedules(saves);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Guarda la lista de dias no laborales
        /// </summary>
        /// <param name="specialDays"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost, Route("SaveNonLaborableDays")]
        public async Task<HttpResponseMessage> SaveNonLaborableDays(List<SpecialDay> specialDays)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.SaveNonLaborableDays(specialDays);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Guarda la lista de dias especiales y su configuracion
        /// </summary>
        /// <param name="specialDaysapi"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost, Route("SaveSpecialDayAndSchedule")]
        public async Task<HttpResponseMessage> SaveSpecialDayAndSchedule(List<SpecialDayScheduleApi> specialDaysapi)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.SaveSpecialDayAndSchedule(specialDaysapi);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }



        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<Delegation>))]
        [HttpGet, Route("Delegations")]
        public async Task<HttpResponseMessage> GetDelegations()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();
                var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
                    .Select(r => r.Name)
                    .FirstOrDefault();
                var user = owinContext.GetAuthenticatedUser();
                var delegations = owinContext.GetAuthenticatedUser().GetDelegations();

                var result = await _commonDomainService.GetDelegationFiltered(delegations.ToArray());
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost, Route("SaveSpecialScheduleDays")]
        public async Task<HttpResponseMessage> SaveSpecialScheduleDays(List<SpecialDaysSchedule> specialSchedules)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.SaveSpecialSchedules(specialSchedules);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<Appoinment>))]
        [HttpGet, Route("Appointments/Delegation/{delegationId}")]
        public async Task<HttpResponseMessage> GetAppointmentsByDelegation(int delegationId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetAppointmentsForDelegation(delegationId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }



        [ResponseType(typeof(PagedRequestsResult))]
        [HttpGet, Route("Messages")]
        public async Task<HttpResponseMessage> GetMessages(int pageSize, int page, string query = null)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();

                var result = await _commonDomainService.GetMessagesAsync(pageSize, page, query);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(IEnumerable<Message>))]
        [HttpGet, Route("MessagesId/{messageId}")]
        public async Task<HttpResponseMessage> GetMessageById(int messageId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _commonDomainService.GetMessageById(messageId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }



        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost, Route("SaveDescription")]
        public async Task<HttpResponseMessage> SaveDescription(List<Message> message)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _commonDomainService.SaveDescription(message);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        /// Valida si el Número ISSSTE es válido
        /// </summary>
        /// <returns>Número ISSSTE del derechohabiente si es válido, si no nulo</returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(string))]
        [HttpGet, Route("Entitle/{isssteNumber}")]
        public async Task<HttpResponseMessage> ValidateIsssteNumber(string isssteNumber)
        {
            return await this.HandleOperationExecutionAsync(async () =>
            {
                bool isValid = false;

                isValid = await this._entitleDomainService.IsIsssteNumberValid(isssteNumber);

                return Request.CreateResponse(HttpStatusCode.OK, isValid ? isssteNumber : null);
            });
        }
        /// <summary>
        /// Obtiene el Número ISSSTE del derechochabiente buscandolo por CURP o popr RFC
        /// </summary>
        /// <returns>Número ISSSTE del derechohabiente</returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(string))]
        [HttpGet, Route("Entitle")]
        public async Task<HttpResponseMessage> GetEntitleIsssteNumber([FromUri]string curp, [FromUri] string rfc)
        {
            return await this.HandleOperationExecutionAsync(async () =>
            {
                HttpResponseMessage response = null;

                if (!String.IsNullOrEmpty(curp))
                {
                    var isssteNumber = await this._entitleDomainService.GetEntitleIsssteNumberByCurp(curp);

                    response = Request.CreateResponse(HttpStatusCode.OK, isssteNumber);
                }
                else if (!String.IsNullOrEmpty(rfc))
                {
                    var isssteNumber = await this._entitleDomainService.GetEntitleIsssteNumberByRfc(rfc);

                    response = Request.CreateResponse(HttpStatusCode.OK, isssteNumber);
                }
                else
                    response = Request.CreateResponse(HttpStatusCode.NotAcceptable, Domian.Resources.Resources.CurpAndRfcMissing);

                return response;
            });
        }
        /*
        /// <summary>
        /// Obtiene el Número ISSSTE del derechochabiente buscandolo por CURP o popr RFC
        /// </summary>
        /// <returns>Número ISSSTE del derechohabiente</returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(string))]
        [HttpGet, Route("RFC")]
        public async Task<HttpResponseMessage> GetEntitleRFC(string rfc)
        {
            return await this.HandleOperationExecutionAsync(async () =>
            {
                HttpResponseMessage response = null;


                if (!String.IsNullOrEmpty(rfc))
                {
                    var isssteNumber = await this._entitleDomainService.GetEntitleIsssteNumberByRfc(rfc);
                }
            });
        }

        */
        #endregion

        #region Catalogs Actions

        /// <summary>
        /// Obtiene una la lista de elementos del catalogo especificado
        /// </summary>
        /// <param name="catalogName">Nombre del catálogo</param>
        /// <returns>Lista de elementos del catálogo</returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [HttpGet, Route("Catalogs/{catalogName}")]
        public async Task<HttpResponseMessage> GetAllCatalogs(string catalogName)
        {
            return await HandleOperationExecutionAsync(
            async () =>
            {
                var result = await _catalogReflexionHelper.InvokeGetAllAsync<object>(_catalogRepository, catalogName);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        /// Obtiene un elemento específico de un catálogo
        /// </summary>
        /// <param name="catalogName">Nombre del catálogo</param>
        /// <param name="key">Llave del elemento</param>
        /// <returns>Detalle del elemento especificado</returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [HttpGet, Route("Catalogs/{catalogName}/{key}")]
        public async Task<HttpResponseMessage> GetCatalog(string catalogName, string key)
        {
            return await HandleOperationExecutionAsync(
            async () =>
            {
                var result = await _catalogReflexionHelper.InvokeGetAsync<object>(_catalogRepository, catalogName, key);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        /// Crea o actualiza un elemento de un catálogo
        /// </summary>
        /// <param name="catalogName">Nombre del catálogo</param>
        /// <param name="objectToSave">Información del elemento</param>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(void))]
        [HttpPost, Route("Catalogs/{catalogName}")]
        public async Task<HttpResponseMessage> SaveOrUpdateCatalogEntity(string catalogName, [FromBody]string objectToSave)
        {
            return await HandleOperationExecutionAsync(
            async () =>
            {
                dynamic parsedObject = JsonConvert.DeserializeObject(objectToSave, _catalogReflexionHelper.GetType(catalogName), new JsonSerializerSettings { DateFormatString = MexicoDateFormat });

                var result = await _catalogRepository.AddOrUpdateAsync(parsedObject, catalogName);

                return Request.CreateResponse(HttpStatusCode.OK, (object)result);
            });
        }

        /// <summary>
        /// Elimina un elemento de un catálogo
        /// </summary>
        /// <param name="catalogName">Nombre del catálogo</param>
        /// <param name="objectToSave"></param>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(void))]
        [HttpDelete, Route("Catalogs/{catalogName}")]
        public async Task<HttpResponseMessage> DeleteCatalogEntity(string catalogName, [FromBody]string objectToSave)
        {
            return await HandleOperationExecutionAsync(
            async () =>
            {
                HttpResponseMessage result = null;

                dynamic parsedObject = JsonConvert.DeserializeObject(objectToSave, _catalogReflexionHelper.GetType(catalogName), new JsonSerializerSettings { DateFormatString = MexicoDateFormat });

                try
                {
                    await _catalogRepository.DeleteAsync(parsedObject, catalogName);

                    result = Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (DbUpdateException ex)
                {
                    base.LogException(ex);

                    result = base.CreateStringResponseMessage(HttpStatusCode.InternalServerError, ResponseMessages.EntityDependedUponDeleteError);
                }

                return result;
            });
        }

        /// <summary>
        /// Obtiene la lista de elementos de los catalogos dependientes enviados
        /// </summary>
        /// <param name="catalogName">Nombre del catálogo</param>
        /// <param name="dependentPropertyNames">Propiedades dependientes</param>
        /// <returns>Elementos de cada propiedad dependiente enviada</returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [HttpPut, Route("Catalogs/{catalogName}/Dependents")]
        public async Task<HttpResponseMessage> GetCatalogEntityDependentCatalogs(string catalogName, [FromBody]List<string> dependentPropertyNames)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _catalogReflexionHelper.GetTypeDependentPropertiesAsync(_catalogRepository, catalogName, dependentPropertyNames);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        #endregion



        #region Comentado
        /// <summary>
        ///     Obtiene la lista de solicitudes paginando y filtrando por sexo / genero 
        /// </summary>
        /// /// <param name="pageSize">Cantidad de registros por página</param>
        /// <param name="page">Pagina de la solicitud</param>
        /// <param name="Gender"> sexo del derechohabiente</param>
        /// <param name="delID">id con el que se buscara la delegacion </param>
        /// <param name="pensionId"> tipo de pension</param>
        /// <param name="statusId">estatus que se buscara </param>
        /// <param name="nameEntiti">nombre del derechohabiente</param>
        /// <param name="numIssste">numero de issste del derechohabiente</param>
        /// <param name="inicio">fecha de inicio</param>
        /// <param name="final">fecha final </param>
        /// <returns>Solicitudes</returns>
        //[AuthorizeByConfig("AllAdminRoles")]
        //[ResponseType(typeof(List<PagedReportResult>))]
        //[HttpGet, Route("Reportes")]
        //public async Task<HttpResponseMessage> GetReportes(int pageSize, int page, string Gender = null,
        //    int? delID = default(int?), int? pensionId = default(int?), int? statusId = default(int?),
        //    string nameEntiti = null,
        //    string numIssste = null,
        //    DateTime? inicio = default(DateTime?),
        //    DateTime? final = default(DateTime?), bool? banderaSoloDelegacion = default(bool?))
        //{
        //    return await HandleOperationExecutionAsync(async () =>
        //    {
        //        /*var owinContext = Request.GetOwinContext();
        //        var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
        //            .Select(r => r.Name)
        //            .FirstOrDefault();
        //        var user = owinContext.GetAuthenticatedUser();
        //        var delegation = user.Properties.Exists(r => r.Name == IsssteUserPropertyTypes.Delegation)
        //            ? int.Parse(user.Properties.First(r => r.Name == IsssteUserPropertyTypes.Delegation).Value)
        //            : 0;
        //        var delegations = user.GetDelegations();*/
        //        var result =
        //            await
        //                _requestDomainService.GetPagedRequestsRepo(pageSize, page,
        //             Gender, delID, pensionId, statusId, nameEntiti, numIssste, inicio, final, banderaSoloDelegacion);
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    });
        //}


        /// <summary>
        ///     Obitiene el diagnostico por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[AuthorizeByConfig("AllAdminRoles")]
        //[ResponseType(typeof(List<OpinionMessage>))]
        //[HttpGet, Route("OpinionsMessagesConfig/{id}")]
        //public async Task<HttpResponseMessage> GetOpinionMessagesByConfig(int id)
        //{
        //    return await HandleOperationExecutionAsync(async () =>
        //    {
        //        var result = await _requestDomainService.GetOpinionMessagesByConfig((EnumConfigMessages)id);

        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    });
        //}


        //[ResponseType(typeof(List<OpinionMessage>))]
        //[HttpGet, Route("UsersOperator")]
        //public async Task<HttpResponseMessage> GetUsersOperator()
        //{
        //    return await this.HandleOperationExecutionAsync(async () =>
        //    {


        //        var result = await this._requestDomainService.GetUsersOperator();

        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    });
        //}

        #endregion
        //    [AuthorizeByConfig("RequestsRoles")]
        //    [ResponseType(typeof(RequestDetail))]
        //    [HttpGet, Route("Request/{requestId}/Status/{statusId}")]
        //    public async Task<HttpResponseMessage> GetRequestDetail(Guid requestId, int statusId)
        //    {
        //        return await HandleOperationExecutionAsync(async () =>
        //        {
        //            var owinContext = Request.GetOwinContext();
        //            var role = owinContext.GetAuthenticatedUserAuthorizedRoles()
        //                .Select(r => r.Name)
        //                .FirstOrDefault();

        //            var result = await _administratorDomainService.GetRequestDetailAsync(requestId, role, statusId);

        //            return Request.CreateResponse(HttpStatusCode.OK, result);
        //        });
        //    }



    }



}