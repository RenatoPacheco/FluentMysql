using FluentMysql.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData.Artigo
{
    public class InsereForm
    {
        [Display(Name = "Autor")]
        public virtual IList<long> Autores { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(255, ErrorMessage="{0} deve ter no máximo {1} caractére(s)")]
        public virtual string Titulo { get; set; }

        [Display(Name = "Resumo")]
        [MaxLength(255, ErrorMessage = "{0} deve ter no máximo {1} caractére(s)")]
        public virtual string Resumo { get; set; }

        [Display(Name = "Hashtag")]
        [MaxLength(255, ErrorMessage = "{0} deve ter no máximo {1} caractére(s)")]
        public virtual string Hashtag { get; set; }

        [Display(Name = "Texto")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public virtual string Texto { get; set; }

        [Display(Name = "Início")]
        [IsDate(ErrorMessage = "{0} deve conter uma data dd/mm/aaaa")]
        public virtual string DataInicio { get; set; }

        [Display(Name = "Término")]
        [IsDate(ErrorMessage = "{0} deve conter uma data dd/mm/aaaa")]
        public virtual string DataTermino { get; set; }

        [Display(Name = "Status")]
        public virtual bool Status { get; set; }
    }
}
