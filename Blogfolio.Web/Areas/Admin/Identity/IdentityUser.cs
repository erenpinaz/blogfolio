using System;
using Microsoft.AspNet.Identity;

/* Credits: https://github.com/timschreiber/Mvc5IdentityExample */

namespace Blogfolio.Web.Areas.Admin.Identity
{
    public class IdentityUser : IUser<Guid>
    {
        public IdentityUser()
        {
            Id = Guid.NewGuid();
        }

        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
    }
}