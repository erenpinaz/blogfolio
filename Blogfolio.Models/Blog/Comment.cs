using System;

namespace Blogfolio.Models.Blog
{
    public class Comment
    {
        #region Scalar Properties

        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Content { get; set; }
        public CommentStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Post Post { get; set; }

        #endregion
    }
}