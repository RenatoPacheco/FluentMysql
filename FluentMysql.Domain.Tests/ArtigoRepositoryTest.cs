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
    public class ArtigoRepositoryTest
    {
        [TestMethod]
        public void Inserir_um_novo_artigo()
        {
            Artigo info = new Artigo()
            {
                Titulo = "Artigo teste",
                Resumo = "Qualquer resumo",
                Texto = "conteúdo do artigo aqui...",
                Hashtag = "para,facilitar,busca",
                DataInicio = null,
                DataTermino = null,
                Status = Status.Ativo
            };

            using (UsuarioRepository acao = new UsuarioRepository())
            {
                info.Responsavel = acao.Query().ToList().FirstOrDefault();
                info.Autor.Add(acao.Query().ToList().FirstOrDefault());
            }

            using (ArtigoRepository acao = new ArtigoRepository())
            {
                acao.Add(info);
            }

            Assert.AreNotEqual(0, info.Id);

            Debug.WriteLine(string.Format("Id: {0}", info.Id));
            Debug.WriteLine(string.Format("Título: {0}", info.Titulo));
        }
    }
}
