using FluentMysql.Domain.Repository;
using FluentMysql.Infrastructure;
using FluentMysql.Infrastructure.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Domain.Services
{
    public static class UsuarioService
    {
        public static Usuario ChecarEmailExistente(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("email", "O valor não pode ser nulo ou vazio");

            Usuario resultado = null;

            using(UsuarioRepository acao = new UsuarioRepository())
            {
                resultado = acao.Query().Where(x => x.Email == email).FirstOrDefault();
            }

            return resultado;
        }
        public static Usuario ChecarEmailExistente(string email, Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("email", "O valor não pode ser nulo ou vazio");

            if (object.Equals(usuario, null) || usuario.Id <= 0)
                throw new ArgumentNullException("usuario", "O valor não pode ser nulo ou vazio");

            Usuario resultado = null;
            
            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    resultado = session.QueryOver<Usuario>()
                        .Where(x => x.Email == email && x.Id != usuario.Id)
                        .Take(1)
                        .List<Usuario>()
                        .FirstOrDefault();
                }
            }

            return resultado;
        }
        
        public static Usuario ChecarLoginExistente(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException("login", "O valor não pode ser nulo ou vazio");

            Usuario resultado = null;

            using (UsuarioRepository acao = new UsuarioRepository())
            {
                resultado = acao.Query().Where(x => x.Login == login).FirstOrDefault();
            }

            return resultado;
        }
        public static Usuario ChecarLoginExistente(string login, Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException("login", "O valor não pode ser nulo ou vazio");

            if (object.Equals(usuario, null) || usuario.Id <= 0)
                throw new ArgumentNullException("usuario", "O valor não pode ser nulo ou vazio");

            Usuario resultado = null;

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    resultado = session.QueryOver<Usuario>()
                        .Where(x => x.Login == login && x.Id != usuario.Id)
                        .Take(1)
                        .List<Usuario>()
                        .FirstOrDefault();
                }
            }

            return resultado;
        }
        
        public static Usuario ChecarCPFExistente(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentNullException("cpf", "O valor não pode ser nulo ou vazio");

            Usuario resultado = null;

            using (UsuarioRepository acao = new UsuarioRepository())
            {
                resultado = acao.Query().Where(x => x.CPF == cpf).FirstOrDefault();
            }

            return resultado;
        }
        public static Usuario ChecarCPFExistente(string cpf, Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentNullException("cpf", "O valor não pode ser nulo ou vazio");

            if (object.Equals(usuario, null) || usuario.Id <= 0)
                throw new ArgumentNullException("usuario", "O valor não pode ser nulo ou vazio");

            Usuario resultado = null;

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    resultado = session.QueryOver<Usuario>()
                        .Where(x => x.CPF == cpf && x.Id != usuario.Id)
                        .Take(1)
                        .List<Usuario>()
                        .FirstOrDefault();
                }
            }

            return resultado;
        }
        
        public static Usuario InserirUnico(Usuario usuario)
        {
            if (object.Equals(usuario, null))
                throw new ArgumentNullException("usuario", "O valor não pode ser nulo ou vazio");

            if (!string.IsNullOrWhiteSpace(usuario.Login) && ChecarLoginExistente(usuario.Login) != null)
                throw new ValidationException("Já existe um registro com esse login");

            if (!string.IsNullOrWhiteSpace(usuario.Email) && ChecarEmailExistente(usuario.Email) != null)
                throw new ValidationException("Já existe um registro com esse e-mail");

            if (!string.IsNullOrWhiteSpace(usuario.CPF) && ChecarEmailExistente(usuario.CPF) != null)
                throw new ValidationException("Já existe um registro com esse CPF");
            
            using(UsuarioRepository acao = new UsuarioRepository())
            {
                acao.Add(usuario);
            }

            return usuario;
        }

        public static Usuario AlterarUnico(Usuario usuario)
        {
            if (object.Equals(usuario, null) && usuario.Id >= 0)
                throw new ArgumentNullException("usuario", "O valor não pode ser nulo ou vazio");

            if (!string.IsNullOrWhiteSpace(usuario.Login) && ChecarLoginExistente(usuario.Login, usuario) != null)
                throw new ValidationException("Já existe um registro com esse login");

            if (!string.IsNullOrWhiteSpace(usuario.Email) && ChecarEmailExistente(usuario.Email, usuario) != null)
                throw new ValidationException("Já existe um registro com esse e-mail");

            if (!string.IsNullOrWhiteSpace(usuario.CPF) && ChecarEmailExistente(usuario.CPF, usuario) != null)
                throw new ValidationException("Já existe um registro com esse CPF");

            using (UsuarioRepository acao = new UsuarioRepository())
            {
                acao.Edit(usuario);
            }

            return usuario;
        }
    }
}
