using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData.MinhaConta
{
    public class RedefineSenhaForm
    {
        [Display(Name="Token")]
        [Required(ErrorMessage="{0} é obrigatório")]
        public string Token { get; set; }
        
        [Display(Name = "Nova senha")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string NovaSenha { get; set; }

        [Display(Name = "Confirma senha")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [Compare("NovaSenha", ErrorMessage = "{0} não conicide com {1}")]
        public string ComfirmaSenha { get; set; }
    }
}
