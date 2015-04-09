using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData.MinhaConta
{
    public class LoginForm
    {
        [Display(Name = "Login")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public virtual string Login { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "{0} deve ter entre {2} e {1} caractéres")]
        [RegularExpression(@"^[0-9A-Za-z]*$", ErrorMessage = "{0} deve ter somente caracteres de a-z e 0-9")]
        public virtual string Senha { get; set; }

        [Display(Name = "Lembrar acesso")]
        public virtual bool LembarAcesso { get; set; }
    }
}
