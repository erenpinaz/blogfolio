using System.Web.Mvc;
using Blogfolio.Web.Areas.Admin.ViewModels;
using ErenPinaz.Common.Services.Settings;

namespace Blogfolio.Web.Areas.Admin.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public ActionResult General(string message)
        {
            ViewBag.StatusMessage = message;

            var model = _settingsService.GetByName<SiteSettingsEditModel>("site-settings");
            return View(model);
        }

        [HttpPost, ActionName("General")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGeneral(SiteSettingsEditModel model)
        {
            if (ModelState.IsValid)
            {
                _settingsService.Save(model, "site-settings");
            }

            return RedirectToAction("General", "Settings", new {message = "Changes saved succesfully"});
        }

        [HttpGet]
        public ActionResult Social(string message)
        {
            ViewBag.StatusMessage = message;

            var model = _settingsService.GetByName<SocialSettingsEditModel>("social-settings");
            return View(model);
        }

        [HttpPost, ActionName("Social")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSocial(SocialSettingsEditModel model)
        {
            if (ModelState.IsValid)
            {
                _settingsService.Save(model, "social-settings");
            }

            return RedirectToAction("Social", "Settings", new {message = "Changes saved succesfully"});
        }

        [HttpGet]
        public ActionResult AddSocialItem()
        {
            return PartialView("_SocialItem", new SocialItemEditModel());
        }
    }
}