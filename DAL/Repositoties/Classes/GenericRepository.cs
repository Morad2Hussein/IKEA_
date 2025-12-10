

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

        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public IEnumerable<TEntity> GetAll(bool withTracking = false) {
            if (withTracking)
                return _dbContext.Set<TEntity>().Where(entity => entity.IsDeleted == false).ToList();
            else
                return _dbContext.Set<TEntity>().Where(entity => entity.IsDeleted == false).AsNoTracking().ToList();
        }
        public TEntity? GetById(int id)
            => _dbContext.Set<TEntity>().Find(id);

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
       
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return _dbContext.Set<TEntity>().Where(entity => entity.IsDeleted == false).Select(selector).ToList();
        }

        public IEnumerable<TEntity> GetALL(Expression<Func<TEntity, bool>> predicate, bool withTracking = false)
        {
            var query = _dbContext.Set<TEntity>().Where(predicate).Where(e => e.IsDeleted == false);

            return withTracking ? query.ToList() : query.AsNoTracking().ToList();
        }


        public IEnumerable<TEntity> GetIEnumerable()
        {
            return _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetIQueryable()
        {
            return _dbContext.Set<TEntity>();
        }
    }
}
