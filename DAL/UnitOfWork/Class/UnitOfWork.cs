using Demo.DAL.Repositoties.Classes;
using Demo.DAL.Repositoties.Interfaces;
using Demo.DAL.UnitOfWork.Interface;


namespace Demo.DAL.UnitOfWork.Class
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity, new()
        {
            var entityType = typeof(TEntity);

            if (!_repositories.ContainsKey(entityType))
            {
                // Create the repository and add it to the dictionary for reuse
                var newRepo = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(entityType, newRepo);
            }

            return (IGenericRepository<TEntity>)_repositories[entityType];
        }

        // Async Implementation
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}