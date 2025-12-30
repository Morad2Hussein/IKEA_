using System;
using System.Threading.Tasks;


namespace Demo.DAL.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity, new();

        // Changed to Task<int> for async saving
        Task<int> SaveChangesAsync();
    }
}