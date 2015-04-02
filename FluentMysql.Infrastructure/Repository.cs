using FluentMysql.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FluentMysql.Infrastructure
{
    public abstract class Repository<T> : IRepository<T> where T : class, new()
    {
        private IConnection _Connection;
        public IConnection Connection
        {
            get { return this._Connection; }
            private set { this._Connection = value; }
        }
        public Repository()
        {
            this._Connection = new Connection();
            this.Connection.Open();
        }
        public Repository(IConnection Connection)
        {
            this._Connection = Connection;
            this.Connection.Open();
        }

        public void Add(T model)
        {
            this.Add(new List<T>() { model });
        }

        public void Add(IList<T> model)
        {
            this._Connection.Session.Transaction.Begin();
            foreach (T item in model)
            {
                this._Connection.Session.Save(item);
            }
            this._Connection.Session.Transaction.Commit();
        }

        public void Edit(T model)
        {
            this.Edit(new List<T>() { model });
        }

        public void Edit(IList<T> model)
        {
            this._Connection.Session.Transaction.Begin();
            foreach (T item in model)
            {
                this._Connection.Session.Update(item);
            }
            this._Connection.Session.Transaction.Commit();
        }

        public void AddOrEdit(T model)
        {
            this.AddOrEdit(new List<T>() { model });
        }

        public void AddOrEdit(IList<T> model)
        {
            this._Connection.Session.Transaction.Begin();
            foreach (T item in model)
            {
                this._Connection.Session.SaveOrUpdate(item);
            }
            this._Connection.Session.Transaction.Commit();
        }

        public void Remove(T model)
        {
            this.Remove(new List<T>() { model });
        }

        public void Remove(IList<T> model)
        {
            this._Connection.Session.Transaction.Begin();
            foreach (T item in model)
            {
                this._Connection.Session.Delete(item);
            }
            this._Connection.Session.Transaction.Commit();
        }

        public T Find(object key)
        {
            return (T)this._Connection.Session.Get<T>(key);
        }

        public IQueryable<T> Query()
        {
            try
            {
                return this._Connection.Session.CreateCriteria<T>().List<T>().AsQueryable();
            }
            catch (NHibernate.ADOException ex)
            {
                var er = ex.Data;
            }
            return null;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return this._Connection.Session.CreateCriteria<T>().List<T>().AsQueryable<T>().Where<T>(where);
        }

        public void Dispose()
        {
            this.Connection.Close();
            GC.SuppressFinalize(this);
        }
    }
}
