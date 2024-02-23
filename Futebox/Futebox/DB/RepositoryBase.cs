using Dapper;
using Dommel;
using Futebox.DB.Interfaces;
using Futebox.Interfaces.DB;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Futebox.DB
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : BaseEntity
    {
        protected readonly IDatabaseConfig _databaseConfig;
        protected readonly SqliteConnection _dbContext;
        protected SqliteTransaction _transaction = null;

        public RepositoryBase(IDatabaseConfig databaseConfig)
        {
            this._databaseConfig = databaseConfig;
            this._dbContext = new SqliteConnection(_databaseConfig.ConnectionString());
            this._transaction = null;
            this._dbContext.Open();
        }

        ~RepositoryBase()
        {
            Rollback();
            _dbContext.Close();
        }

        public virtual string GenerateID()
        {
            return Guid.NewGuid().ToString().Substring(0, 5);
        }

        public virtual void OpenTransaction()
        {
            Rollback();
            _transaction = this._dbContext.BeginTransaction();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbContext.GetAll<TEntity>();
        }

        public virtual TEntity GetById(string id)
        {
            return _dbContext.Select<TEntity>(_ => _.id == id).FirstOrDefault();
        }

        public virtual void Insert(ref TEntity entity)
        {
            try
            {
                entity.id = GenerateID();
                entity.criacao = DateTime.Now;
                _dbContext.Insert(entity, _transaction);
                entity = GetById(entity.id);
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
                throw ex;
            }
        }

        public virtual void InsertWithId(string id, ref TEntity entity)
        {
            try
            {
                entity.id = id;
                entity.criacao = DateTime.Now;
                _dbContext.Insert(entity, _transaction);
                entity = GetById(entity.id);
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
                throw ex;
            }
        }

        public virtual bool Update(TEntity entity)
        {
            try
            {
                entity.alteracao = DateTime.Now;
                _dbContext.DeleteMultiple<TEntity>(_ => _.id == entity.id, _transaction);
                _dbContext.Insert(entity, _transaction);
                return true;
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
                throw ex;
            }
        }

        public virtual TEntity UpdateReturn(TEntity entity)
        {
            try
            {
                entity.alteracao = DateTime.Now;
                _dbContext.DeleteMultiple<TEntity>(_ => _.id == entity.id, _transaction);
                _dbContext.Insert(entity, _transaction);
                return entity;
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
                throw ex;
            }
        }

        public virtual bool Delete(string id)
        {
            var entity = GetById(id);

            if (entity == null) throw new Exception("Registro não encontrado");

            var rows = _dbContext.DeleteMultiple<TEntity>(_ => _.id == entity.id, _transaction);
            return rows > 0;
        }

        public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _dbContext.Select(predicate);
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
                throw ex;
            }
        }

        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _dbContext.Select(predicate)?.First();
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
                return null;
            }
        }

        public virtual void Commit()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }

        public virtual void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            Rollback();
            _dbContext.Close();
        }
    }
}

