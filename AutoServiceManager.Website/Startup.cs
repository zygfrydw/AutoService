using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoServiceManager.Website.Startup))]
namespace AutoServiceManager.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
