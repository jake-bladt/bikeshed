using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mvc5Shell.Startup))]
namespace Mvc5Shell
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
