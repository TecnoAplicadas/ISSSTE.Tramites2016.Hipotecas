#region

using System;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Hipotecas.Areas.HelpPage.Controllers;
using Microsoft.Practices.Unity;
using ISSSTE.Tramites2016.Hipotecas.Domian.Implementation;

using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Domian;
using ISSSTE.Tramites2016.Common.Reports;

using ISSSTE.Tramites2016.Common.Mail;
using ISSSTE.Tramites2016.Common.ServiceAgents;
using ISSSTE.Tramites2016.Common.ServiceAgents.Implementation;
using ISSSTE.Tramites2016.Common.Reports.Implementation;
using ISSSTE.Tramites2016.Hipotecas.Domians;
using ISSSTE.Tramites2016.Common.Catalogs;
using ISSSTE.Tramites2016.Common.Catalogs.Implemantions;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas
{
    /// <summary>
    ///     Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        ///     There is no need to register concrete types such as controllers or API controllers (unless you want to
        ///     change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // Register your types here
            container.RegisterType<ILogger, Logger>();

            container.RegisterType<ISipeAvDataServiceAgent, SipeAvDataServiceAgent>();
            container.RegisterType<IEntitleDomainService, EntitleDomainService>();
            container.RegisterType<HelpController>(new InjectionConstructor());
            container.RegisterType<IUnitOfWork, HipotecasContext>();
            container.RegisterType<ICatalogReflexionHelper, CatalogReflexionHelper>(new InjectionConstructor(typeof(Request).Namespace));
            container.RegisterType<ICalendarDomainService, CalendarDomainService>();
            container.RegisterType<ICommonDomainService, CommonDomainService>();
            container.RegisterType<IEntitleDomainService, EntitleDomainService>();
            container.RegisterType<IRequestDomainService, RequestDomainService>();
            container.RegisterType<IMortgageReportHelper, MortgageReportHelper>();
        }

        #region Unity Container

        private static readonly Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        ///     Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        #endregion
    }
}