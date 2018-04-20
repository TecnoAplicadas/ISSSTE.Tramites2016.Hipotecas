#region
using System.Configuration;
using System.Web.Http;
using ISSSTE.Tramites2016.Common.Mail;
using ISSSTE.Tramites2016.Common.ServiceAgents;
using ISSSTE.Tramites2016.Common.ServiceAgents.Implementation;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Domian;
using ISSSTE.Tramites2016.Hipotecas.Domian.Implementation;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using ISSSTE.Tramites2016.Hipotecas.Domians;
using ISSSTE.Tramites2016.Common.Reports.Implementation;
using ISSSTE.Tramites2016.Common.Reports;
using ISSSTE.Tramites2016.Common.Catalogs.Implemantions;
using ISSSTE.Tramites2016.Common.Catalogs;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Common.DataAccess;
using ISSSTE.Tramites2016.Common.DataAccess.Implementations;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas
{
    public static class UnityApiConfig
    {
        #region Static Properties

        /// <summary>
        /// Obtiene la ruta relativa donde se encuentra el html a utilizar como plantilla de los correos
        /// </summary>
        private static string MailMasterPagePath
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["MailMasterPagePath"]);
            }
        }

        /// <summary>
        /// Obtiene la ruta relativa donde se encuentra la a utilizar como logo para los correos
        /// </summary>
        private static string MailMasterPageLogoPath
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["MailMasterPageLogoPath"]);
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Registra los tipos necesarios para los controladores Web API
        /// </summary>
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IMailService, MailService>(new InjectionConstructor(MailMasterPagePath, MailMasterPageLogoPath));
            container.RegisterType<ICatalogReflexionHelper, CatalogReflexionHelper>(new InjectionConstructor(typeof(Request).Namespace));
            container.RegisterType<ISipeAvDataServiceAgent, SipeAvDataServiceAgent>();
            container.RegisterType<IUnitOfWork, HipotecasContext>();
            container.RegisterType<ICalendarDomainService, CalendarDomainService>();
            container.RegisterType<ICommonDomainService, CommonDomainService>();
            container.RegisterType<IEntitleDomainService, EntitleDomainService>();
            container.RegisterType<IRequestDomainService, RequestDomainService>();
            container.RegisterType<IMortgageReportHelper, MortgageReportHelper>();
            container.RegisterType<ICatalogRepository, CatalogRepository>();
            container.RegisterType<IConection, Common.DataAccess.Implementations.Conection>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        #endregion
    }
}