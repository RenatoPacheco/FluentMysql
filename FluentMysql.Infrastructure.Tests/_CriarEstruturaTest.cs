using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentMysql.Infrastructure;

namespace FluentMysql.Infrastructure.Tests
{
    [TestClass]
    public class _CriarEstruturaTest
    {
        [TestMethod]
        public void Cria_as_tabelas_limpando_o_banco()
        {
            Connection.CreatTable();
        }

        [TestMethod]
        public void Tenta_altualizar_a_estrutura_das_tabelas()
        {
            Connection.UpdateTable();
        }
    }
}
