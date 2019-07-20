using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(POCServices.Startup))]

namespace POCServices
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}