#region

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Common.Web;
using ISSSTE.Tramites2016.Hipotecas.Domain;
using ISSSTE.Tramites2016.Hipotecas.Domain.Implementation;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using System.IO;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Domians;
using System.Configuration;
using ISSSTE.Tramites2016.Hipotecas.Model.Enums;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;
using Newtonsoft.Json;
using ISSSTE.Tramites2016.Hipotecas.Filters;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Bussines;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Controllers
{
    /// <summary>
    ///     Controllador para el derechohabiente
    /// </summary>
    [RoutePrefix("Entitle")]
    public class EntitleController : BaseController
    {
        private EntitleDomain entitleDomain = new EntitleDomain();
        private EntitleBussiness entBuss = new EntitleBussiness();
        private DocumentBussinessLogic documentBussinessLogic = new DocumentBussinessLogic();
        private RequestBussinessLogic requestBussinessLogic = new RequestBussinessLogic();
        private DelegationBussinessLogic delegationBussinessLogic = new DelegationBussinessLogic();
        private List<CivilState> listCV = new List<CivilState>();
        int contadorDoctos = 0;
        int contadorRequeridos = 0;
        HttpPostedFileBase file;
        #region Constructor


        private readonly IEntitleDomainService _entitleDomainService;
        /// <summary>
        ///     Constructor del Controller
        /// </summary>
        /// <param name="logger"></param>
        public EntitleController(ILogger logger, IEntitleDomainService entitleDomainService) : base(logger)
        {
            _entitleDomainService = entitleDomainService;
        }

        #endregion


        public class Catalogos
        {

            private readonly List<CivilState> _cv;

            public Catalogos(IEnumerable<CivilState> cv)
            {
                // _cv = cv;


            }

            public IEnumerable<CivilState> CivilStateCat { get { return _cv; } }


        }
        #region Actions

        public ActionResult Index(string NoIssste)
        {
            HttpContext context = System.Web.HttpContext.Current;
            context.Session["NoIssste"] = NoIssste;
            TempData["RequestId"] = "";
            if (String.IsNullOrEmpty(NoIssste))
            {
                return RedirectToAction("Empty");
            }
            var result = entitleDomain.GetEntitle(NoIssste);

            if (result != null)
            {
                if (result.Gender == "H")
                    result.Gender = "HOMBRE";
                else
                    result.Gender = "MUJER";

                if (result.Telephone == "" || result.Email == "")
                {
                    ViewBag.Bandera = "Vacio";
                }
                ViewBag.Message = TempData["Message"];
                @ViewBag.NoIssste = NoIssste;
            }
            else
            {
                ViewBag.Bandera = "Vacio";
                ViewBag.Message = "Error";
                ViewBag.Message1 = ConfigurationManager.AppSettings["EntitledNotExist"];
            }



            return View(result);
        }

        public ActionResult Empty()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SaveIndex(string hidden, string lada, string telephone, string email, string MobilePhone)
        {
            entitleDomain.saveInformationEntitle(hidden, lada, telephone, email, MobilePhone);
            TempData["Message"] = "Success";
            return RedirectToAction("Index", new
            {
                NoIssste = hidden
            });

        }

        public ActionResult RedirectPage(string Page, string NoIssste, string RequestId)
        {
            return RedirectToAction(Page, new
            {
                Page = Page,
                NoIssste = NoIssste,
                RequestId = RequestId
            });
        }

        public async Task<ActionResult> DownloadDocumentMortgageCancel(string RequestId)
        {

            var result = await _entitleDomainService.GetMortgageCancel(Guid.Parse(RequestId));

            var stream = new MemoryStream(result.Data);

            FileStreamResult archivo = null;
            archivo = File(stream, "application/pdf");

            return archivo;

        }

        [IsssteNumberFilter]
        public ActionResult Documentacion(string NoIssste, string RequestId, String Sts)
        {
            Session["StsSend"] = Sts;
            //recuperar el tipo de credito
            //TempData["TypeCredit"]
            try
            {
                TempData["saveRequest"] = true;

                if (TempData["WritingProperty"] != null)
                {
                    string WritingProperty = TempData["WritingProperty"].ToString();
                    TempData["WritingProperty"] = WritingProperty;
                }
                string tipoCred = string.Empty;
                var GetNombre = entitleDomain.GetEntitle(NoIssste);

                if (TempData["TypeCredit"] == null)
                {
                    if (!GetNombre.IsActive)
                    {
                        TempData["TypeCredit"] = (int)EnumBeneficiario.Apoderado;
                    }
                    else { TempData["TypeCredit"] = (int)EnumBeneficiario.Derechohabiente; }

                }
                else
                {
                    tipoCred = TempData["TypeCredit"].ToString();
                    TempData["TypeCredit"] = tipoCred;
                }

                if (string.IsNullOrEmpty(RequestId))
                    return RedirectToAction("Registro", new
                    {
                        NoIssste = NoIssste
                    });

                int i = 0;
                List<int> documentsRequired = new List<int>();
                var result = documentBussinessLogic.GeDocuments(tipoCred);
                var entitle = entitleDomain.GetEntitle(NoIssste);
                var request = requestBussinessLogic.GetRequestByEntitleId(entitle.EntitleId);
                List<DocumentType> documentType = new List<DocumentType>();

                //var GetNombre = entitleDomain.GetEntitle(NoIssste);

                @ViewBag.NoIssste = NoIssste;
                @ViewBag.RequestId = RequestId;
                @ViewBag.Name = entitle.Name;

                foreach (var document in result)
                {
                    if (document.BeneficiarieType == 1 || document.BeneficiarieType == 5)
                    {
                        documentType.Add(document);
                        if (document.Required == true)
                        {
                            documentsRequired.Add(i);
                        }
                    }
                    else if (document.BeneficiarieType == 4)
                    {
                        documentType.Add(document);
                        if (document.Required == true)
                        {
                            documentsRequired.Add(i);

                        }
                    }
                    else if (document.BeneficiarieType == 3 || document.BeneficiarieType == 2)
                    {
                        documentType.Add(document);
                        if (document.Required == true)
                        {
                            documentsRequired.Add(i);

                        }
                    }
                    i++;
                }

                foreach (var document in documentType)
                {
                    if (document.BeneficiarieType == 1 || document.BeneficiarieType == 5)
                    {
                        //  documentType.Add(document);

                        if (document.Required == true)
                        {
                            contadorRequeridos = contadorRequeridos + 1;
                        }
                    }
                }
                ViewBag.documentsRequired = JsonConvert.SerializeObject(documentsRequired);

                if (entitle.IsActive)
                {
                    if (Session["ErrorDoctos"] != null)
                    {
                        if (Session["ErrorDoctos"].ToString() == "Error")
                        {
                            ViewBag.Bandera = "Vacio";
                            ViewBag.Message = "Error";
                            ViewBag.Message1 = Session["Message"];
                            // Session["Message"] = "Algún documento ingresado es mayor al tamaño permitido.";
                        }

                    }
                    else
                    {
                        Session["Message"] = "";
                        ViewBag.Message = "Error";
                        ViewBag.Message1 = Session["Message"];
                    }

                    return View(documentType);
                }
                if (Session["ErrorDoctos"] != null)
                {
                    if (Session["ErrorDoctos"].ToString() == "Error")
                    {
                        ViewBag.Bandera = "Vacio";
                        ViewBag.Message = "Error";
                        ViewBag.Message1 = Session["Message"];
                        //  ViewBag.Message1 = "Algún documento ingresado es mayor al tamaño permitido.";

                    }

                }
                else
                {
                    Session["Message"] = "";
                    ViewBag.Message = "Error";
                    ViewBag.Message1 = Session["Message"];
                }

                return View(result);
            }



            catch (Exception ex)
            {
                return RedirectToAction("Empty");
            }
        }

        [HttpPost]
        public ActionResult SaveDocumentacion(string NoIssste, string RequestId, IEnumerable<HttpPostedFileBase> files)
        {
            Session["ErrorDoctos"] = null;
            List<byte[]> len = new List<byte[]> { };
            try
            {
                if (string.IsNullOrEmpty(RequestId))
                    return RedirectToAction("Registro", new
                    {
                        NoIssste = NoIssste
                    });

                int contArchivosConten = 0;
                for (int x = 0; x < Request.Files.Count; x++)
                {
                    file = Request.Files[x];
                    if (file.ContentLength > 0)
                    {
                        contArchivosConten = contArchivosConten + 1;
                    }

                }

                if (contArchivosConten == 0)
                {
                    return RedirectToAction("Documentacion", new
                    {
                        NoIssste = NoIssste,
                        RequestId = RequestId,
                        Sts = "3"
                    });
                }
                else if (contArchivosConten < contadorRequeridos)
                {
                    return RedirectToAction("Documentacion", new
                    {
                        NoIssste = NoIssste,
                        RequestId = RequestId,
                        Sts = "3"
                    });
                }
                else
                {
                    for (int x = 0; x < Request.Files.Count; x++)
                    {
                        file = Request.Files[x];
                        if (Path.GetFileName(file.FileName).Equals(""))
                        {
                            contadorDoctos = contadorDoctos + 1;
                            continue;
                        }
                        else
                        {
                            string extension = Path.GetExtension(file.FileName);
                            if (extension.ToUpper() == ".JPG" || extension.ToUpper() == ".PNG" || extension.ToUpper() == ".PDF")
                            {
                                Session["ErrorDoctos"] = null;
                                byte[] archivo = ConvertToBytes(file);
                                if (Request.Files[x] != null)
                                {
                                    if (archivo.Length <= 5242880)//5 megas
                                    {
                                        Session["ErrorDoctos"] = null;
                                        contadorDoctos = contadorDoctos + 1;
                                        len.Add(archivo);
                                    }

                                    else
                                    {

                                        //ViewBag.Bandera = "Vacio";
                                        ViewBag.Message = "Error";
                                        Session["Message"] = "Algún documento ingresado es mayor al tamaño permitido.";
                                        Session["ErrorDoctos"] = ViewBag.Message;
                                        return RedirectToAction("Documentacion", new
                                        {
                                            NoIssste = NoIssste,
                                            RequestId = RequestId,
                                            Sts = "1"
                                        });
                                    }
                                }



                            }
                            else
                            {

                                //ViewBag.Bandera = "Vacio";
                                ViewBag.Message = "Error";
                                Session["Message"] = "Algún documento ingresado no es del formato correcto.";
                                Session["ErrorDoctos"] = ViewBag.Message;
                                return RedirectToAction("Documentacion", new
                                {
                                    NoIssste = NoIssste,
                                    RequestId = RequestId,
                                    Sts = "2"
                                });
                            }
                        }

                    }


                    if (contadorDoctos == Request.Files.Count)
                    {

                        // bool guardado = GuardarArchivos(NoIssste, RequestId, len);
                        for (int x = 0; x < len.Count; x++)
                        {
                            file = Request.Files[x];
                            string extension = Path.GetExtension(file.FileName);
                            documentBussinessLogic.SaveDocument(len[x], extension, NoIssste, Convert.ToInt32(Request.Files.AllKeys[x].ToString()), RequestId);
                        }

                        string StatusMail = "0";
                        TempData["currentStep"] = 2;
                        if (Session["StsSend"] != null)
                        {
                            if (Convert.ToInt32(Session["StsSend"].ToString()) == (int)StatusEnum.EnesperadecargadedocumentacionDer)
                            {
                                requestBussinessLogic.SaveStatusNext((int)StatusEnum.EnesperadecargadedocumentacionDer, RequestId);
                                StatusMail = "1";
                            }
                        }

                        //aqui meter el envio de correo
                        //Envía Correo electrónico
                        entBuss.SendMail(RequestId, 201);

                        return RedirectToAction("RegistroFinalizado", new
                        {
                            NoIssste = NoIssste,
                            RequestId = RequestId
                        });
                    }
                    else
                    {
                        return RedirectToAction("Empty");
                    }
                }


            }
            catch (Exception ex)
            {
                return RedirectToAction("Documentacion", new
                {
                    NoIssste = NoIssste,
                    RequestId = RequestId,
                    Sts = Session["StsSend"].ToString()
                });
            }

        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            reader.Dispose();
            return imageBytes;
        }

        [IsssteNumberFilter]
        public ActionResult About(string NoIssste)
        {
            TempData["RequestId"] = "";
            TempData["WritingProperty"] = null;
            var GetNombre = entitleDomain.GetEntitle(NoIssste);
            var result = documentBussinessLogic.GeDocuments();
            @ViewBag.Name = GetNombre.Name;
            @ViewBag.NoIssste = NoIssste;

            return View(result);
        }

        [HttpPost]
        public ActionResult SaveRequestNoComplete(string NoIssste, string RequestId)
        {
            requestBussinessLogic.UpdateRequestNoComplete(RequestId);
            TempData["currentStep"] = 3;
            entBuss.SendMail(RequestId, 200);
//            Session["Status"] = (int)StatusEnum.EnesperadeAgendarCiraDer;
            return RedirectToAction("RegistroFinalizado", new
            {
                NoIssste = NoIssste,
                RequestId = RequestId
            });
        }

        [IsssteNumberFilter]
        public ActionResult Solicitud(string NoIssste, string RequestId)
        {
            var requestActive = requestBussinessLogic.GetActivesRequests(NoIssste);
            if (requestActive == 1)
            {
                @ViewBag.requestActive = true;
            }
            else
            {
                @ViewBag.requestActive = false;
            }

            RequestId = Convert.ToString(TempData["RequestId"]);
            TempData["WritingProperty"] = null;
            TempData["RequestId"] = RequestId;
            var GetNombre = entitleDomain.GetEntitle(NoIssste);
            var result = documentBussinessLogic.GeDocuments();
            @ViewBag.Name = GetNombre.Name;
            @ViewBag.NoIssste = NoIssste;
            @ViewBag.RequestId = RequestId;
            var hidden = NoIssste;

            return View(result);
        }

        [IsssteNumberFilter]
        public ActionResult Historial(string NoIssste)
        {
            TempData["RequestId"] = "";
            var GetNombre = entitleDomain.GetEntitle(NoIssste);
            //var requestResponse = requestBussinessLogic.GetAllRequest(NoIssste);
            var requestHistory = requestBussinessLogic.GetAllRequestHistoria(NoIssste);
            @ViewBag.Name = GetNombre.Name;
            @ViewBag.NoIssste = NoIssste;

            return View(requestHistory);
        }
        public ActionResult Registro(string NoIssste, string RequestId)
        {
            Session["Message"] = "";
            if (TempData["WritingProperty"] != null)
            {
                string Writing = TempData["WritingProperty"].ToString();
                ViewBag.Writing = Writing;
            }
            else
            {
                ViewBag.Writing = "";
            }

            if (TempData["RequestId"] != null)
            {
                RequestId = Convert.ToString(TempData["RequestId"]);
            }
            listCV = requestBussinessLogic.GetCivilState();

            @ViewBag.CivilState = listCV;


            if (String.IsNullOrEmpty(NoIssste))
            {
                return RedirectToAction("Empty");
            }
            var GetNombre = entitleDomain.GetEntitle(NoIssste);
            var result = entitleDomain.GetStatusEntitle(NoIssste);
            if (!GetNombre.IsActive)
            {
                TempData["TypeCredit"] = (int)EnumBeneficiario.Apoderado;
            }
            else { TempData["TypeCredit"] = (int)EnumBeneficiario.Derechohabiente; }


            @ViewBag.Name = GetNombre.Name;
            @ViewBag.RequestId = RequestId;
            @ViewBag.Active = result;
            @ViewBag.NoIssste = NoIssste;

            return View();
        }

        [HttpPost]
        public ActionResult SaveRequest(string hidden, [Bind(Prefix = "Item2")] Request requestData, [Bind(Prefix = "Item1")] Entitle entitle, string RequestId)
        {

            //Meter variables de session o temp para guardar los datos del campo de direccion y el de la opcion de conyugue o individual
            //Validar de algun modo que si ya existe el numero de RequestId no tiene que volver arrojar otro
            try
            {
                Request request = new Request();
                if (RequestId == null || RequestId == "")
                {
                    request.RequestId = Guid.NewGuid();
                }
                else
                {
                    request.RequestId = new Guid(RequestId);
                }


                request.IsConjugalCredit = Convert.ToBoolean(requestData.IsConjugalCredit);
                request.WritingProperty = Request.Params["WritingProperty"].ToString();
                var result = entitleDomain.GetEntitle(hidden);
                if (RequestId == null || RequestId == "")
                {
                    requestBussinessLogic.SaveRequest(request, result.EntitleId);

                }

                else
                {
                    requestBussinessLogic.UpdateRequest(RequestId, request.WritingProperty, request.IsConjugalCredit);
                }


                if (request.IsConjugalCredit)
                {
                    TempData["TypeCredit"] = (int)EnumBeneficiario.Conyugal;
                }
                TempData["WritingProperty"] = request.WritingProperty.ToString();
                TempData["RequestId"] = request.RequestId;
                ViewBag.Message = "1";

                Session["Status"] = (int)StatusEnum.EnesperaderevisiondedocumentacionDer;
                return RedirectToAction("Documentacion", new
                {
                    NoIssste = hidden,
                    RequestId = request.RequestId
                });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error";
                return View();
            }
        }


        public ActionResult RegistroFinalizado(string NoIssste, string RequestId)
        {
            TempData["RequestId"] = "";
            var GetNombre = entitleDomain.GetEntitle(NoIssste);
            @ViewBag.NoIssste = NoIssste;
            @ViewBag.Name = GetNombre.Name;
            @ViewBag.RequestId = RequestId;
            @ViewBag.currentStep = TempData["currentStep"].ToString();

            ////Envía Correo electrónico
            //entBuss.SendMail(RequestId, Session["Status"] != null? Convert.ToInt32(Session["Status"].ToString()): Convert.ToInt32(Session["StsSend"].ToString()));

            return View();
        }
        public ActionResult AgendarCita(string NoIssste, string RequestId)
        {
            if (string.IsNullOrEmpty(RequestId))
                return RedirectToAction("Historial", new
                {
                    NoIssste = NoIssste
                });

            var GetNombre = entitleDomain.GetEntitle(NoIssste);

            @ViewBag.NoIssste = NoIssste;
            @ViewBag.Name = GetNombre.Name;
            @ViewBag.RequestId = RequestId;

            return View();
        }


        public ActionResult Calendario(string NoIssste, string RequestId)
        {
            if (string.IsNullOrEmpty(RequestId))
                return RedirectToAction("Historial", new
                {
                    NoIssste = NoIssste
                });

            var GetNombre = entitleDomain.GetEntitle(NoIssste);
            var delegations = delegationBussinessLogic.GetDelegations();

            @ViewBag.NoIssste = NoIssste;
            @ViewBag.Name = GetNombre.Name;
            @ViewBag.RequestId = RequestId;

            return View(delegations);
        }
        public ActionResult SolicitudCompleta(string NoIssste)
        {
            var tempSplit = NoIssste.Split('|');
            var entitle = entitleDomain.GetEntitle(tempSplit[0]);

            if (entitle.Gender == "H")
                entitle.Gender = "HOMBRE";
            else
                entitle.Gender = "MUJER";
            var requestId = tempSplit[2];
            var request = requestBussinessLogic.GetRequestByRequestId(new Guid(requestId));
            var NameDH = entitle.Name;
            var FolioT = tempSplit[1];

            @ViewBag.NameDH = NameDH;
            @ViewBag.NoIssste = tempSplit[0];
            @ViewBag.FolioT = FolioT;
            ViewBag.RequestId = tempSplit[2];
            return View(new Tuple<Entitle, Request>(entitle, request));
        }

        #endregion



    }
}