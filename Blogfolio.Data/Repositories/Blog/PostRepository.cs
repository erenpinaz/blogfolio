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
    ///     Entity framework implementation of <see cref="IPostRepository" />
    /// </summary>
    internal class PostRepository : Repository<Post>, IPostRepository
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="context"></param>
        internal PostRepository(BlogfolioContext context)
            : base(context)
        {
        }

        /// <summary>
        ///     Returns a list of published posts
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of published <see cref="Post" /></returns>
        public List<Post> GetPosts(int count = 0)
        {
            var posts = Set.Where(p => p.Status == PostStatus.Published)
                .OrderByDescending(p => p.DateCreated);

            return count > 0
                ? posts.Take(count).ToList()
                : posts.ToList();
        }

        /// <summary>
        ///     Asynchronously returns a list of published posts
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of published <see cref="Post" /></returns>
        public Task<List<Post>> GetPostsAsync(int count = 0)
        {
            var posts = Set.Where(p => p.Status == PostStatus.Published)
                .OrderByDescending(p => p.DateCreated);

            return count > 0
                ? posts.Take(count).ToListAsync()
                : posts.ToListAsync();
        }

        /// <summary>
        ///     Asynchronously returns a list of published posts
        ///     with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="count"></param>
        /// <returns>List of published <see cref="Post" /></returns>
        public Task<List<Post>> GetPostsAsync(CancellationToken cancellationToken, int count = 0)
        {
            var posts = Set.Where(p => p.Status == PostStatus.Published)
                .OrderByDescending(p => p.DateCreated);

            return count > 0
                ? posts.Take(count).ToListAsync(cancellationToken)
                : posts.ToListAsync(cancellationToken);
        }

        /// <summary>
        ///     Returns single published post
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <returns>A published <see cref="Post" /></returns>
        public Post GetPost(int year, int month, string slug)
        {
            var post =
                Set.FirstOrDefault(p => p.DateCreated.Year == year && p.DateCreated.Month == month && p.Slug == slug
                                        && p.Status == PostStatus.Published);

            return post;
        }

        /// <summary>
        ///     Asynchronously returns single published post
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <returns>A published <see cref="Post" /></returns>
        public Task<Post> GetPostAsync(int year, int month, string slug)
        {
            var post =
                Set.FirstOrDefaultAsync(
                    p => p.DateCreated.Year == year && p.DateCreated.Month == month && p.Slug == slug
                         && p.Status == PostStatus.Published);

            return post;
        }

        /// <summary>
        ///     Asynchronously returns single published post with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <returns>A published <see cref="Post" /></returns>
        public Task<Post> GetPostAsync(CancellationToken cancellationToken, int year, int month, string slug)
        {
            var post =
                Set.FirstOrDefaultAsync(
                    p => p.DateCreated.Year == year && p.DateCreated.Month == month && p.Slug == slug
                         && p.Status == PostStatus.Published, cancellationToken);

            return post;
        }
    }
}