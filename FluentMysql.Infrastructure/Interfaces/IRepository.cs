using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FluentMysql.Infrastructure.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class,new()
    {
        IConnection Connection { get; }

        void Add(T model);
        void Add(IList<T> model);

        void Edit(T model);
        void Edit(IList<T> model);

        void AddOrEdit(T model);
        void AddOrEdit(IList<T> model);

        void Remove(T model);
        void Remove(IList<T> model);

        T Find(object key);
        IQueryable<T> Query();
        IQueryable<T> Query(Expression<Func<T, bool>> where);
    }
}
