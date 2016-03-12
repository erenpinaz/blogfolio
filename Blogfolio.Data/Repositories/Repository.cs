using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogfolio.Models.Repositories;

namespace Blogfolio.Data.Repositories
{
    /// <summary>
    ///     Entity framework implementation of <see cref="IRepository{TEntity}" />
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly BlogfolioContext _context;
        private DbSet<TEntity> _set;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="context"></param>
        internal Repository(BlogfolioContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Returns entity set
        /// </summary>
        protected DbSet<TEntity> Set => _set ?? (_set = _context.Set<TEntity>());

        /// <summary>
        ///     Returns a list of entity
        /// </summary>
        /// <returns>List of <see cref="TEntity" /></returns>
        public List<TEntity> GetAll()
        {
            return Set.ToList();
        }

        /// <summary>
        ///     Asynchronously returns entities
        /// </summary>
        /// <returns>List of <see cref="TEntity" /></returns>
        public Task<List<TEntity>> GetAllAsync()
        {
            return Set.ToListAsync();
        }

        /// <summary>
        ///     Asynchronously returns entities
        ///     with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>List of <see cref="TEntity" /></returns>
        public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Set.ToListAsync(cancellationToken);
        }

        /// <summary>
        ///     Returns paged entities
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>List of paged <see cref="TEntity" /></returns>
        public List<TEntity> PageAll(int skip, int take)
        {
            return Set.Skip(skip).Take(take).ToList();
        }

        /// <summary>
        ///     Asynchronously returns paged entities
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>List of paged <see cref="TEntity" /></returns>
        public Task<List<TEntity>> PageAllAsync(int skip, int take)
        {
            return Set.Skip(skip).Take(take).ToListAsync();
        }

        /// <summary>
        ///     Asynchronously returns paged entities
        ///     with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>List of paged <see cref="TEntity" /></returns>
        public Task<List<TEntity>> PageAllAsync(CancellationToken cancellationToken, int skip, int take)
        {
            return Set.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        /// <summary>
        ///     Returns entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="TEntity" /></returns>
        public TEntity FindById(object id)
        {
            return Set.Find(id);
        }

        /// <summary>
        ///     Asynchronously returns entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="TEntity" /></returns>
        public Task<TEntity> FindByIdAsync(object id)
        {
            return Set.FindAsync(id);
        }

        /// <summary>
        ///     Asynchronously returns entity
        ///     with cancellation support
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="id"></param>
        /// <returns>A <see cref="TEntity" /></returns>
        public Task<TEntity> FindByIdAsync(CancellationToken cancellationToken, object id)
        {
            return Set.FindAsync(cancellationToken, id);
        }

        /// <summary>
        ///     Creates new entity record
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        /// <summary>
        ///     Updates an entity record
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                Set.Attach(entity);
                entry = _context.Entry(entity);
            }
            entry.State = EntityState.Modified;
        }

        /// <summary>
        ///     Deletes an entity record
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(TEntity entity)
        {
            Set.Remove(entity);
        }
    }
}