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
    /// Entity framework implementation of <see cref="ICategoryRepository" />
    /// </summary>
    internal class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        internal CategoryRepository(BlogfolioContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Returns a list of categories containing published posts
        /// </summary>
        /// <returns> List of <see cref="Category" /> that containing at least one published post</returns>
        public List<Category> GetCategories()
        {
            var categories = Set.Where(c => c.Posts.Any(p => p.Status == PostStatus.Published))
                .OrderByDescending(c => c.Posts.Count);

            return categories
                .ToList();
        }

        /// <summary>
        /// Asynchronously returns a list of categories containing published  posts
        /// </summary>
        /// <returns> List of <see cref="Category" /> that containing at least one published post</returns>
        public Task<List<Category>> GetCategoriesAsync()
        {
            var categories = Set.Where(c => c.Posts.Any(p => p.Status == PostStatus.Published))
                .OrderByDescending(c => c.Posts.Count);

            return categories
                .ToListAsync();
        }

        /// <summary>
        /// Asynchronously returns a list of categories containing published  posts
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>List of <see cref="Category" /> that containing at least one published post</returns>
        public Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            var categories = Set.Where(c => c.Posts.Any(p => p.Status == PostStatus.Published))
                .OrderByDescending(c => c.Posts.Count);

            return categories
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Returns single category containing published posts
        /// </summary>
        /// <param name="slug"></param>
        /// <returns>A <see cref="Category" /> that containing at least one published post</returns>
        public Category GetCategory(string slug)
        {
            var category =
                Set.FirstOrDefault(c => c.Slug == slug && c.Posts.Any(p => p.Status == PostStatus.Published));

            return category;
        }

        /// <summary>
        /// Asynchronously returns single category containing published posts
        /// </summary>
        /// <param name="slug"></param>
        /// <returns>A <see cref="Category" /> that containing at least one published post</returns>
        public Task<Category> GetCategoryAsync(string slug)
        {
            var category =
                Set.FirstOrDefaultAsync(c => c.Slug == slug && c.Posts.Any(p => p.Status == PostStatus.Published));

            return category;
        }

        /// <summary>
        /// Asynchronously Returns single category containing published posts
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="slug"></param>
        /// <returns> A <see cref="Category" /> that containing at least one published post</returns>
        public Task<Category> GetCategoryAsync(CancellationToken cancellationToken, string slug)
        {
            var category =
                Set.FirstOrDefaultAsync(c => c.Slug == slug && c.Posts.Any(p => p.Status == PostStatus.Published),
                    cancellationToken);

            return category;
        }
    }
}