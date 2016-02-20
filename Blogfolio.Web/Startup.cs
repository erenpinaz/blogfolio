using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof (Blogfolio.Web.Startup))]

namespace Blogfolio.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}