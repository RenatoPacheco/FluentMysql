using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.UserTypes;
using FluentMysql.Infrastructure.ValueObject;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentMysql.Infrastructure.Maps
{
    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Table("tbl_usuario");

            Id(x => x.Id)
                .GeneratedBy
                .Identity();

            Map(x => x.Guid)
               .Not.Nullable()
               .Length(100);

            Map(x => x.Nome)
               .Not.Nullable()
               .Length(100);

            Map(x => x.Sobrenome)
               .Not.Nullable()
               .Length(255);

            Map(x => x.Email)
               .Not.Nullable()
               .Length(255);

            Map(x => x.Login)
               .Nullable()
               .Length(255);

            Map(x => x.Senha)
               .Nullable()
               .Length(100);

            Map(x => x.CPF)
               .Not.Nullable()
               .Length(100);

            Map(x => x.DataInicio)
               .Nullable();

            Map(x => x.DataTermino)
               .Nullable();

            Map(x => x.Nivel)
               .Not.Nullable()
               .CustomType<EnumAsChar<Nivel>>()
               .CustomSqlType("char(1)");

            Map(x => x.DataCriacao)
               .Not.Nullable();

            Map(x => x.DataAlteracao)
               .Not.Nullable();

            Map(x => x.Status)
               .Not.Nullable()
               .CustomType<EnumAsChar<Status>>()
               .CustomSqlType("char(1)");

            References(x => x.Responsavel)
               .Nullable()
               .Not.LazyLoad();

            HasManyToMany<Artigo>(x => x.Artigo)
                .LazyLoad()
                .Cascade.Merge()
                .Table("tbl_autor");
        }
    }
}
