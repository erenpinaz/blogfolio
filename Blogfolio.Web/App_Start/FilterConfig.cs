using System.Web.Mvc;
using Blogfolio.Web.Attributes;

namespace Blogfolio.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // Check for the setup status and redirect accordingly
            filters.Add(new CheckSetupAttribute());
        }
    }
}