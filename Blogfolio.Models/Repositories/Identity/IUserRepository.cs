using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Identity;

namespace Blogfolio.Models.Repositories.Identity
{
    /// <summary>
    /// Repository interface for <see cref="User"/>
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Returns user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A <see cref="User" /></returns>
        User FindByUserName(string username);

        /// <summary>
        /// Asynchronously returns user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A <see cref="User" /></returns>
        Task<User> FindByUserNameAsync(string username);

        /// <summary>
        /// Asynchronously returns user 
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="username"></param>
        /// <returns>A <see cref="User" /></returns>
        Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username);
    }
}