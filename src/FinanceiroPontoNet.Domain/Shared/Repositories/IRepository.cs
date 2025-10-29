using System.Linq.Expressions;
using FinanceiroPontoNet.Domain.Shared;

namespace FinanceiroPontoNet.Domain.Shared.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task CreateAsync(TEntity entity);

        Task<TEntity?> GetByIdAsync(Guid id);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
