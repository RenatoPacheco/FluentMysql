using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.Factory;
using FluentMysql.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Domain.ValueObject
{
    public class TokenAcesso
    {
        private static Encryption GetEncryption()
        {
            Encryption resultado = new Encryption();
            resultado.salt = "0PImk95yAl";
            resultado.password = "7RgsX0kJQI";
            return resultado;
        }

        private string _Token { get; set; }

        private Usuario _Usuario { get; set; }

        private TokenAcesso(Usuario usuario)
        {
            this._Usuario = FindFactory.Find<Usuario>(usuario.Id);
            this._Token = GetEncryption().EncryptString(string.Format("{0}|{1}", usuario.Id, DateTime.Now));
        }

        private TokenAcesso(string token)
        {
            this._Token = token;
            this._Usuario = FindFactory.Find<Usuario>(long.Parse(GetEncryption().DecryptString(token).Split('|')[0]));
        }

        public static TokenAcesso Parse(string value)
        {
            TokenAcesso resultado = null;
            try
            {
                resultado = new TokenAcesso(value);
            }
            catch
            {
                throw new Exception(string.Format("Valor de {0} é inválido para extarir o token", typeof(string)));
            }

            return resultado;
        }

        public static bool TryParse(string value, out TokenAcesso result)
        {
            try
            {
                result = Parse(value);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static TokenAcesso Parse(Usuario value)
        {
            TokenAcesso resultado = null;
            try
            {
                resultado = new TokenAcesso(value);
            }
            catch
            {
                throw new Exception(string.Format("Valor de {0} é inválido para extarir o token", typeof(Usuario)));
            }

            return resultado;
        }

        public static bool TryParse(Usuario value, out TokenAcesso result)
        {
            try
            {
                result = Parse(value);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static explicit operator TokenAcesso(string value)
        {
            TokenAcesso resultado = null;
            TryParse(value, out resultado);
            return resultado;
        }

        public static explicit operator TokenAcesso(Usuario value)
        {
            TokenAcesso resultado = null;
            TryParse(value, out resultado);
            return resultado;
        }

        public static explicit operator string(TokenAcesso value)
        {
            return object.Equals(value, null) ? null : value._Token;
        }

        public static explicit operator Usuario(TokenAcesso value)
        {
            return object.Equals(value, null) ? null : value._Usuario;
        }

        public override string ToString()
        {
            return _Token;
        }
    }
}
