using System;

namespace Blogfolio.Models.Library
{
    public class Media
    {
        #region Scalar Properties

        public Guid MediaId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ThumbPath { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        #endregion
    }
}