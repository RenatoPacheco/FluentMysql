using FluentMysql.Infrastructure.Interfaces;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FluentMysql.Infrastructure.Entities
{
    public class Artigo : IEntities, ICloneable
    {
        [Display(Name = "Artigo")]
        public virtual long Id { get; set; }

        [Display(Name = "Identificação Única")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public virtual Guid Guid { get; set; }

        [Display(Name="Autor")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public virtual IList<Usuario> Autor { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(255, ErrorMessage = "{0} deve conter no máximo {1} caractér(es)")]
        public virtual string Titulo { get; set; }

        [Display(Name = "Resumo")]
        [MaxLength(255, ErrorMessage = "{0} deve conter no máximo {1} caractér(es)")]
        public virtual string Resumo { get; set; }

        [Display(Name = "Hashtag")]
        [MaxLength(255, ErrorMessage = "{0} deve conter no máximo {1} caractér(es)")]
        public virtual string Hashtag { get; set; }

        [Display(Name = "Texto")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public virtual string Texto { get; set; }

        [Display(Name = "Data de Início")]
        public virtual DateTime? DataInicio { get; set; }

        [Display(Name = "Data de Término")]
        public virtual DateTime? DataTermino { get; set; }

        [Display(Name = "Data de Criação")]
        public virtual DateTime DataCriacao { get; set; }

        [Display(Name = "Data de Alteração")]
        public virtual DateTime DataAlteracao { get; set; }

        [Display(Name = "Status")]
        public virtual Status Status { get; set; }

        [Display(Name = "Responsável")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public virtual Usuario Responsavel { get; set; }

        public Artigo()
        {
            Guid = Guid.NewGuid();
            Autor = new List<Usuario>();
            DataCriacao = DateTime.Now;
            DataAlteracao = DateTime.Now;
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
