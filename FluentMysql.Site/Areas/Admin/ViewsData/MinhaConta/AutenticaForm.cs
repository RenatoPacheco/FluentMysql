using FluentMysql.Infrastructure.DataAnnotations;
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
        [MaxLength(100, ErrorMessage = "{0} deve ter no máximo caractére(s)")]
        public string Nome { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(255, ErrorMessage = "{0} deve ter no máximo caractére(s)")]
        public string Sobrenome { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [StringLength(30, MinimumLength=5, ErrorMessage = "{0} deve ter ente {2} e {1} caractére(s)")]
        public string Login { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [IsCPF(ErrorMessage="{0} não possui um valor válido")]
        public string CPF { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [StringLength(16, MinimumLength=6, ErrorMessage="{0} deve ter entre {2} e {1} caractéres")]
        [RegularExpression(@"^[0-9A-Za-z]*$", ErrorMessage="{0} deve ter somente caracteres de a-z e 0-9")]
        public string Senha { get; set; }

        [Display(Name = "Confirma")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [Compare("Senha", ErrorMessage="{0} não coincide com {1}")]
        public string ConfirmaSenha { get; set; }

        [Display(Name = "Token")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Token { get; set; }
    }
}
