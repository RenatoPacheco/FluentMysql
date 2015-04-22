using AutoMapper;
using FluentMysql.Domain.Repository;
using FluentMysql.Infrastructure;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.ViewsData.Artigo;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.Services
{
    public static class ArtigoService
    {
        internal static void Ativar(IList<long> id, Usuario usuario)
        {
            if (object.Equals(id, null) || id.Count.Equals(0))
                throw new ArgumentNullException("id", "Valor não pode ser nulo ou vazio");

            if (object.Equals(usuario, null))
                throw new ArgumentNullException("usuario", "Valor não pode ser nulo ou vazio");

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    session.CreateQuery(@"UPDATE Artigo SET Status = :status WHERE Id IN (:id)")
                        .SetInt32("status", (int)Status.Ativo)
                        .SetParameterList("id", id)
                        .ExecuteUpdate();
                }
            }
        }

        internal static void Desativar(IList<long> id, Usuario usuario)
        {
            if (object.Equals(id, null) || id.Count.Equals(0))
                throw new ArgumentException("Valor não pode ser nulo ou vazio", "id");

            if (object.Equals(usuario, null))
                throw new ArgumentException("Valor não pode ser nulo ou vazio", "usuario");

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    session.CreateQuery(@"UPDATE Artigo SET Status = :status WHERE Id IN (:id)")
                        .SetInt32("status", (int)Status.Inativo)
                        .SetParameterList("id", id)
                        .ExecuteUpdate();
                }
            }
        }

        internal static void Excluir(IList<long> id, Usuario usuario)
        {
            if (object.Equals(id, null) || id.Count.Equals(0))
                throw new ArgumentException("Valor não pode ser nulo ou vazio", "id");

            if (object.Equals(usuario, null))
                throw new ArgumentException("Valor não pode ser nulo ou vazio", "usuario");

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    session.CreateQuery(@"UPDATE Artigo SET Status = :status WHERE Id IN (:id)")
                        .SetInt32("status", (int)Status.Excluido)
                        .SetParameterList("id", id)
                        .ExecuteUpdate();
                }
            }
        }

        internal static IList<Artigo> Filtrar(FiltroForm filtro)
        {
            IList<Artigo> resultado = new List<Artigo>();

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    resultado = session.CreateQuery(@"FROM Artigo WHERE Status IN (:status) AND (Titulo LIKE :texto OR Texto LIKE :texto OR Resumo LIKE :texto) ORDER BY Id Desc")
                        .SetParameterList("status", new List<Status>() { Status.Ativo, Status.Inativo })
                        .SetString("texto", "%" + filtro.PalavraChave + "%")
                        .SetMaxResults(1000)
                        .List<Artigo>();
                }
            }

            return resultado;
        }

        internal static Artigo Alterar(AlteraForm dados, Usuario usuario)
        {
            if (object.Equals(dados, null))
                throw new ArgumentException("Valor não pode ser nulo", "dados");

            if (object.Equals(usuario, null))
                throw new ArgumentException("Valor não pode ser nulo", "usuario");

            Artigo resultado = Mapper.Map<AlteraForm, Artigo>(dados);

            resultado.Responsavel = usuario;
            resultado.DataAlteracao = DateTime.Now;
            using (ArtigoRepository acao = new ArtigoRepository())
            {
                acao.Edit(resultado);
            }

            return resultado;
        }

        internal static Artigo Info(long id)
        {
            if (id <= 0)
                throw new ArgumentException("Valor não pode ser menor ou igual a 0", "id");

            Artigo resultado = null;

            using (ArtigoRepository acao = new ArtigoRepository())
            {
                resultado = acao.Find(id);
                if (resultado != null)
                    resultado.Autor.Count();
            }

            return resultado;
        }

        internal static IList<Artigo> Info(IList<long> ids)
        {
            if (object.Equals(ids, null))
                throw new ArgumentNullException("ids", "Valor não pode ser nulo");

            if (ids.Where(x => x <= 0).Count() > 0)
                throw new ArgumentException("Valor não pode ser menor ou igual a 0", "ids");

            IList<Artigo> resultado = new List<Artigo>();

            using (Connection connection = new Connection())
            {
                using (ISession session = connection.Session)
                {
                    resultado = session.CreateQuery(@"FROM Artigo WHERE Id IN (:id)")
                        .SetParameterList("id", ids)
                        .List<Artigo>();
                }
            }

            return resultado;
        }

        internal static Artigo Inserir(InsereForm dados, Usuario usuario)
        {
            if (object.Equals(dados, null))
                throw new ArgumentException("Valor não pode ser nulo", "dados");

            if (object.Equals(usuario, null))
                throw new ArgumentException("Valor não pode ser nulo", "usuario");

            Artigo resultado = Mapper.Map<InsereForm, Artigo>(dados);

            resultado.Responsavel = usuario;
            using(ArtigoRepository acao = new ArtigoRepository())
            {
                acao.Add(resultado);
            }

            return resultado;
        }
    }
}
