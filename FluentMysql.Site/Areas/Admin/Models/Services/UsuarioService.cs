using AutoMapper;
using FluentMysql.Domain.Repository;
using FluentMysql.Infrastructure;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.ViewsData.Usuario;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.Models.Services
{
    public static class UsuarioService
    {
        internal static void Ativar(IList<long> id, Usuario responsavel)
        {
            if (object.Equals(id, null) || id.Count.Equals(0))
                throw new ArgumentNullException("Valor não pode ser nulo ou vazio", "id");

            if (object.Equals(responsavel, null))
                throw new ArgumentNullException("Valor não pode ser nulo ou vazio", "responsavel");

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    session.CreateQuery(@"UPDATE Usuario SET Status = :status WHERE Id IN (:id)")
                        .SetInt32("status", (int)Status.Ativo)
                        .SetParameterList("id", id)
                        .ExecuteUpdate();
                }
            }
        }

        internal static void Desativar(IList<long> id, Usuario responsavel)
        {
            if (object.Equals(id, null) || id.Count.Equals(0))
                throw new ArgumentNullException("Valor não pode ser nulo ou vazio", "id");

            if (object.Equals(responsavel, null))
                throw new ArgumentNullException("Valor não pode ser nulo", "responsavel");

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    session.CreateQuery(@"UPDATE Usuario SET Status = :status WHERE Id IN (:id)")
                        .SetInt32("status", (int)Status.Inativo)
                        .SetParameterList("id", id)
                        .ExecuteUpdate();
                }
            }
        }

        internal static void Excluir(IList<long> id, Usuario responsavel)
        {
            if (object.Equals(id, null) || id.Count.Equals(0))
                throw new ArgumentNullException("Valor não pode ser nulo ou vazio", "id");

            if (object.Equals(responsavel, null))
                throw new ArgumentNullException("Valor não pode ser nulo", "responsavel");

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    session.CreateQuery(@"UPDATE Usuario SET Status = :status WHERE Id IN (:id)")
                        .SetInt32("status", (int)Status.Excluido)
                        .SetParameterList("id", id)
                        .ExecuteUpdate();
                }
            }
        }

        internal static IList<Usuario> Filtrar()
        {
            return Filtrar(new FiltroForm());
        }

        internal static IList<Usuario> Filtrar(FiltroForm filtro)
        {
            if (object.Equals(filtro, null))
                throw new ArgumentNullException("Valor não pode ser nulo", "filtro");

            IList<Usuario> resultado = new List<Usuario>();
            
            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    resultado = session.CreateQuery(@"FROM Usuario WHERE Status IN (:status) AND (Email LIKE :texto OR concat(Nome, ' ', Sobrenome) LIKE :texto) ORDER BY Id Desc")
                        .SetParameterList("status", new List<Status>() { Status.Ativo, Status.Inativo })
                        .SetString("texto", "%" + filtro.PalavraChave + "%")
                        .SetMaxResults(1000)
                        .List<Usuario>();
                }
            }

            return resultado;
        }

        internal static Usuario Alterar(AlteraForm dados, Usuario responsavel)
        {
            if (object.Equals(dados, null))
                throw new ArgumentNullException("Valor não pode ser nulo", "dados");

            if (object.Equals(responsavel, null))
                throw new ArgumentNullException("Valor não pode ser nulo", "responsavel");

            Usuario resultado = Mapper.Map<AlteraForm, Usuario>(dados);

            resultado.Responsavel = responsavel;
            resultado.DataAlteracao = DateTime.Now;
            using (UsuarioRepository acao = new UsuarioRepository())
            {
                acao.Edit(resultado);
            }

            return resultado;
        }

        internal static Usuario Info(long id)
        {
            if (id <= 0)
                throw new ArgumentException("Valor não pode ser menor ou igual a 0", "id");

            Usuario resultado = null;

            using (UsuarioRepository acao = new UsuarioRepository())
            {
                resultado = acao.Find(id);
            }

            return resultado;
        }
        
        internal static IList<Usuario> Info(IList<long> ids)
        {
            if (object.Equals(ids, null))
                throw new ArgumentNullException("Valor não pode ser nulo", "ids");

            if (ids.Where(x => x <= 0).Count() > 0)
                throw new ArgumentException("Valor não pode ser menor ou igual a 0", "ids");

            IList<Usuario> resultado = new List<Usuario>();

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    resultado =session.CreateQuery(@"FROM Usuario WHERE Id IN (:id)")
                        .SetParameterList("id", ids)
                        .List<Usuario>();
                }
            }

            return resultado;
        }
        
        internal static Usuario Info(string identificacao)
        {
            if (string.IsNullOrWhiteSpace(identificacao))
                throw new ArgumentNullException("Valor não pode ser nulo ou vazio", "identificacao");
            
            Usuario resultado = null;

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    resultado = session.CreateQuery(@"FROM Usuario WHERE Login = :id OR Email = :id OR CPF = :id")
                        .SetString("id", identificacao)
                        .SetMaxResults(1)
                        .List<Usuario>()
                        .FirstOrDefault();
                }
            }

            return resultado;
        }

        internal static Usuario Inserir(InsereForm dados, Usuario responsavel)
        {
            if (object.Equals(dados, null))
                throw new ArgumentNullException("Valor não pode ser nulo", "dados");

            if (object.Equals(responsavel, null))
                throw new ArgumentNullException("Valor não pode ser nulo", "responsavel");

            Usuario resultado = Mapper.Map<InsereForm, Usuario>(dados);

            resultado.Responsavel = responsavel;
            using (UsuarioRepository acao = new UsuarioRepository())
            {
                acao.Add(resultado);
            }

            return resultado;
        }
    }
}
