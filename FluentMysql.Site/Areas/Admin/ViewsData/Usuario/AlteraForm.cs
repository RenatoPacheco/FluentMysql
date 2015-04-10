using FluentMysql.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData.Usuario
{
    public class AlteraForm : InsereForm
    {
        [Display(Name = "Usuário")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} é obrigatório")]
        public virtual long Id { get; set; }

        [Display(Name = "Data Criação")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [IsDateTime(ErrorMessage = "{0} deve conter uma data e hora válida")]
        public virtual string DataCriacao { get; set; }
    }
}
