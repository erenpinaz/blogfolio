using System.ComponentModel.DataAnnotations;

namespace Blogfolio.Web.ViewModels
{
    public class ContactEditModel : BaseModel
    {
        [Required]
        [StringLength(64, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(320, MinimumLength = 3)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [StringLength(1200, MinimumLength = 3)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}