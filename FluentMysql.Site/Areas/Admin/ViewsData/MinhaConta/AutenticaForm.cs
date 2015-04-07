using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData.MinhaConta
{
    public class AutenticaForm
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Nome { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Sobrenome { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage="{0} é obrigatório")]
        public string Login { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string CPF { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Senha { get; set; }

        [Display(Name = "Comfirma")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [Compare("Senha", ErrorMessage="{0} não coincide com {1}")]
        public string ComfirmaSenha { get; set; }

        [Display(Name = "Token")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Token { get; set; }
    }
}
