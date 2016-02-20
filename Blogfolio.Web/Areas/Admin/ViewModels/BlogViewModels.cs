using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Blogfolio.Models.Blog;

namespace Blogfolio.Web.Areas.Admin.ViewModels
{
    public class PostEditModel
    {
        public Guid PostId { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(320, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [AllowHtml]
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 3)]
        [Display(Name = "Slug", Description = "URL-friendly version of the name")]
        public string Slug { get; set; }

        [DisplayName("Comments Enabled")]
        public bool CommentsEnabled { get; set; }

        [Display(Name = "Status")]
        public PostStatus Status { get; set; }
    }

    public class CategoryEditModel
    {
        public Guid CategoryId { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 3)]
        [Display(Name = "Slug", Description = "URL-friendly version of the name")]
        public string Slug { get; set; }
    }
}