using System.Web.Mvc;
using Blogfolio.Models;
using Blogfolio.Web.Areas.Admin.ViewModels;
using Blogfolio.Web.ViewModels;
using ErenPinaz.Common.Services.Settings;

namespace Blogfolio.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly ISettingsService SettingsService;

        public BaseController(ISettingsService settingsService)
        {
            SettingsService = settingsService;
        }

        public BaseController(IUnitOfWork unitOfWork, ISettingsService settingsService)
        {
            UnitOfWork = unitOfWork;
            SettingsService = settingsService;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            // Get the view result base
            var viewResultBase = filterContext.Result as ViewResultBase;

            // Check if the view base and the model are present
            if (viewResultBase == null) return;

            var model = viewResultBase.ViewData.Model as BaseModel;
            if (model == null)
            {
                viewResultBase.ViewData.Model = new BaseModel();
            }

            // Set the base model and populate data
            model = (BaseModel) viewResultBase.ViewData.Model;
            model.SiteSettings = SettingsService.GetByName<SiteSettingsEditModel>("site-settings");
            model.SocialSettings = SettingsService.GetByName<SocialSettingsEditModel>("social-settings");
        }
    }
}