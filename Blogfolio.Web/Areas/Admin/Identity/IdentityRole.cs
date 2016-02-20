using System;
using Microsoft.AspNet.Identity;

/* Credits: https://github.com/timschreiber/Mvc5IdentityExample */

namespace Blogfolio.Web.Areas.Admin.Identity
{
    public class IdentityRole : IRole<Guid>
    {
        public IdentityRole()
        {
            Id = Guid.NewGuid();
        }

        public IdentityRole(string name)
            : this()
        {
            Name = name;
        }

        public IdentityRole(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}