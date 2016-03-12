using System.Data.Entity;
using Blogfolio.Data.Configuration.Blog;
using Blogfolio.Data.Configuration.Identity;
using Blogfolio.Data.Configuration.Library;
using Blogfolio.Data.Configuration.Portfolio;
using Blogfolio.Models.Blog;
using Blogfolio.Models.Identity;
using Blogfolio.Models.Library;
using Blogfolio.Models.Portfolio;

namespace Blogfolio.Data
{
    /// <summary>
    /// Creates the database
    /// </summary>
    internal class BlogfolioContext : DbContext
    {
        /// <summary>
        /// Public constructor for migrations
        /// </summary>
        public BlogfolioContext()
            : base("BlogfolioContext")
        {
        }

        /// <summary>
        /// Constructor for dependency injector
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        internal BlogfolioContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        // Identity
        internal DbSet<User> Users { get; set; }
        internal DbSet<Role> Roles { get; set; }
        internal IDbSet<ExternalLogin> ExternalLogins { get; set; }

        // Blog
        internal DbSet<Post> Posts { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<Comment> Comments { get; set; }

        // Portfolio
        internal DbSet<Project> Projects { get; set; }

        // Library
        internal DbSet<Media> Medias { get; set; }

        /// <summary>
        /// Configures values & table relationships
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Identity
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new ExternalLoginConfiguration());
            modelBuilder.Configurations.Add(new ClaimConfiguration());

            // Blog
            modelBuilder.Configurations.Add(new PostConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());

            // Portfolio
            modelBuilder.Configurations.Add(new ProjectConfiguration());

            // Library
            modelBuilder.Configurations.Add(new MediaConfiguration());
        }
    }
}