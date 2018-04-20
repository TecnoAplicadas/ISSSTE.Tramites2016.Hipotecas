
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISSSTE.Tramites2016.Common.Web.Mvc;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Domain.Implementation;
using System.IO;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using ISSSTE.Tramites2016.Hipotecas.Domian;
using ISSSTE.Tramites2016.Hipotecas.Domian.Implementation;
using ISSSTE.Tramites2016.Common.Catalogs;
using ISSSTE.Tramites2016.Hipotecas.Models;
using ISSSTE.Tramites2016.Hipotecas.Bussiness;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using Newtonsoft.Json;

namespace ISSSTE.Tramites2016.Hipotecas.Controllers
{


    /// <summary>
    ///     Controllador para la pagina de administrador
    /// </summary>
    [AuthorizeByConfig("AllAdminRoles")]
    //[AuthorizeByConfig("admin.hipotecas")]
    [RoutePrefix("Administrator")]
    public class AdministratorController : Controller
    {
        private RequestBussinessLogic requestBussinessLogic = new RequestBussinessLogic();
        private EntitleDomain entitleDomain = new EntitleDomain();
        private DocumentBussinessLogic documentBussinessLogic = new DocumentBussinessLogic();
        private UpdateStatusBussiness updateS = new UpdateStatusBussiness();
        private RequestDataAccess reqDA = new RequestDataAccess();
        private HttpContext context;

        private ICatalogReflexionHelper _catalogReflexionHelper;
        public AdministratorController(ICatalogReflexionHelper catalogReflexionHelper)
        {
            _catalogReflexionHelper = catalogReflexionHelper;
        }

        // GET: Administrator
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Documentos(string NoIssste, string Request, string Observations)
        {
            var lstDocuments = documentBussinessLogic.GetListDocumentsByRequestId(new Guid(Request));
            var documents = "";
            context = System.Web.HttpContext.Current;
            if ((bool)context.Session["StateObservations"])
            {
                var observations = JsonConvert.SerializeObject(context.Session["Observations"]);
                documents = observations;
            }
            else
            {
                var observations = new List<string>();
                foreach(var document in lstDocuments)
                {
                    observations.Add(document.Observations);
                }
                documents = JsonConvert.SerializeObject(observations);
            }

            ViewBag.documents = documents.Equals("[]") ? "" : documents;
            ViewBag.StateObservations = (bool)context.Session["StateObservations"];
            @ViewBag.Page = "Documentos";
            @ViewBag.ReturnUrl = Request;
            @ViewBag.NoIssste = NoIssste;
            return View(lstDocuments);
        }

        public ActionResult Seguimiento(string NoIssste, string Request, string Observations)
        {

            HttpRequestMessage req = new HttpRequestMessage();
            var owinContext = req.GetOwinContext();

            @ViewBag.Page = "Seguimiento";
            @ViewBag.ReturnUrl = Request;
            @ViewBag.NoIssste = NoIssste;

            var result = documentBussinessLogic.GetResult(Guid.Parse(Request));

            return View(result);

        }
        public ActionResult ResultadoRevision(string NoIssste, string Request, string Observations)
        {
            @ViewBag.Page = "ResultadoRevision";
            @ViewBag.ReturnUrl = Request;
            @ViewBag.NoIssste = NoIssste;
            @ViewBag.RequestId = Request;

            return View();
        }
        public ActionResult UpdateRequestStatus(string RequestId, string StatusID)
        {

            //Request = @ViewBag.RequestId;
            reqDA.UpdateRequestStatus(RequestId, StatusID);


            return View("Index");
        }




        public ActionResult Detail(string NoIssste, string Request, string Observations)
        {

            @ViewBag.Page = "Detail";
            @ViewBag.ReturnUrl = Request;
            @ViewBag.NoIssste = NoIssste;

            var result = entitleDomain.GetEntitle(NoIssste);
            if (result.Gender == "H")
                result.Gender = "HOMBRE";
            else
                result.Gender = "MUJER";
            return View("DetailEntitle", result);
        }


