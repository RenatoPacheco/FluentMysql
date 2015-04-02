using FluentMysql.Infrastructure;
using FluentMysql.Infrastructure.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Domain.Services
{
    public static class AutenticacaoService
    {
        public static Usuario Logar(string login, string senha)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentException("Valor não pode ser nulo ou vazio", "login");
            
            if (string.IsNullOrEmpty(senha))
                throw new ArgumentException("Valor não pode ser nulo ou vazio", "senha");

            Usuario resultado = null;

            using(Connection connection = new Connection())
            {
                using(ISession session =connection.Session)
                {
                    resultado = session.QueryOver<Usuario>()
                        .Where(x => (x.Login == login || x.Email == login || x.CPF == login) && x.Senha == senha)
                        .Take(1)
                        .List<Usuario>()
                        .FirstOrDefault();
                }
            }

            return resultado;
        }

        public static Usuario Recuperar(string login)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentException("Valor não pode ser nulo ou vazio", "login");

            Usuario resultado = null;

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    resultado = session.QueryOver<Usuario>()
                        .Where(x => x.Login == login || x.Email == login)
                        .Take(1)
                        .List<Usuario>()
                        .FirstOrDefault();
                }
            }

            return resultado;
        }
    }
}
