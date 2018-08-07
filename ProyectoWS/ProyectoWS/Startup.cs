using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProyectoWS.Startup))]
namespace ProyectoWS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
