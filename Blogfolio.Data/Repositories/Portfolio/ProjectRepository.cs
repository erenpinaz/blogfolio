using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Portfolio;
using Blogfolio.Models.Repositories.Portfolio;

namespace Blogfolio.Data.Repositories.Portfolio
{
    /// <summary>
    /// Entity framework implementation of <see cref="IProjectRepository" />
    /// </summary>
    internal class ProjectRepository : Repository<Project>, IProjectRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        internal ProjectRepository(BlogfolioContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Returns a list of public projects
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of public <see cref="Project" /></returns>
        public List<Project> GetProjects(int count = 0)
        {
            var projects = Set.Where(p => p.Status == ProjectStatus.Public)
                .OrderBy(p => p.DateCreated);

            return count > 0
                ? projects.Take(count).ToList()
                : projects.ToList();
        }

        /// <summary>
        /// Asynchronously returns a list of public projects
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of public <see cref="Project" /></returns>
        public Task<List<Project>> GetProjectsAsync(int count = 0)
        {
            var projects = Set.Where(p => p.Status == ProjectStatus.Public)
                .OrderBy(p => p.DateCreated);

            return count > 0
                ? projects.Take(count).ToListAsync()
                : projects.ToListAsync();
        }

        /// <summary>
        /// Asynchronously returns a list of public projects
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="count"></param>
        /// <returns>List of public <see cref="Project" /></returns>
        public Task<List<Project>> GetProjectsAsync(CancellationToken cancellationToken, int count = 0)
        {
            var projects = Set.Where(p => p.Status == ProjectStatus.Public)
                .OrderBy(p => p.DateCreated);

            return count > 0
                ? projects.Take(count).ToListAsync(cancellationToken)
                : projects.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Returns single public project
        /// </summary>
        /// <param name="slug"></param>
        /// <returns>A public <see cref="Project" /></returns>
        public Project GetProject(string slug)
        {
            var project = Set.FirstOrDefault(p => p.Slug == slug && p.Status == ProjectStatus.Public);

            return project;
        }

        /// <summary>
        /// Asynchronously returns single public project
        /// </summary>
        /// <param name="slug"></param>
        /// <returns>A public <see cref="Project" /></returns>
        public Task<Project> GetProjectAsync(string slug)
        {
            var project = Set.FirstOrDefaultAsync(p => p.Slug == slug && p.Status == ProjectStatus.Public);

            return project;
        }

        /// <summary>
        /// Asynchronously returns single public project
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="slug"></param>
        /// <returns>A public <see cref="Project" /></returns>
        public Task<Project> GetProjectAsync(CancellationToken cancellationToken, string slug)
        {
            var project = Set.FirstOrDefaultAsync(p => p.Slug == slug && p.Status == ProjectStatus.Public,
                cancellationToken);

            return project;
        }
    }
}