using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Blog;
using Blogfolio.Models.Repositories.Blog;

namespace Blogfolio.Data.Repositories.Blog
{
    /// <summary>
    ///     Entity framework implementation of <see cref="ICommentRepository" />
    /// </summary>
    internal class CommentRepository : Repository<Comment>, ICommentRepository
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="context"></param>
        internal CommentRepository(BlogfolioContext context)
            : base(context)
        {
        }

        /// <summary>
        ///     Returns a list of approved comments
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of approved <see cref="Comment" /></returns>
        public List<Comment> GetComments(int count = 0)
        {
            var comments = Set.Where(c => c.Status == CommentStatus.Approved)
                .OrderByDescending(c => c.DateCreated);

            return count > 0
                ? comments.Take(count).ToList()
                : comments.ToList();
        }

        /// <summary>
        ///     Asynchronously returns a list of approved comments
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of approved <see cref="Comment" /></returns>
        public Task<List<Comment>> GetCommentsAsync(int count = 0)
        {
            var comments = Set.Where(c => c.Status == CommentStatus.Approved)
                .OrderByDescending(c => c.DateCreated);

            return count > 0
                ? comments.Take(count).ToListAsync()
                : comments.ToListAsync();
        }

        /// <summary>
        ///     Asynchronously returns a list of approved comments
        ///     with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="count"></param>
        /// <returns>List of approved <see cref="Comment" /></returns>
        public Task<List<Comment>> GetCommentsAsync(CancellationToken cancellationToken, int count = 0)
        {
            var comments = Set.Where(c => c.Status == CommentStatus.Approved)
                .OrderByDescending(c => c.DateCreated);

            return count > 0
                ? comments.Take(count).ToListAsync(cancellationToken)
                : comments.ToListAsync(cancellationToken);
        }
    }
}