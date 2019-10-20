using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Agnos.Startup))]
namespace Agnos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
