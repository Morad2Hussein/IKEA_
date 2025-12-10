using Demo.DAL.Repositoties.Classes;
using Demo.DAL.UnitOfWork.Interface;
namespace Demo.DAL.UnitOfWork.Class
{
    public class UnitOfWork : IUnitOfWork
    {
       
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext )
        {
            _dbContext = dbContext;
           
        }

      

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity, new()
        {
           var EntityType = typeof(TEntity);
            if (_repositories.TryGetValue(EntityType, out var Repo)) 
                 return (IGenericRepository<TEntity>)Repo;
            var NewRepo = new GenericRepository<TEntity>(_dbContext);
            return NewRepo;
        }

        public int SaveChanges()
        {
           return _dbContext.SaveChanges();
        }
    }
}
