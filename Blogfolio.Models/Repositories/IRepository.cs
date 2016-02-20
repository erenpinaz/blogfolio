using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blogfolio.Models.Repositories
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Returns a list of entity
        /// </summary>
        /// <returns>List of <see cref="TEntity" /></returns>
        List<TEntity> GetAll();

        /// <summary>
        /// Asynchronously returns entities
        /// </summary>
        /// <returns>List of <see cref="TEntity" /></returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// Asynchronously returns entities
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>List of <see cref="TEntity" /></returns>
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns paged entities
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>List of paged <see cref="TEntity" /></returns>
        List<TEntity> PageAll(int skip, int take);

        /// <summary>
        /// Asynchronously returns paged entities
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>List of paged <see cref="TEntity" /></returns>
        Task<List<TEntity>> PageAllAsync(int skip, int take);

        /// <summary>
        /// Asynchronously returns paged entities
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>List of paged <see cref="TEntity" /></returns>
        Task<List<TEntity>> PageAllAsync(CancellationToken cancellationToken, int skip, int take);

        /// <summary>
        /// Returns entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="TEntity" /></returns>
        TEntity FindById(object id);

        /// <summary>
        /// Asynchronously returns entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="TEntity" /></returns>
        Task<TEntity> FindByIdAsync(object id);

        /// <summary>
        /// Asynchronously returns entity
        /// with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="id"></param>
        /// <returns>A <see cref="TEntity" /></returns>
        Task<TEntity> FindByIdAsync(CancellationToken cancellationToken, object id);

        /// <summary>
        /// Creates new entity record
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Updates an entity record
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Deletes an entity record
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);
    }
}