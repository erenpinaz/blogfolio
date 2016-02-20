using System;

namespace Blogfolio.Models.Portfolio
{
    public class Project
    {
        #region Scalar Properties

        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        #endregion
    }
}