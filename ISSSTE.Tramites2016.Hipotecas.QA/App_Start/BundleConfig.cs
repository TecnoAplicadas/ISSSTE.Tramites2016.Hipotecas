#region

using System.Web.Optimization;
using ISSSTE.Tramites2016.Common.Web;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Libraries
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .IncludeDirectory("~/Scripts/Libraries/jQuery", "*.js", false)
                 .IncludeDirectory("~/Scripts", "bootstrap.js", false)
                );





            bundles.Add(new ScriptBundle("~/bundles/ramda")
                .Include(
                    "~/Scripts/Libraries/ramda/ramda.js"
                )
                );

            bundles.Add(new ScriptBundle("~/bundles/angular")
                .Include(
                    "~/Scripts/Libraries/Angular/angular.js",
                    "~/Scripts/Libraries/Angular/angular-route.js",
                    "~/Scripts/Libraries/Angular/angular-sanitize.js",
                    "~/Scripts/Libraries/Angular/angular-local-storage.js",
                    "~/Scripts/Libraries/Angular/angular-local-storage.js"
                )
                );

            bundles.Add(new ScriptBundle("~/bundles/angular-auto-validate")
                .Include(
                    "~/Scripts/Libraries/AngularAutoValidate/jcs-auto-validate.js"
                )
                );

            bundles.Add(new ScriptBundle("~/bundles/angular-upload")
                .Include(
                    "~/Scripts/Libraries/AngularUpload/startup.js",
                    "~/Scripts/Libraries/AngularUpload/ng-file-upload-all.js"
                )
                );

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datetimepicker")
                .Include(
                    "~/Scripts/Libraries/BootstrapDatetimePicker/moment-with-locales.js",
                    "~/Scripts/Libraries/BootstrapDatetimePicker/bootstrap-datetimepicker.js",
                    "~/Scripts/Libraries/BootstrapDatetimePicker/angular-bootstrap-datetimepicker-directive.js"
                )
                );

            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .IncludeDirectory("~/Scripts/Libraries/Modernizr", "*.js", false)
                );

            bundles.Add(new ScriptBundle("~/bundles/respond")
                .IncludeDirectory("~/Scripts/Libraries/Respond", "*.js", false)
                );

            bundles.Add(new ScriptBundle("~/bundles/utils")
                .IncludeDirectory("~/Scripts/Libraries/Util", "*.js", false)
                );

            //Applications CSS and UI Scripts
            bundles.Add(new StyleBundle("~/bundles/entitle/css")
                .IncludeWithCssRewriteTransform(
                    "~/Content/Entitle/abl.css",
                    "~/Content/Entitle/bootstrap-datetimepicker.css",
                    "~/Content/Entitle/breadcrum.css",
                    "~/Content/Entitle/custom.css",
                    "~/Content/Entitle/fileupload.css",
                    "~/Content/Entitle/iframe.css",
                    "~/Content/Entitle/site.css"
                )
                );
            //"~/Content/Entitle/calendario.css",
            //   "~/Content/Entitle/gobmx.css",
            // "~/Content/Entitle/abl.css",
            bundles.Add(new ScriptBundle("~/bundles/entitle/javascript")
                .Include(
                    "~/Scripts/Entitle/UI/bootstrap.min.js",
                    "~/Scripts/Entitle/UI/scripts.js",
                    "~/Scripts/Entitle/UI/jquery.dataTables.min.js",
                    "~/Scripts/Entitle/UI/dataTables.bootstrap.js",
                    "~/Scripts/Entitle/UI/dataTables.responsive.min.js"
                )
                );
            //"~/Scripts/Entitle/UI/jquery.min.js",
            //"~/Scripts/Entitle/UI/gobmx.js",
            //"~/Scripts/Entitle/UI/jquery.dataTables.min.js",  
            //"~/Scripts/Entitle/UI/dataTables.bootstrap.js",
            // "~/Scripts/Entitle/UI/dataTables.responsive.min.js"
            //"~/Scripts/Entitle/UI/gobmx-scripts/main.js",
            //"~/Scripts/Entitle/UI/gobmx-scripts/plugins.js",
            bundles.Add(new StyleBundle("~/bundles/administrator/css")
                .IncludeWithCssRewriteTransform(
                    "~/Content/Administrator/bootstrap.css",
                    "~/Content/Administrator/font-awesome.min.css",
                    "~/Content/Administrator/general.css",
                    "~/Content/Administrator/menu.css",
                    "~/Content/Administrator/menu-lateral.css",
                    "~/Content/Administrator/buscador.css",
                    "~/Content/Administrator/site.css",
                    "~/Content/Administrator/jquery.dataTables.min.css",
                    "~/Content/Administrator/bootstrap-datetimepicker.css"
                )
                );

            bundles.Add(new ScriptBundle("~/bundles/administrator/javascript")
                .Include(
                    "~/Scripts/Administrator/UI/bootstrap.js",
                    "~/Scripts/Administrator/UI/scripts.js",
                    "~/Scripts/Administrator/UI/agregar-tabla.js"
                )
                );

            //Application Angular Modules
            bundles.Add(new ScriptBundle("~/bundles/entitle/app")
                .Include(
                    "~/Scripts/Entitle/App/app.js",
                    "~/Scripts/Entitle/App/config.js",
                    "~/Scripts/Entitle/App/config.exceptionHandler.js",
                    "~/Scripts/Entitle/App/config.routes.js"
                )
                 .IncludeDirectory("~/Scripts/Entitle/App/Appoinments", "*.js", true)
                .IncludeDirectory("~/Scripts/Entitle/App/resources", "*.js", true)
                .IncludeDirectory("~/Scripts/Entitle/App/common", "*.js", true)
                .IncludeDirectory("~/Scripts/Entitle/App/error", "*.js", true)
                .IncludeDirectory("~/Scripts/Entitle/App/home", "*.js", true)
                .IncludeDirectory("~/Scripts/Entitle/App/requests", "*.js", true)
                );

            bundles.Add(new ScriptBundle("~/bundles/administrator/app")
                .IncludeDirectory("~/Scripts/Administrator/App/resources", "*.js", true)
                .Include(
                    "~/Scripts/Administrator/App/app.js",
                    "~/Scripts/Administrator/App/config.js",
                    "~/Scripts/Administrator/App/config.exceptionHandler.js",
                    "~/Scripts/Administrator/App/config.routes.js"
                )
                .IncludeDirectory("~/Scripts/Administrator/App/common", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/catalogs", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/error", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/Appointments", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/search", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/requests", "*.js", true)
               .IncludeDirectory("~/Scripts/Administrator/App/reports", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/login", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/calendar", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/dates", "*.js", true)
                 .IncludeDirectory("~/Scripts/Administrator/App/messages", "*.js", true)
                  .IncludeDirectory("~/Scripts/Administrator/App/support", "*.js", true)
                  .IncludeDirectory("~/Scripts/Administrator/App/statusManager", "*.js", true)
                );

            bundles.Add(new ScriptBundle("~/bundles/administrator/app/login")
                .IncludeDirectory("~/Scripts/Administrator/App/resources", "*.js", true)
                .Include(
                    "~/Scripts/Administrator/App/app.js",
                    "~/Scripts/Administrator/App/config.js",
                    "~/Scripts/Administrator/App/config.exceptionHandler.js"
                )
                .IncludeDirectory("~/Scripts/Administrator/App/common", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/login", "*.js", true)
                );

            //Se deshabilita el bundle, pues cuando se minifica se tiene un problema con las rutas de las fuentes de bootstrap
            BundleTable.EnableOptimizations = false;

            //#if DEBUG
            //            BundleTable.EnableOptimizations = false;
            //#else
            //            BundleTable.EnableOptimizations = true;
            //#endif
        }
    }
}