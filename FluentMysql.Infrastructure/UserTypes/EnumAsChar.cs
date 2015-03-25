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
    public class EnumAsChar<T> : IUserType where T : struct
    {
        public static char ConvertToChar(T value)
        {
            return (char)((int)Enum.ToObject(typeof(T), value) + (int)'0');
        }

        public static int ConvertToInt(char value)
        {
            return (int)value - (int)'0';
        }

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
            T value;
            try
            {
                value = (T)Enum.ToObject(typeof(T), (int)obj.ToString()[0] - (int)'0');
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
                var text = (char)((int)Enum.ToObject(typeof(T), value) + (int)'0');
                ((IDataParameter)cmd.Parameters[index]).Value = text.ToString();
            }
        }
        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public Type ReturnedType
        {
            get { return typeof(T); }
        }

        public NHibernate.SqlTypes.SqlType[] SqlTypes
        {
            get { return new[] { NHibernateUtil.Character.SqlType }; }
        }
        #endregion

        bool IUserType.Equals(object x, object y)
        {
            return object.Equals(x, y);
        }
    }
}
