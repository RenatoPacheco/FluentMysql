using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.Factory
{
    public static class QueryFactory
    {
        public static IQueryable<T> Query<T>() where T : class,new()
        {
            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    return session.CreateCriteria<T>().List<T>().AsQueryable();
                }
            }
        }

        public static IQueryable<T> Query<T>(ISession session) where T : class,new()
        {
            return session.CreateCriteria<T>().List<T>().AsQueryable();
        }

        public static IQueryable<T> Query<T>(ISession session, Expression<Func<T, bool>> where) where T : class,new()
        {
            return session.CreateCriteria<T>().List<T>().AsQueryable<T>().Where<T>(where);
        }

        public static IQueryable<T> Query<T>(Expression<Func<T, bool>> where) where T : class,new()
        {
            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    return session.CreateCriteria<T>().List<T>().AsQueryable<T>().Where<T>(where);
                }
            }
        }
    }
}
