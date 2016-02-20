using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blogfolio.Web.Areas.Admin.ViewModels;
using Blogfolio.Web.ViewModels;
using ErenPinaz.Common.Services.Email;
using ErenPinaz.Common.Services.Settings;

namespace Blogfolio.Web.Controllers
{
    public class ContactController : BaseController
    {
        private readonly string _contactEmail;

        private readonly IEmailService _emailService;

        public ContactController(ISettingsService settingsService, IEmailService emailService)
            : base(settingsService)
        {
            var siteSettings = SettingsService.GetByName<SiteSettingsEditModel>("site-settings");
            _contactEmail = siteSettings.ContactEmail;

            _emailService = emailService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new ContactEditModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMessage(ContactEditModel model)
        {
            if (!ModelState.IsValid)
                return Json(new {success = false, message = "Please correctly fill all the required fields."});

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(model.Email, model.Name),
                To = {_contactEmail},
                Subject = model.Subject,
                Body = model.Message
            };
            try
            {
                await _emailService.SendAsync(mailMessage);
                return Json(new {success = true, message = "Your message has been successfully sent."});
            }
            catch
            {
                return Json(new {success = false, message = "An error occurred while sending your mail."});
            }
            finally
            {
                mailMessage.Dispose();
            }
        }
    }
}