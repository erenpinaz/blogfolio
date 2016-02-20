using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Blog;

namespace Blogfolio.Models.Repositories.Blog
{
    /// <summary>
    /// Repository interface for <see cref="Category"/>
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Returns a list of categories containing published posts
        /// </summary>
        /// <returns> List of <see cref="Category" /> that containing at least one published post</returns>
        List<Category> GetCategories();

        /// <summary>
        /// Asynchronously returns a list of categories containing published posts
        /// </summary>
        /// <returns> List of <see cref="Category" /> that containing at least one published post</returns>
        Task<List<Category>> GetCategoriesAsync();

        /// <summary>
        /// Asynchronously returns a list of categories containing published posts
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>List of <see cref="Category" /> that containing at least one published post</returns>
        Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns single category containing published posts
        /// </summary>
        /// <param name="slug"></param>
        /// <returns>A <see cref="Category" /> that containing at least one published post</returns>
        Category GetCategory(string slug);

        /// <summary>
        /// Asynchronously returns single category containing published posts
        /// </summary>
        /// <param name="slug"></param>
        /// <returns>A <see cref="Category" /> that containing at least one published post</returns>
        Task<Category> GetCategoryAsync(string slug);

        /// <summary>
        /// Asynchronously Returns single category containing published posts
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="slug"></param>
        /// <returns> A <see cref="Category" /> that containing at least one published post</returns>
        Task<Category> GetCategoryAsync(CancellationToken cancellationToken, string slug);
    }
}