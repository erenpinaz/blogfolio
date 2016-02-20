using System;
using System.Collections.Generic;
using Blogfolio.Models.Identity;

namespace Blogfolio.Models.Blog
{
    public class Post
    {
        #region Fields

        private ICollection<Category> _categories;
        private ICollection<Comment> _comments;

        #endregion

        #region Scalar Properties

        public Guid PostId { get; set; }
        public virtual Guid UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public string Slug { get; set; }
        public bool CommentsEnabled { get; set; }
        public PostStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Category> Categories
        {
            get { return _categories ?? (_categories = new List<Category>()); }
            set { _categories = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return _comments ?? (_comments = new List<Comment>()); }
            set { _comments = value; }
        }

        public virtual User User { get; set; }

        #endregion
    }
}