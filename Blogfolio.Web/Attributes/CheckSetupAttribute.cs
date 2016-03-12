using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Blogfolio.Web.Attributes
{
    /// <summary>
    /// Checks the setup status value from the web.config
    /// file and prevents access to the main app if it is
    /// in progress (value: 0)
    /// </summary>
    public class CheckSetupAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var setupStatus = int.Parse(WebConfigurationManager.AppSettings["SetupStatus"]);
            var action = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");

            if (setupStatus == 0)
            {
                if (!action.Equals("CreateAdmin", StringComparison.InvariantCultureIgnoreCase))
                {
                    filterContext.HttpContext.Response.Redirect("/setup");
                }
            }
            else if (setupStatus == 1)
            {
                if (action.Equals("CreateAdmin", StringComparison.InvariantCultureIgnoreCase))
                {
                    filterContext.HttpContext.Response.Redirect("/");
                }
            }
        }
    }
}