using AutoMapper;
using FluentMysql.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InfraEntities = FluentMysql.Infrastructure.Entities;
using InfraValueObject = FluentMysql.Infrastructure.ValueObject;

namespace FluentMysql.Site.Areas.Admin
{
    public class AutoMapperConfig
    {
        public static void RegisterAutoMappers()
        {

            Mapper.CreateMap<ViewsData.Artigo.InsereForm, InfraEntities.Artigo>()
                .AfterMap((s, d) => d.Autor = s.Autores != null && s.Autores.Count > 0 ? s.Autores.Select(x => new Usuario(){ Id = x }).ToList() : new List<Usuario>() );
            Mapper.CreateMap<InfraEntities.Artigo, ViewsData.Artigo.InsereForm>()
                .AfterMap((s, d) => d.DataInicio = string.IsNullOrEmpty(d.DataInicio) ? d.DataInicio : Regex.Replace(d.DataInicio.Trim(), " [^ ]*$", ""))
                .AfterMap((s, d) => d.DataTermino = string.IsNullOrEmpty(d.DataTermino) ? d.DataTermino : Regex.Replace(d.DataTermino.Trim(), " [^ ]*$", ""));

            Mapper.CreateMap<ViewsData.Artigo.AlteraForm, InfraEntities.Artigo>()
                .AfterMap((s, d) => d.Autor = s.Autores != null && s.Autores.Count > 0 ? s.Autores.Select(x => new Usuario() { Id = x }).ToList() : new List<Usuario>());
            Mapper.CreateMap<InfraEntities.Artigo, ViewsData.Artigo.AlteraForm>()
                .AfterMap((s, d) => d.DataInicio = string.IsNullOrEmpty(d.DataInicio) ? d.DataInicio : Regex.Replace(d.DataInicio.Trim(), " [^ ]*$", ""))
                .AfterMap((s, d) => d.DataTermino = string.IsNullOrEmpty(d.DataTermino) ? d.DataTermino : Regex.Replace(d.DataTermino.Trim(), " [^ ]*$", ""))
                .AfterMap((s, d) => d.Autores = s.Autor.Select(x => x.Id).ToList() );

        }
    }
}
