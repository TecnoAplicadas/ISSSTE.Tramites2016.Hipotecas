using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace ISSSTE.Tramites2016.Hipotecas.Filters
{
    public class IsssteNumberFilter : ActionFilterAttribute
    {

        public IsssteNumberFilter()
        {
            View = "../Entitle/Empty";
        }

        public string View
        {
            get;
            set;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string noIssste = filterContext?.ActionParameters?["NoIssste"]?.ToString() ?? "";

            var view = new ViewResult();
            view.ViewName = View;
            var result = view;
            if (filterContext.HttpContext.Session.Contents["NoIssste"] != null)
            {

                string session = filterContext.HttpContext.Session.Contents["NoIssste"].ToString();

                if (String.IsNullOrEmpty(noIssste) || !filterContext.HttpContext.Session["NoIssste"].ToString().Equals(noIssste))
                {
                    filterContext.Result = result;
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}