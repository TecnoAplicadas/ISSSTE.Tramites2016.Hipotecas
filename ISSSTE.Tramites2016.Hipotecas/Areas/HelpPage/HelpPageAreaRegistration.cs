#region

using System.Web.Http;
using System.Web.Mvc;
using ISSSTE.Tramites2016.Hipotecas.Areas.HelpPage.App_Start;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Areas.HelpPage
{
    public class HelpPageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "HelpPage"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HelpPage_Default",
                "Help/{action}/{apiId}",
                new {controller = "Help", action = "Index", apiId = UrlParameter.Optional});

            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}