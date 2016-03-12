using System;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Data.Repositories.Blog;
using Blogfolio.Data.Repositories.Identity;
using Blogfolio.Data.Repositories.Library;
using Blogfolio.Data.Repositories.Portfolio;
using Blogfolio.Models;
using Blogfolio.Models.Repositories.Blog;
using Blogfolio.Models.Repositories.Identity;
using Blogfolio.Models.Repositories.Library;
using Blogfolio.Models.Repositories.Portfolio;

namespace Blogfolio.Data
{
    /// <summary>
    ///     Entity framework implementation of <see cref="IUnitOfWork" />
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public UnitOfWork(string nameOrConnectionString)
        {
            _context = new BlogfolioContext(nameOrConnectionString);
        }

        #endregion

        #region IDisposable Members

        private bool _disposed = false;

        /// <summary>
        ///     Disposes all the resources used by the <see cref="UnitOfWork" />
        ///     This will be called by DI containers lifetime manager
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Identity
                    _userRepository = null;
                    _roleRepository = null;
                    _externalLoginRepository = null;

                    // Blog
                    _postRepository = null;
                    _categoryRepository = null;
                    _commentRepository = null;

                    // Portfolio
                    _projectRepository = null;

                    // Library
                    _mediaRepository = null;

                    // Database Context
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Fields

        // Database Context
        private readonly BlogfolioContext _context;

        // Identity
        private IExternalLoginRepository _externalLoginRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;

        // Blog
        private IPostRepository _postRepository;
        private ICategoryRepository _categoryRepository;
        private ICommentRepository _commentRepository;

        // Portfolio
        private IProjectRepository _projectRepository;

        // Library
        private IMediaRepository _mediaRepository;

        #endregion

        #region IUnitOfWork Members

        // Identity
        public IExternalLoginRepository ExternalLoginRepository
            => _externalLoginRepository ?? (_externalLoginRepository = new ExternalLoginRepository(_context));

        public IRoleRepository RoleRepository => _roleRepository ?? (_roleRepository = new RoleRepository(_context));
        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_context));

        // Blog
        public IPostRepository PostRepository => _postRepository ?? (_postRepository = new PostRepository(_context));

        public ICategoryRepository CategoryRepository
            => _categoryRepository ?? (_categoryRepository = new CategoryRepository(_context));

        public ICommentRepository CommentRepository
            => _commentRepository ?? (_commentRepository = new CommentRepository(_context));

        // Portfolio
        public IProjectRepository ProjectRepository
            => _projectRepository ?? (_projectRepository = new ProjectRepository(_context));

        // Library
        public IMediaRepository MediaRepository
            => _mediaRepository ?? (_mediaRepository = new MediaRepository(_context));

        /// <summary>
        ///     Saves changes that are made in the current context
        /// </summary>
        /// <returns>Number of rows affected as an <see cref="int" /></returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        ///     Asynchronously saves changes that are made in the current context
        /// </summary>
        /// <returns>Number of rows affected as an <see cref="int" /></returns>
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        /// <summary>
        ///     Asynchronously saves changes that are made in the current context
        ///     with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Number of rows affected as an <see cref="int" /></returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}