using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Identity;
using Blogfolio.Models.Repositories.Identity;

namespace Blogfolio.Data.Repositories.Identity
{
    /// <summary>
    ///     Entity framework implementation of <see cref="IUserRepository" />
    /// </summary>
    internal class UserRepository : Repository<User>, IUserRepository
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="context"></param>
        internal UserRepository(BlogfolioContext context)
            : base(context)
        {
        }

        /// <summary>
        ///     Returns user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A <see cref="User" /></returns>
        public User FindByUserName(string username)
        {
            return Set.FirstOrDefault(x => x.UserName == username);
        }

        /// <summary>
        ///     Asynchronously returns user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A <see cref="User" /></returns>
        public Task<User> FindByUserNameAsync(string username)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == username);
        }

        /// <summary>
        ///     Asynchronously returns user with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="username"></param>
        /// <returns>A <see cref="User" /></returns>
        public Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }
    }
}