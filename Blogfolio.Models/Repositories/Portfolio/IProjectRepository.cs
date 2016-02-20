using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Portfolio;

namespace Blogfolio.Models.Repositories.Portfolio
{
    /// <summary>
    /// Repository interface for <see cref="Project"/>
    /// </summary>
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>
        /// Returns a list of public projects
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of public <see cref="Project" /></returns>
        List<Project> GetProjects(int count = 0);

        /// <summary>
        /// Asynchronously returns a list of public projects
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of public <see cref="Project" /></returns>
        Task<List<Project>> GetProjectsAsync(int count = 0);

        /// <summary>
        /// Asynchronously returns a list of public projects
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="count"></param>
        /// <returns>List of public <see cref="Project" /></returns>
        Task<List<Project>> GetProjectsAsync(CancellationToken cancellationToken, int count = 0);

        /// <summary>
        /// Returns single public project
        /// </summary>
        /// <param name="slug"></param>
        /// <returns>A public <see cref="Project" /></returns>
        Project GetProject(string slug);

        /// <summary>
        /// Asynchronously returns single public project
        /// </summary>
        /// <param name="slug"></param>
        /// <returns>A public <see cref="Project" /></returns>
        Task<Project> GetProjectAsync(string slug);

        /// <summary>
        /// Asynchronously returns single public project
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="slug"></param>
        /// <returns>A public <see cref="Project" /></returns>
        Task<Project> GetProjectAsync(CancellationToken cancellationToken, string slug);
    }
}