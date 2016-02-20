using System;

namespace Blogfolio.Models.Identity
{
    public class Claim
    {
        #region Fields

        private User _user;

        #endregion

        #region Scalar Properties

        public virtual int ClaimId { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }

        #endregion

        #region Navigation Properties

        public virtual User User
        {
            get { return _user; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _user = value;
                UserId = value.UserId;
            }
        }

        #endregion
    }
}