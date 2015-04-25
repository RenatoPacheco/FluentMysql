using FluentMysql.Infrastructure.Security;
using FluentMysql.Infrastructure.ValueObject;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FluentMysql.Infrastructure.UserTypes
{
    public class StringAsPassword : IUserType
    {
        #region IUserType Members

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public int GetHashCode(object x)
        {
            if (x == null)
                return 0;
            return x.GetHashCode();
        }
        public bool IsMutable
        {
            get { return false; }
        }

        // represents conversion on load-from-db operations:
        public object NullSafeGet(System.Data.IDataReader rs, string[] names, object owner)
        {
            var obj = NHibernateUtil.String.NullSafeGet(rs, names[0]);
            if (obj == null)
                return null;
            string value;
            try
            {
                value = string.Format("{0}", obj.ToString());
            }
            catch (Exception)
            {
                return null;
            }
            return value;
        }

        // represents conversion on save-to-db operations:
        public void NullSafeSet(System.Data.IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                string text = EncryptyPassword.Set(value);
                ((IDataParameter)cmd.Parameters[index]).Value = text.ToString();
            }
        }
        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public Type ReturnedType
        {
            get { return typeof(string); }
        }

        public NHibernate.SqlTypes.SqlType[] SqlTypes
        {
            get { return new[] { NHibernateUtil.String.SqlType }; }
        }
        #endregion

        bool IUserType.Equals(object x, object y)
        {
            return object.Equals(x, y);
        }
    }
}
