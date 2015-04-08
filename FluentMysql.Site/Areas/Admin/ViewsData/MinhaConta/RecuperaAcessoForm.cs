using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData.MinhaConta
{
    public class RecuperaAcessoForm
    {
        [Display(Name="Identificação")]
        [Required(ErrorMessage="{0} é obrigatório")]
        public virtual string Identificacao { get; set; }

        [Display(Name = "Ação")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public virtual string Acao { get; set; }
    }
}
