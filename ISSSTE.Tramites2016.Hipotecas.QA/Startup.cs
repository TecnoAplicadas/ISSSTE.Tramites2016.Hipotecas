#region

using ISSSTE.Tramites2016.Hipotecas;
using Microsoft.Owin;
using Owin;

#endregion



[assembly: OwinStartup(typeof(Startup))]

namespace ISSSTE.Tramites2016.Hipotecas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
