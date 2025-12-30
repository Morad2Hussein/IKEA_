using System.Linq.Expressions;


namespace Demo.DAL.Repositoties.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        // AddAsync is used for some providers, though usually Add is fine. 
        // We'll use Async to keep the pattern consistent.
        Task AddAsync(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false);

        Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool withTracking = false);

        Task<TEntity?> GetByIdAsync(int id);

        // Update and Remove don't have Async versions in EF because they only mark the state in memory
        void Remove(TEntity entity);
        void Update(TEntity entity);

        IQueryable<TEntity> GetIQueryable();
    }
}