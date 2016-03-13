using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blogfolio.Web.Areas.Admin.ViewModels;
using Blogfolio.Web.ViewModels;
using ErenPinaz.Common.Services.Captcha;
using ErenPinaz.Common.Services.Email;
using ErenPinaz.Common.Services.Settings;

namespace Blogfolio.Web.Controllers
{
    public class ContactController : BaseController
    {
        private readonly string _contactEmail;
        private readonly string _recaptchaKey;
        private readonly string _recaptchaSecret;

        private readonly IEmailService _emailService;
        private readonly ICaptchaService _captchaService;

        public ContactController(ISettingsService settingsService, IEmailService emailService,
            ICaptchaService captchaService)
            : base(settingsService)
        {
            var siteSettings = SettingsService.GetByName<SiteSettingsEditModel>("site-settings");

            _contactEmail = siteSettings.ContactEmail;
            _recaptchaKey = siteSettings.ReCaptchaKey;
            _recaptchaSecret = siteSettings.ReCaptchaSecret;

            _emailService = emailService;
            _captchaService = captchaService;
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

            if (!string.IsNullOrWhiteSpace(_recaptchaKey) && !string.IsNullOrWhiteSpace(_recaptchaSecret))
            {
                var captchaResponse =
                    await _captchaService.ValidateAsync(_recaptchaSecret, Request.Form["g-recaptcha-response"]);
                if (!captchaResponse.Success)
                {
                    return
                        Json(
                            new
                            {
                                success = false,
                                message =
                                    string.Format("Captcha validation failed. ({0})", captchaResponse.ErrorCodes[0])
                            });
                }
            }

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