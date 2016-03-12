using System;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Repositories.Blog;
using Blogfolio.Models.Repositories.Identity;
using Blogfolio.Models.Repositories.Library;
using Blogfolio.Models.Repositories.Portfolio;

namespace Blogfolio.Models
{
    /// <summary>
    ///     Unit of work interface
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Properties

        // Identity
        IExternalLoginRepository ExternalLoginRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }

        // Blog
        IPostRepository PostRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICommentRepository CommentRepository { get; }

        // Portfolio
        IProjectRepository ProjectRepository { get; }

        // Library
        IMediaRepository MediaRepository { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Saves changes that are made in the current context
        /// </summary>
        /// <returns>Number of rows affected as an <see cref="int" /></returns>
        int SaveChanges();

        /// <summary>
        ///     Asynchronously saves changes that are made in the current context
        /// </summary>
        /// <returns>Number of rows affected as an <see cref="int" /></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        ///     Asynchronously saves changes that are made in the current context
        ///     with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Number of rows affected as an <see cref="int" /></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        #endregion
    }
}