using System.Web.Mvc;
using ErenPinaz.Common.Services.Settings;

namespace Blogfolio.Web.Controllers
{
    public class AboutController : BaseController
    {
        public AboutController(ISettingsService settingsService)
            : base(settingsService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}