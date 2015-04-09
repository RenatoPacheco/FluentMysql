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
        [StringLength(16, MinimumLength = 6, ErrorMessage = "{0} deve ter entre {2} e {1} caractéres")]
        [RegularExpression(@"^[0-9A-Za-z]*$", ErrorMessage = "{0} deve ter somente caracteres de a-z e 0-9")]
        public string NovaSenha { get; set; }

        [Display(Name = "Confirma senha")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [Compare("NovaSenha", ErrorMessage = "{0} não conicide com {1}")]
        public string ConfirmaSenha { get; set; }
    }
}
