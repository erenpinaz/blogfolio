using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blogfolio.Web.Areas.Admin.ViewModels
{
    public class SiteSettingsEditModel
    {
        [Required]
        [StringLength(12, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(36, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Tagline")]
        public string Tagline { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Heading")]
        public string Heading { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Logo Path", Description = "URL for the site logo image")]
        public string LogoPath { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Site URL")]
        public string SiteUrl { get; set; }

        [Required]
        [StringLength(160, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Meta Description", Description = "Should be kept between 150-160 characters")]
        public string MetaDescription { get; set; }

        [StringLength(160, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Meta Keywords", Description = "Should be separated with comma (,) (Optional)")]
        public string MetaKeywords { get; set; }

        [StringLength(128, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Disqus Shortname",
            Description =
                "Register your site at <a href=\"https://disqus.com/admin/signup\" target=\"_blank\" > Disqus</a> to enable comments (Optional)"
            )]
        public string DisqusShortname { get; set; }

        [StringLength(128, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Shareaholic Key",
            Description =
                "Register your site at <a href=\"https://shareaholic.com/signup\" target=\"_blank\">Shareaholic</a> to enable sharing (Optional)"
            )]
        public string ShareaholicKey { get; set; }

        [StringLength(128, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Google Analytics Key",
            Description =
                "Register your site at <a href=\"https://www.google.com/analytics\" > Google Analytics</a> to enable site analysis (Optional)"
            )]
        public string GoogleAnalyticsKey { get; set; }

        [Required]
        [Range(1, 20)]
        [DataType(DataType.Text)]
        [Display(Name = "Page Size", Description = "How many posts will be displayed per page")]
        public int PageSize { get; set; }

        [Required]
        [Range(1, 20)]
        [DataType(DataType.Text)]
        [Display(Name = "Feed Size", Description = "How many posts will be displayed in the feed")]
        public int FeedSize { get; set; }

        [Required]
        [StringLength(320, MinimumLength = 3)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Contact Email", Description = "This field will be used in contact form")]
        public string ContactEmail { get; set; }
    }

    public class SocialSettingsEditModel
    {
        public SocialSettingsEditModel()
        {
            SocialItems = new List<SocialItemEditModel>();
        }

        public List<SocialItemEditModel> SocialItems { get; set; }
    }

    public class SocialItemEditModel
    {
        [Required]
        [StringLength(12, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Name", Description = "Unique identifier for the item")]
        public string Name { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Url")]
        public string Url { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Icon")]
        public string Icon { get; set; }
    }
}