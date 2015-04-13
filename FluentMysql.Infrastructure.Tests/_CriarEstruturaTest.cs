using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentMysql.Infrastructure;
using NHibernate;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;

namespace FluentMysql.Infrastructure.Tests
{
    [TestClass]
    public class _CriarEstruturaTest
    {
        [TestMethod]
        public void Cria_as_tabelas_limpando_o_banco()
        {
            Connection.CreatTable();
            
            using(Connection connection = new Connection())
            {
                using(ISession session = connection.Session)
                {
                    Usuario usuario = new Usuario() {
                        Nome = "Renato",
                        Sobrenome = "Bevilacqua Pacheco",
                        Email = "contato@renatopacheco.com.br",
                        Login = "RenatoPacheco",
                        Senha = "123456",
                        CPF = "91782716491",
                        Nivel = ValueObject.Nivel.Sistema,
                        Status = Status.Ativo
                    };

                    session.Save(usuario);

                    usuario.Responsavel = usuario;
                    session.Transaction.Begin();
                    session.Update(usuario);
                    session.Transaction.Commit();
                }
            }
        }

        [TestMethod]
        public void Tenta_altualizar_a_estrutura_das_tabelas()
        {
            Connection.UpdateTable();
        }
    }
}
