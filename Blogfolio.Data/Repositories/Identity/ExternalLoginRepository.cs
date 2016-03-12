using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Identity;
using Blogfolio.Models.Repositories.Identity;

namespace Blogfolio.Data.Repositories.Identity
{
    /// <summary>
    /// Entity framework implementation of <see cref="IExternalLoginRepository" />
    /// </summary>
    internal class ExternalLoginRepository : Repository<ExternalLogin>, IExternalLoginRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        internal ExternalLoginRepository(BlogfolioContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Returns external login
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>An <see cref="ExternalLogin" /></returns>
        public ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey)
        {
            return Set.FirstOrDefault(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
        }

        /// <summary>
        /// Asynchronously returns external login
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>An <see cref="ExternalLogin" /></returns>
        public Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey)
        {
            return Set.FirstOrDefaultAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
        }

        /// <summary>
        /// Asynchronously returns external login with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>An <see cref="ExternalLogin" /></returns>
        public Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider,
            string providerKey)
        {
            return Set.FirstOrDefaultAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey,
                cancellationToken);
        }
    }
}