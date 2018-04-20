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
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Domians;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Controllers
{
    /// <summary>
    ///     Controllador para el api de operaciones comunes
    /// </summary>
    [RoutePrefix("api/Common")]
    public class CommonApiController : BaseApiController
    {
        #region Constants

        /// <summary>
        ///     Nombre del encabezado utilizado para enviar la identidad del usuario
        /// </summary>
        private const string UserIdentityHeader = "Issste-Tramites2016-UserIdentity";

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor del controlador
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="requestDomainService"></param>
        /// <param name="entitleDomainService"></param>
        /// <param name="calendarDomainService"></param>
        /// <param name="commonDomainService"></param>
        public CommonApiController(ILogger logger, IRequestDomainService requestDomainService,
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
        ///     *Dominio de la solicitud
        /// </summary>
        private IRequestDomainService _requestDomainService;

        /// <summary>
        ///     Dominio del derechohabiente
        /// </summary>
        private IEntitleDomainService _entitleDomainService;

        /// <summary>
        ///     Dominio del calendario
        /// </summary>
        private ICalendarDomainService _calendarDomainService;

        /// <summary>
        ///     Dominio comun
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
        ///     Obtiene las delegaciones
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof (List<Delegation>))]
        [HttpGet, Route("Delegations")]
        public async Task<HttpResponseMessage> GetDelegations()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _commonDomainService.GetDelegations();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Regresa la delegacion por su id
        /// </summary>
        /// <param name="idDelegation"></param>
        /// <returns></returns>
        [ResponseType(typeof (Delegation))]
        [HttpGet, Route("Delegation/{idDelegation}")]
        public async Task<HttpResponseMessage> GetDelegationById(int idDelegation)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _commonDomainService.GetDelegationById(idDelegation);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la configuracion por su llave
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [ResponseType(typeof (Configuration))]
        [HttpGet, Route("Configuration/{name}")]
        public async Task<HttpResponseMessage> GetConfigurationByName(string name)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _commonDomainService.GetConfiguration(name);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene el mensaje por su llave
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ResponseType(typeof (Message))]
        [HttpGet, Route("Message/{key}")]
        public async Task<HttpResponseMessage> GetMessage(string key)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _commonDomainService.GetMessage(key);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene los la lista de mensajes
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof (List<Message>))]
        [HttpGet, Route("Messages")]
        public async Task<HttpResponseMessage> GetMessages()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _commonDomainService.GetMessages();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene los requerimientos de pension
        /// </summary>
        /// <param name="pensionId"></param>
        /// <returns></returns>
        [ResponseType(typeof (List<Requirement>))]
        [HttpGet, Route("Requirements/{pensionId}")]
        public async Task<HttpResponseMessage> GetRequirements(int pensionId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _commonDomainService.GetRequirements(pensionId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene el role por id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [ResponseType(typeof (Role))]
        [HttpGet, Route("Role/{roleId}")]
        public async Task<HttpResponseMessage> GetRole(int roleId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _commonDomainService.GetRole(roleId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     obtiene los roles
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof (List<Role>))]
        [HttpGet, Route("Roles")]
        public async Task<HttpResponseMessage> GetRoles()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _commonDomainService.GetRoles();

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