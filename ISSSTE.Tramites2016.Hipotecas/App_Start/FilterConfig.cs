#region

using ISSSTE.Tramites2016.Hipotecas.Filters;
using System.Web.Mvc;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
      
        }
    }
}