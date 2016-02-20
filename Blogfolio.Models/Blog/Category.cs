using System;
using System.Collections.Generic;

namespace Blogfolio.Models.Blog
{
    public class Category
    {
        #region Fields

        private ICollection<Post> _posts;

        #endregion

        #region Scalar Properties

        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Post> Posts
        {
            get { return _posts ?? (_posts = new List<Post>()); }
            set { _posts = value; }
        }

        #endregion
    }
}