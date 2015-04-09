using FluentMysql.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData.MinhaConta
{
    public class AlterarDadosForm
    {
        [Display(Name = "Usuário")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} é obrigatório")]
        public virtual long Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(100, ErrorMessage = "{0} deve ter no máximo caractére(s)")]
        public virtual string Nome { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(255, ErrorMessage = "{0} deve ter no máximo caractére(s)")]
        public virtual string Sobrenome { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "{0} deve ter ente {2} e {1} caractére(s)")]
        public virtual string Login { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [IsCPF(ErrorMessage = "{0} não possui um valor válido")]
        public virtual string CPF { get; set; }

        [Display(Name = "Nova senha")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "{0} deve ter entre {2} e {1} caractéres")]
        [RegularExpression(@"^[0-9A-Za-z]*$", ErrorMessage = "{0} deve ter somente caracteres de a-z e 0-9")]
        public virtual string NovaSenha { get; set; }

        [Display(Name = "Confirma senha")]
        [Compare("NovaSenha", ErrorMessage = "{0} não coincide com o valor de {1}")]
        public virtual string ConfirmaSenha { get; set; }
        
        [Display(Name = "Novo e-mail")]
        [EmailAddress(ErrorMessage="{0} não contém um valor válido")]
        public virtual string NovoEmail { get; set; }

        [Display(Name = "Confirma e-mail")]
        [Compare("NovoEmail", ErrorMessage = "{0} não coincide com o valor de {1}")]
        public virtual string ConfirmaEmail { get; set; }

        [Display(Name = "Senha atual")]
        [IsRequeridIfOtherNotNull(new string[] { "NovaSenha", "NovoEmail" }, ErrorMessage = "{0} é obrigatório")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "{0} deve ter entre {2} e {1} caractéres")]
        [RegularExpression(@"^[0-9A-Za-z]*$", ErrorMessage = "{0} deve ter somente caracteres de a-z e 0-9")]
        public virtual string SenhaAtual { get; set; }
    }
}
