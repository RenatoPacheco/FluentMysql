using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentMysql.Domain.Repository;
using NHibernate;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using System.Linq;
using System.Diagnostics;

namespace FluentMysql.Domain.Tests
{
    [TestClass]
    public class UsuarioRepositoryTest
    {
        [TestMethod]
        public void Inserir_um_novo_usuario()
        {
            Usuario info = new Usuario(){
                Nome = "Renato",
                Sobrenome = "Bevilacqua Pacheco",
                Login = "RenatoPacheco",
                Email = "contato@renatopacheco.com.br",
                Senha = "123456",
                CPF = "00000000000",
                Nivel = Nivel.Sistema,
                Status = Status.Ativo
            };

            using(UsuarioRepository acao = new UsuarioRepository())
            {
                info.Responsavel = acao.Query().ToList().FirstOrDefault();
                acao.Add(info);
            }

            Assert.AreNotEqual(0, info.Id);

            Debug.WriteLine(string.Format("Id: {0}", info.Id));
            Debug.WriteLine(string.Format("Nome: {0} {1}", info.Nome, info.Sobrenome));
        }
    }
}
