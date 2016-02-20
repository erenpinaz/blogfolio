using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Blogfolio.Models.Portfolio;

namespace Blogfolio.Web.Areas.Admin.ViewModels
{
    public class ProjectEditModel
    {
        public Guid ProjectId { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Image", Description = "Relative path of an image file")]
        public string Image { get; set; }

        [AllowHtml]
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Slug", Description = "URL-friendly version of the name")]
        public string Slug { get; set; }

        [Display(Name = "Status")]
        public ProjectStatus Status { get; set; }
    }
}