using System;
using Blogfolio.Data;
using Blogfolio.Models;
using Blogfolio.Web.Areas.Admin.Identity;
using ErenPinaz.Common.Services.Captcha;
using ErenPinaz.Common.Services.Email;
using ErenPinaz.Common.Services.Settings;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;

namespace Blogfolio.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or API controllers (unless you want to
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager(),
                new InjectionConstructor("BlogfolioContext"));

            container.RegisterType<IUserStore<IdentityUser, Guid>, UserStore>();
            container.RegisterType<IRoleStore<IdentityRole, Guid>, RoleStore>();

            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<ICaptchaService, ReCaptchaService>();
            container.RegisterType<ISettingsService, JsonSettingsService>();
        }
    }
}