        public ActionResult RequestView(string NoIssste, string Request, string Observations)
        {
            @ViewBag.Page = "RequestView";
            @ViewBag.ReturnUrl = Request;
            @ViewBag.NoIssste = NoIssste;


            var entitle = entitleDomain.GetEntitle(NoIssste);
            var request = requestBussinessLogic.GetRequestByEntitleId(entitle.EntitleId);
            return View(request);
        }

        public ActionResult RedirectPage(string NoIssste, string Page, string Request)
        {

            return RedirectToAction(Page, new
            {
                NoIssste = NoIssste,
                Request = Request,
                Page = Page
            });
        }

        public ActionResult GetImage(string id, string documentTypeId)
        {
            var image = documentBussinessLogic.GetImage(new Guid(id), int.Parse(documentTypeId));

            string contentType = string.Empty;
            string fileName = "Archivo." + image.FileExtension;
            if (image.FileExtension.ToLower() == ".pdf")
            {
                contentType = "application/pdf";
            }
            else if (image.FileExtension.ToLower() == ".jpg")
            {
                contentType = "image/png";
            }
            else if (image.FileExtension.ToLower() == ".png")
            {
                contentType = "image/png";
            }
            else
            {
                contentType = "application/pdf";
            }

            return File(image.Data, contentType, fileName);


        }



        public ActionResult GetImageView(string id, string documentTypeId)
        {
            var image = documentBussinessLogic.GetImage(new Guid(id), int.Parse(documentTypeId));

            string contentType = string.Empty;

            if (image.FileExtension.ToLower() == ".pdf")
            {
                contentType = "application/pdf";
                MemoryStream imagen = new MemoryStream(image.Data);
                return File(imagen.ToArray(), contentType);
            }
            else if (image.FileExtension.ToLower() == ".jpg")
            {
                contentType = "image/pjpeg";
                MemoryStream imagen = new MemoryStream(image.Data);
                return File(imagen.ToArray(), contentType);
            }
            else if (image.FileExtension.ToLower() == ".png")
            {
                contentType = "image/png";
                MemoryStream imagen = new MemoryStream(image.Data);
                return File(imagen.ToArray(), contentType);
            }
            else
            {
                contentType = "application/pdf";
                MemoryStream imagen = new MemoryStream(image.Data);
                return File(imagen.ToArray(), contentType);
            }

        }
        /// <summary>
        /// Returns the list view of a catalog
        /// </summary>
        /// <param name="catalogName">Name of the catalog to display</param>
        /// <returns><see cref="ViewResult"/></returns>
        public ActionResult CatalogElements(string catalogName)
        {
            var name = _catalogReflexionHelper.GetCatalogDisplayName(catalogName);
            var key = _catalogReflexionHelper.GetCatalogKeyPropertyName(catalogName);
            var propertiesToShow = _catalogReflexionHelper.GetPropertiesToDisplayInListView(catalogName);

            var model = new CatalogViewModel
            {
                CatalogDisplayName = name,
                CatalogName = catalogName,
                KeyProperty = key,
                Properties = propertiesToShow
            };

            return View(model);
        }
        /// <summary>
        /// Returns the detail view of a catalog
        /// </summary>
        /// <param name="catalogName">Name of the catalog to display</param>
        /// <returns><see cref="ViewResult"/></returns>
        public ActionResult CatalogItemDetail(string catalogName)
        {
            var name = _catalogReflexionHelper.GetCatalogDisplayName(catalogName);
            var propertiesToShow = _catalogReflexionHelper.GetPropertiesToDisplayInDetailView(catalogName);

            var model = new CatalogViewModel
            {
                CatalogDisplayName = name,
                CatalogName = catalogName,
                Properties = propertiesToShow
            };

            return View(model);
        }

    }


}