using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Identity;

namespace Blogfolio.Models.Repositories.Identity
{
    /// <summary>
    ///     Repository interface for <see cref="Role" />
    /// </summary>
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        ///     Returns role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>A <see cref="Role" /></returns>
        Role FindByName(string roleName);

        /// <summary>
        ///     Asynchronously returns role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>A <see cref="Role" /></returns>
        Task<Role> FindByNameAsync(string roleName);

        /// <summary>
        ///     Asynchronously returns role
        ///     with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="roleName"></param>
        /// <returns>A <see cref="Role" /></returns>
        Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName);
    }
}