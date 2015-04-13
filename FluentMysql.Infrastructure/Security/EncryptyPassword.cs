using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.Security
{
    public static class EncryptyPassword
    {
        public static string Set(object value)
        {
            Encryption encryption = new Encryption();
            encryption.salt = "encriptarsenha";
            encryption.password = "1PvFtfKZ0W";

            return encryption.EncryptString(string.Format("{0}", value));
        }
    }
}
