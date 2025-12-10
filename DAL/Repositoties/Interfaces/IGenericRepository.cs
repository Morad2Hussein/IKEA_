

using Demo.DAL.Models.Common;
using System.Linq.Expressions;

namespace Demo.DAL.Repositoties.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
  
        void Add(TEntity entity);
        IEnumerable<TEntity> GetAll(bool withTracking = false);
        IEnumerable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector);
        IEnumerable<TEntity> GetALL(Expression<Func<TEntity,bool>> predicate, bool withTracking = false);
        TEntity? GetById(int id);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        IEnumerable<TEntity> GetIEnumerable();
        IQueryable<TEntity> GetIQueryable();


    }
}
