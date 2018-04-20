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
using ISSSTE.Tramites2016.Hipotecas.Bussines;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Domian;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using ISSSTE.Tramites2016.Hipotecas.Domians;
using ISSSTE.Tramites2016.Common.Reports.Model.Hipoteca;
using System.IO;
using System.Net.Http.Headers;
using ISSSTE.Tramites2016.Hipotecas.Resources;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Controllers
{
    /// <summary>
    ///     Controllador para el api del derechohabiente
    /// </summary>
    [RoutePrefix("api/Entitle")]
    public class EntitleApiController : BaseApiController
    {
        #region Constants

        /// <summary>
        ///     Nombre del encabezado utilizado para enviar la identidad del usuario
        /// </summary>
        private const string UserIdentityHeader = "Issste-Tramites2016-UserIdentity";

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor del api
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="requestDomainService"></param>
        /// <param name="entitleDomainService"></param>
        public EntitleApiController(ILogger logger, IRequestDomainService requestDomainService,
            IEntitleDomainService entitleDomainService) : base(logger)
        {
            _requestDomainService = requestDomainService;
            _entitleDomainService = entitleDomainService;
        }

        #endregion

        #region Fields

        /// <summary>
        ///     dominio de la solicitud
        /// </summary>
        private readonly IRequestDomainService _requestDomainService;

        /// <summary>
        ///     dominio del derechohabiente
        /// </summary>
        private readonly IEntitleDomainService _entitleDomainService;

        /// <summary>
        ///     Identidad del usuario enviado en la petición
        /// </summary>
        private string _userIdentity;


        EntitleBussiness entB = new EntitleBussiness();
        List<PropertType> listPT = new List<PropertType>();
        List<TypeOwner> listTO = new List<TypeOwner>();
        CatalogsBussiness ctalB = new CatalogsBussiness();
        Request_Property_Entitles rqpe = new Request_Property_Entitles();
        Entitle entAp = new Entitle();


        DataAccess.HipotecasContext _penContext = new HipotecasContext();

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

        public class Catalogos
        {
            //  private readonly string _typeowner;
            private readonly List<PropertType> _pt;
            private readonly List<TypeOwner> _to;
            private readonly List<UrbanCenter> _uc;
            private readonly Request_Property_Entitles _rpe;
            private readonly Entitle _entap;

            private readonly List<DelegationCalendar> _dc;

            public Catalogos(IEnumerable<DelegationCalendar> dc,IEnumerable<PropertType> pt, IEnumerable<TypeOwner> to, IEnumerable<UrbanCenter> uc, Request_Property_Entitles rpe, Entitle entap)
            {
                _pt = pt.ToList();
                _to = to.ToList();
                _uc = uc.ToList();
                _rpe = rpe;
                _entap = entap;

                _dc = dc.ToList();
                //                _typeowner = typeowner;

            }
            public IEnumerable<PropertType> PropertTypeCat { get { return _pt; } }
            public IEnumerable<DelegationCalendar> DelegatioC { get { return _dc; } }
            public IEnumerable<TypeOwner> TypeOwnerCat { get { return _to; } }
            public IEnumerable<UrbanCenter> UrbanCenterCat { get { return _uc; } }
            public Request_Property_Entitles reqPrpEnt { get { return _rpe; } }
            public Entitle entAp { get { return _entap; } }

            // public string typeowner { get { return _typeowner; } }

        }

        #region Actions

        /// <summary>
        ///     Obtiene la informacion del derechohabiente
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Entitle))]
        [HttpGet, Route("Information")]
        public async Task<HttpResponseMessage> GetEntitledInformation()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _entitleDomainService.GetEntitleByNoIssste(UserIdentity);

                if (result == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent, result);

                else
                    return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la lista de deudos del derechohabiente
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<Debtor>))]
        [HttpGet, Route("Debtors")]
        public async Task<HttpResponseMessage> GetDebtors()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _entitleDomainService.GetDebtorsbyNoIssste(UserIdentity);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Valida el curp
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        [ResponseType(typeof(bool))]
        [HttpGet, Route("ValidateCurp/{curp}")]
        public async Task<HttpResponseMessage> ValidateCurpRenapo(string curp)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _entitleDomainService.ValidateCurp(curp);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        /// Obtiene las alertas de las solicitudes del derechohabiente
        /// </summary>
        /// <param name="isssteNumber">Número Issste del derechohabiente</param>
        /// <returns>Lista de alertas del sistema</returns>
        [ResponseType(typeof(IEnumerable<Alert>))]
        [HttpGet, Route("{isssteNumber}/Alerts")]
        public async Task<HttpResponseMessage> GetEntitledAlerts(string isssteNumber)
        {
            return await base.HandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetAlertsAsync(isssteNumber);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        /// Obtiene una lista que contiene las listas de docuemntos para mostrarlos en la sección correspondiente
        /// </summary>
        /// <returns>Lista de listas de documentos</returns>
        [ResponseType(typeof(List<List<DocumentTypes>>))]
        [HttpGet, Route("Documents")]
        public async Task<HttpResponseMessage> GetDocumentsForInfo()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.GetDocumentsForInfoAsync();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        //[ResponseType(typeof(Catalogos))]
        //[HttpGet, Route("CatalogsProperty/{curp}")]
        //public HttpResponseMessage GetTypeOwner(string curp)
        //{

        //    entAp = entB.GetEntitle(curp);


        //    listTO = ctalB.GetTypeOwner();
        //    int indexTOF = listTO.IndexOf(listTO.Where(p => p.Description.Contains(ConfigurationManager.AppSettings["EstatusFinado"].ToString())).FirstOrDefault());
        //    int indexTO = listTO.IndexOf(listTO.Where(p => !p.Description.Contains(ConfigurationManager.AppSettings["EstatusFinado"].ToString())).FirstOrDefault());

        //    List<TypeOwner> tolisF = new List<TypeOwner>();
        //    tolisF.Add(listTO[indexTOF]);

        //    List<TypeOwner> tolis = new List<TypeOwner>();
        //    tolis.Add(listTO[indexTO]);

        //    var Index = new Catalogos(ctalB.GetPropertType(), entAp.State == ConfigurationManager.AppSettings["EstatusFinado"].ToString() ? tolisF : tolis, ctalB.GetUrbanCenters(), rqpe, entAp);


        //    return Request.CreateResponse(HttpStatusCode.OK, Index);

        //    //return await HandleOperationExecutionAsync(async () =>
        //    //{
        //    //    var result = await _requestDomainService.GetDocumentsForInfoAsync();

        //    //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //    //});
        //}
        [ResponseType(typeof(Catalogos))]
        [HttpGet, Route("UrbanCenter/{id}")]
        public HttpResponseMessage GetUrbanCenter(string id)
        {
            var result = ctalB.GetUrbanCenterByID(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
        /// <summary>
        ///     Obtiene toda la informacion del derechohabiente par ala solicitud
        /// </summary>
        /// <param name="idPension"></param>
        /// <returns></returns>
        /// 

        [ResponseType(typeof(EntitledData))]
        [HttpGet, Route("AllInformationNew/{idPension}")]
        //CAP public async Task<HttpResponseMessage> GetAllDataFromEntitleNewRequest(int idPension)
        public async Task<HttpResponseMessage> GetAllDataFromEntitleNewRequest()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = new EntitledData();

                result.Entitle = await _entitleDomainService.GetEntitleByNoIssste(UserIdentity);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        ///     Obtiene toda la informacion de una solicitud y el derechohabiente
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [ResponseType(typeof(EntitledData))]
        [HttpGet, Route("AllInformation/{requestId}")]
        public async Task<HttpResponseMessage> GetAllDataFromEntitleNewRequest(Guid requestId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = new EntitledData();
                result.Entitle = await _entitleDomainService.GetEntitleById(UserIdentity);
                result.Debtors = await _entitleDomainService.GetDebtorsbyRequest(requestId);
                //else
                //result.Validation = await _entitleDomainService.GetValidationByRequest(requestId);
                //result.timeContributions = await _entitleDomainService.GetTimeContributionByRequest(requestId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Guarda el Derechohabiente
        /// </summary>
        /// <param name="entitle"></param>
        /// <returns></returns>
        [ResponseType(typeof(Entitle))]
        [HttpPost, Route("Save")]
        public async Task<HttpResponseMessage> SaveEntitle(Entitle entitle)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _entitleDomainService.SaveEntitle(entitle);
                return Request.CreateResponse(HttpStatusCode.OK, entitle);
            });
        }

        /// <summary>
        ///     Guarda toda la informacion de un derechohabiente apartir de una solicitud
        /// </summary>
        /// <param name="entitledData"></param>
        /// <returns></returns>
        [ResponseType(typeof(EntitledData))]
        [HttpPost, Route("SaveAll")]
        public async Task<HttpResponseMessage> SaveEntitleAll(EntitledData entitledData)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _entitleDomainService.SaveEntitle(entitledData.Entitle);

                if (entitledData.Debtors != null)
                {
                    await _entitleDomainService.SaveDebtors(entitledData.Debtors, entitledData);
                }



                //DESCOMENTAR PARA LIBERACION   await _requestDomainService.SendEmailValidation(entitledData.RequestId);
                return Request.CreateResponse(HttpStatusCode.OK, entitledData);
            });
        }

        /// <summary>
        ///     Guardar el diagnostico de la solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Opinion))]
        [HttpPost, Route("SaveAll")]
        public async Task<HttpResponseMessage> GetOpinionByRequest(Guid requestId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var res = await _requestDomainService.GetOpinionByRequest(requestId);


                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }

        [ResponseType(typeof(Opinion))]
        [HttpGet, Route("GetMorgageCancel")]
        public async Task<HttpResponseMessage> DownloadDocumentMortgageCancel()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                Guid requestId = new Guid();
                HttpResponseMessage response = null;

                var result  = await _entitleDomainService.GetMortgageCancel(requestId);

                if (response != null)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    var stream = new MemoryStream(result.Data);
                    response.Content = new StreamContent(stream);

                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(HttpContants.ContentTypes.OctetStream);
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(HttpContants.ContentDisposition.Attachment)
                    {
                        FileName = result.FileName
                    };
                }
                else
                    response = base.CreateStringResponseMessage(HttpStatusCode.NotFound, ResponseMessages.FileNotFound);

                return response;
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



        #region Public Methods
        public string GetMessagesByCode(string codigo)
        {
            try
            {
                string Mensaje = _penContext.Messages.ToList().Where(x => x.Key == codigo).Select(x => new Message
                {
                    MessageId = x.MessageId,
                    Key = x.Key,
                    Description = x.Description,

                }).FirstOrDefault().Description;

                return Mensaje;// +"\n";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        #endregion

    }
}