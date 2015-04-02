﻿using System;
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
        [Required(ErrorMessage = "{0} é obrigatória")]
        public virtual string Senha { get; set; }

        [Display(Name = "Lembrar acesso")]
        public virtual bool LembarAcesso { get; set; }
    }
}