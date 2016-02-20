using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Identity;

namespace Blogfolio.Models.Repositories.Identity
{
    /// <summary>
    /// Repository interface for <see cref="ExternalLogin"/>
    /// </summary>
    public interface IExternalLoginRepository : IRepository<ExternalLogin>
    {
        /// <summary>
        /// Returns external login
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>An <see cref="ExternalLogin" /></returns>
        ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey);

        /// <summary>
        /// Asynchronously returns external login
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>An <see cref="ExternalLogin" /></returns>
        Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey);

        /// <summary>
        /// Asynchronously returns external login
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>An <see cref="ExternalLogin" /></returns>
        Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider,
            string providerKey);
    }
}