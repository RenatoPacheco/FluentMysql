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
    public class ArtigoMap : ClassMap<Artigo>
    {
        public ArtigoMap()
        {
            Table("tbl_artigo");

            Id(x => x.Id)
                .GeneratedBy
                .Identity();

            Map(x => x.Guid)
               .Not.Nullable()
               .Length(100);

            Map(x => x.Titulo)
               .Not.Nullable()
               .Length(255);

            Map(x => x.Resumo)
               .Nullable()
               .Length(255);

            Map(x => x.Texto)
               .Not.Nullable()
               .CustomSqlType("text");

            Map(x => x.Hashtag)
               .Not.Nullable()
               .Length(255);

            Map(x => x.DataInicio)
               .Nullable();

            Map(x => x.DataTermino)
               .Nullable();

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

            HasManyToMany<Usuario>(x => x.Autor)
                .LazyLoad()
                .Cascade.Merge()
                .Table("tbl_autor");
        }
    }
}
