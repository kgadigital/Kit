using KitAplication.Data;
using KitAplication.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KitAplication.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly SqlContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(SqlContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        /// <summary>
        /// Retrieves an entity by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result is the entity with the specified ID</returns>
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        /// <summary>
        /// Retrieves all entities from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result is a collection of entities.</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        /// <summary>
        ///  Retrieves entities from the database that match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a collection of entities.</returns>
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        /// <summary>
        /// Adds the specified entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task representing the asynchronous operation. The task result is the added entity.</returns>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Updates the specified entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the specified entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
