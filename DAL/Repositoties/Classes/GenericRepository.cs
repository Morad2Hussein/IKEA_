
using System.Linq.Expressions;


namespace Demo.DAL.Repositoties.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            // FindAsync is the async version of Find
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false)
        {
            var query = _dbContext.Set<TEntity>().Where(e => !e.IsDeleted);

            if (!withTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return await _dbContext.Set<TEntity>()
                .Where(e => !e.IsDeleted)
                .Select(selector)
                .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool withTracking = false)
        {
            var query = _dbContext.Set<TEntity>().Where(predicate).Where(e => !e.IsDeleted);

            if (!withTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public IQueryable<TEntity> GetIQueryable()
        {
            return _dbContext.Set<TEntity>().Where(e => !e.IsDeleted);
        }
    }
}