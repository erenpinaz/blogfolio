using System.ComponentModel.DataAnnotations;

namespace Blogfolio.Web.Areas.Admin.ViewModels
{
    public class ManageUserEditModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "User Name", Description = "This field cannot be changed")]
        public string UserName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Name", Description = "This field will be publicly displayed")]
        public string Name { get; set; }

        [Required]
        [StringLength(320, MinimumLength = 3)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password", Description = "You will be prompted to login with your new password")]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginEditModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class CreateAdminEditModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "User name", Description = "This field cannot be changed")]
        public string UserName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 3)]
        [Display(Name = "Name", Description = "This field will be publicly displayed")]
        public string Name { get; set; }

        [Required]
        [StringLength(320, MinimumLength = 3)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}