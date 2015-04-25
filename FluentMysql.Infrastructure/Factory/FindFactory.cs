using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.Factory
{
    public static class FindFactory
    {
        public static T Find<T>(object key) where T : class,new()
        {
            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    return (T)session.Get<T>(key);
                }
            }
        }

        public static T Find<T>(ISession session, object key) where T : class,new()
        {
            return (T)session.Get<T>(key);
        }
    }
}
