#region

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
               name: "Administrador",
               url: "Administrador",
               defaults: new { controller = "Administrator", action = "Index" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "entitle", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}