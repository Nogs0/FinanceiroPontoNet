using System.Linq.Expressions;
using FinanceiroPontoNet.Domain.Shared;
using FinanceiroPontoNet.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroPontoNet.Infrastructure.Persistence.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? predicate = null
        )
        {
            if (predicate == null)
                return await _dbSet.ToListAsync();

            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();

            return await _dbSet.CountAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.AnyAsync();

            return await _dbSet.AnyAsync(predicate);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
