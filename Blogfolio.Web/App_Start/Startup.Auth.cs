using System;
using System.Web.Mvc;
using Blogfolio.Web.Areas.Admin.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Blogfolio.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<UserManager<IdentityUser, Guid>>());

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/admin/login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<UserManager<IdentityUser, Guid>, IdentityUser, Guid>(
                            validateInterval: TimeSpan.FromMinutes(0),
                            regenerateIdentityCallback:
                                (manager, user) =>
                                    manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie),
                            getUserIdCallback: (id) => (Guid.Parse(id.GetUserId())))
                }
            });
        }
    }
}