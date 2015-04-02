using FluentMysql.Infrastructure.Interfaces;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FluentMysql.Infrastructure.Entities
{
    public class Usuario : IEntities, ICloneable
    {
        [Display(Name = "Usuário")]
        public virtual long Id { get; set; }

        [Display(Name = "Identificação Única")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public virtual Guid Guid { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(100, ErrorMessage = "{0} deve conter no máximo {1} caractér(es)")]
        public virtual string Nome { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(255, ErrorMessage = "{0} deve conter no máximo {1} caractér(es)")]
        public virtual string Sobrenome { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(255, ErrorMessage = "{0} deve conter no máximo {1} caractér(es)")]
        public virtual string Email { get; set; }

        [Display(Name = "Login")]
        [MaxLength(255, ErrorMessage = "{0} deve conter no máximo {1} caractér(es)")]
        public virtual string Login { get; set; }

        [Display(Name = "Senha")]
        [MaxLength(100, ErrorMessage = "{0} deve conter no máximo {1} caractér(es)")]
        public virtual string Senha { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(11, ErrorMessage = "{0} deve conter no máximo {1} caractér(es)")]
        public virtual string CPF { get; set; }

        [Display(Name = "Data de Início")]
        public virtual DateTime? DataInicio { get; set; }

        [Display(Name = "Data de Término")]
        public virtual DateTime? DataTermino { get; set; }

        [Display(Name = "Nível")]
        public virtual Nivel Nivel { get; set; }

        [Display(Name="Artigo")]
        public virtual IList<Artigo> Artigo { get; set; }

        [Display(Name = "Data de Criação")]
        public virtual DateTime DataCriacao { get; set; }

        [Display(Name = "Data de Alteração")]
        public virtual DateTime DataAlteracao { get; set; }

        [Display(Name = "Status")]
        public virtual Status Status { get; set; }

        [Display(Name = "Responsável")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public virtual Usuario Responsavel { get; set; }

        public Usuario()
        {
            Guid = Guid.NewGuid();
            DataCriacao = DateTime.Now;
            DataAlteracao = DateTime.Now;
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
