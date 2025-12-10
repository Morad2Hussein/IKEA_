

namespace Demo.DAL.UnitOfWork.Interface
{
    public interface IUnitOfWork
    {


       
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity, new();
        int  SaveChanges();
    }
}
