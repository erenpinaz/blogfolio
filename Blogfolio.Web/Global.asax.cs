using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Blogfolio.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            // Register filters
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // Register areas
            AreaRegistration.RegisterAllAreas();

            // Register routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Register bundles
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PreSendRequestHeaders()
        {
            // Remove server signature
            Response.Headers.Remove("Server");

            // Remove asp-net-version signature
            Response.Headers.Remove("X-AspNet-Version");

            // Remove mvc-version signature
            Response.Headers.Remove("X-AspNetMvc-Version");
        }
    }
}