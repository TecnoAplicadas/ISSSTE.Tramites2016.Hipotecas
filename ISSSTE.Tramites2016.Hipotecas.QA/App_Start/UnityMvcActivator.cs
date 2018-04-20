#region

using System.Linq;
using System.Web.Mvc;
using ISSSTE.Tramites2016.Hipotecas;
using Microsoft.Practices.Unity.Mvc;
using WebActivatorEx;

#endregion

[assembly: PreApplicationStartMethod(typeof (UnityWebActivator), "Start")]
[assembly: ApplicationShutdownMethod(typeof (UnityWebActivator), "Shutdown")]

namespace ISSSTE.Tramites2016.Hipotecas
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {
            var container = UnityConfig.GetConfiguredContainer();

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}