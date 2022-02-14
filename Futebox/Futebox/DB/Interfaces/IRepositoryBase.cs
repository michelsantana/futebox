using Futebox.DB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Futebox.Interfaces.DB
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : BaseEntity
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(string id);
        void Insert(ref TEntity entity);
        bool Update(TEntity entity);
        bool Delete(string id);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);
        void OpenTransaction();
        void Commit();
        void Rollback();
    }
}
