using FluentMysql.Infrastructure.Interfaces;
using FluentMysql.Infrastructure.Maps;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentMysql.Infrastructure
{
    public class Connection : IConnection
    {
        private FluentConfiguration _Configuration;
        private ISessionFactory _Sessiofactory;
        private ISession _Session;
        private string _ConnectionName;

        public FluentConfiguration Configuration
        {
            get { return _Configuration; }
            private set { _Configuration = value; }
        }
        public ISessionFactory SessioFactory
        {
            get { return _Sessiofactory; }
            private set { _Sessiofactory = value; }
        }

        public ISession Session
        {
            get { return _Session; }
            private set { _Session = value; }
        }

        public static void CreatTable()
        {
            CreatTable("FluentMysql");
        }

        public static void CreatTable(string connectionName)
        {
            //throw new Exception("O método CreatTable foi desabilitado");

            FluentConfiguration configuration = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(
                            x => x.FromConnectionStringWithKey(connectionName)).ShowSql())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<UsuarioMap>());

            configuration.BuildSessionFactory();
        }

        public static void UpdateTable()
        {
            UpdateTable("FluentMysql");
        }

        public static void UpdateTable(string connectionName)
        {
            //throw new Exception("O método CreatTable foi desabilitado");

            FluentConfiguration configuration = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(
                            x => x.FromConnectionStringWithKey(connectionName)).ShowSql())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<UsuarioMap>());

            configuration.BuildSessionFactory();
        }

        public Connection() : this("FluentMysql") { }

        public Connection(string connectionName)
        {
            _ConnectionName = connectionName;
            Init();
        }

        private void Init()
        {
            _Configuration = Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                .ConnectionString(
                            x => x.FromConnectionStringWithKey(_ConnectionName)).ShowSql())
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<UsuarioMap>());

            _Sessiofactory = _Configuration.BuildSessionFactory();
            _Session = _Sessiofactory.OpenSession();
        }

        public ISession Open()
        {
            if (_Session.IsOpen == false)
            {
                _Session = _Sessiofactory.OpenSession();
            }
            return _Session;
        }

        public void Close()
        {
            if (_Session.IsOpen) _Session.Close();

            _Configuration = null;

            if (_Sessiofactory.IsClosed == false) _Sessiofactory.Close();

            _Sessiofactory.Dispose();
            _Sessiofactory = null;
            _Session.Dispose();
            _Session = null;
        }
        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }
    }
}
