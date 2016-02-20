using System.Web.Mvc;

namespace Blogfolio.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Route: Setup
            context.MapRoute(
                "Setup",
                "setup",
                new {controller = "Account", action = "CreateAdmin"}
                );

            // Route Manage
            context.MapRoute(
                "Manage",
                "admin/manage",
                new {controller = "Account", action = "Manage"}
                );

            // Route Logoff
            context.MapRoute(
                "LogOff",
                "admin/logoff",
                new {controller = "Account", action = "LogOff"}
                );

            // Route Login
            context.MapRoute(
                "Login",
                "admin/login",
                new {controller = "Account", action = "Login"}
                );

            // Route: Admin Default
            context.MapRoute(
                "AdminDefault",
                "admin/{controller}/{action}/{id}",
                new {controller = "Dashboard", action = "Services", id = UrlParameter.Optional}
                );
        }
    }
}