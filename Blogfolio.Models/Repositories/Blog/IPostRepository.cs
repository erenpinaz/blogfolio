using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Blog;

namespace Blogfolio.Models.Repositories.Blog
{
    /// <summary>
    ///     Repository interface for <see cref="Post" />
    /// </summary>
    public interface IPostRepository : IRepository<Post>
    {
        /// <summary>
        ///     Returns a list of published posts
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of published <see cref="Post" /></returns>
        List<Post> GetPosts(int count = 0);

        /// <summary>
        ///     Asynchronously returns a list of published posts
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of published <see cref="Post" /></returns>
        Task<List<Post>> GetPostsAsync(int count = 0);

        /// <summary>
        ///     Asynchronously returns a list of published posts
        ///     with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="count"></param>
        /// <returns>List of published <see cref="Post" /></returns>
        Task<List<Post>> GetPostsAsync(CancellationToken cancellationToken, int count = 0);

        /// <summary>
        ///     Returns single published post
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <returns>A published <see cref="Post" /></returns>
        Post GetPost(int year, int month, string slug);

        /// <summary>
        ///     Asynchronously returns single published post
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <returns>A published <see cref="Post" /></returns>
        Task<Post> GetPostAsync(int year, int month, string slug);

        /// <summary>
        ///     Asynchronously returns single published post with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <returns>A published <see cref="Post" /></returns>
        Task<Post> GetPostAsync(CancellationToken cancellationToken, int year, int month, string slug);
    }
}