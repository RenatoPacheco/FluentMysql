using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.Security
{
    public class Token
    {
        public Encryption Encryption { get; set; }

        Token()
        {
            Encryption = new Encryption();
            Encryption.salt = "FluentMysql";
            Encryption.password = "4bBhTgFr";
        }

        public static string EncryptString(string value)
        {
            return EncryptString(value, DateTime.Now.AddYears(100));
        }
        public static string EncryptString(string value, DateTime timeout)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("O valor não pode ser vazio ou nulo", "value");
            
            if (object.Equals(timeout, null))
                throw new ArgumentException("O valor não pode ser nulo", "timeout");

            Token tk = new Token();
            return tk.Encryption.EncryptString(string.Format("{0}-{1}-{2}|{3}", timeout.Year, timeout.Month, timeout.Day, value));
        }

        public static string DecryptString(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("O valor não pode ser vazio ou nulo", "value");

            Token tk = new Token();
            string valor = tk.Encryption.DecryptString(value);
            DateTime data = DateTime.Parse(Regex.Replace(valor, @"^([^\|]+)(\|)(.*)$", "$1"));
            string resultado = Regex.Replace(valor, @"^([^\|]+)(\|)(.*)$", "$3");

            if (DateTime.Compare(DateTime.Parse(DateTime.Now.ToShortDateString()), data) > 0)
            {
                resultado = string.Empty;
            }
            return resultado;
        }
    }
}
