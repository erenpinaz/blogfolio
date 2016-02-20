using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Blog;

namespace Blogfolio.Models.Repositories.Blog
{
    /// <summary>
    /// Repository interface for <see cref="Comment"/>
    /// </summary>
    public interface ICommentRepository : IRepository<Comment>
    {
        /// <summary>
        /// Returns a list of approved comments
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of approved <see cref="Comment" /></returns>
        List<Comment> GetComments(int count = 0);

        /// <summary>
        /// Asynchronously returns a list of approved comments
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of approved <see cref="Comment" /></returns>
        Task<List<Comment>> GetCommentsAsync(int count = 0);

        /// <summary>
        /// Asynchronously returns a list of approved comments
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="count"></param>
        /// <returns>List of approved <see cref="Comment" /></returns>
        Task<List<Comment>> GetCommentsAsync(CancellationToken cancellationToken, int count = 0);
    }
}