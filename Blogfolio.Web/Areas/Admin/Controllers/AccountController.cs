using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Blogfolio.Web.Areas.Admin.Identity;
using Blogfolio.Web.Areas.Admin.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Blogfolio.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser, Guid> _userManager;
        private readonly RoleManager<IdentityRole, Guid> _roleManager;

        public AccountController(UserManager<IdentityUser, Guid> userManager,
            RoleManager<IdentityRole, Guid> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            // Create password policy
            userManager.PasswordValidator = new PasswordValidator()
            {
                RequireDigit = true,
                RequireNonLetterOrDigit = true,
                RequireLowercase = true,
                RequiredLength = 6
            };
        }

        #region Account

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CreateAdmin()
        {
            return View(new CreateAdminEditModel() {UserName = "administrator"});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAdmin(CreateAdminEditModel model)
        {
            if (ModelState.IsValid)
            {
                // Create the user
                var user = new IdentityUser {UserName = "administrator", Name = model.Name, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Create the admin role
                    var role = new IdentityRole {Name = "admin"};
                    result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        // Add user to the admin role
                        result = await _userManager.AddToRoleAsync(user.Id, role.Name);
                        if (result.Succeeded)
                        {
                            // Setup is finished
                            var cfg = WebConfigurationManager.OpenWebConfiguration("~");
                            cfg.AppSettings.Settings["SetupStatus"].Value = "1";
                            cfg.Save();

                            return RedirectToAction("Login", "Account");
                        }
                        AddErrors(result);
                    }
                    AddErrors(result);
                }
                else
                {
                    AddErrors(result);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginEditModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await _userManager.UpdateSecurityStampAsync(user.Id);

                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Wrong username or password");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Manage(string message)
        {
            ViewBag.StatusMessage = message;

            var user = await _userManager.FindByIdAsync(GetGuid(User.Identity.GetUserId()));
            var model = new ManageUserEditModel()
            {
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserEditModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.GetUserName());
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.OldPassword))
                    {
                        user.Name = model.Name;
                        user.Email = model.Email;

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            if (model.NewPassword != null && model.ConfirmPassword != null)
                            {
                                result =
                                    await
                                        _userManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
                                if (result.Succeeded)
                                {
                                    AuthenticationManager.SignOut();
                                    return RedirectToAction("Login", "Account");
                                }
                                else
                                {
                                    AddErrors(result);
                                    return View(model);
                                }
                            }
                            return RedirectToAction("Manage", "Account",
                                new {message = "User updated successfully"});
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("OldPassword", "You've enetered wrong password");
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Blog", new {area = ""});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager?.Dispose();
                _roleManager?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Helpers

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() {IsPersistent = isPersistent}, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Posts", "Dashboard");
            }
        }

        private static Guid GetGuid(string value)
        {
            Guid result;
            Guid.TryParse(value, out result);
            return result;
        }

        #endregion
    }
}