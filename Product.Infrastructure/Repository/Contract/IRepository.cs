using System.Linq.Expressions;

namespace Product.Infrastructure.Repository.Contract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        bool Exists(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        Task<TEntity> FirstOrDefaultNoTracking(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> QueryAll(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
