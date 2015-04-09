using FluentMysql.Infrastructure.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace FluentMysql.Site.Areas.Admin.Template.Email
{
    public class SolicitarRedefinirSenha
    {
        public string Nome { get; set; }

        public string Link { get; set; }
        
        public string Empresa { get; set; }
        
        public override string ToString()
        {
            string resultado = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Areas/Admin/Template/Email/SolicitarRedefinirSenha.html"));
            resultado = Regex.Replace(resultado, "{nome}", Nome, RegexOptions.IgnoreCase);
            resultado = Regex.Replace(resultado, "{link}", Link, RegexOptions.IgnoreCase);
            resultado = Regex.Replace(resultado, "{empresa}", Empresa, RegexOptions.IgnoreCase);
            resultado = Regex.Replace(resultado, "{site}", UriUtility.ToAbsoluteUrl("~/"), RegexOptions.IgnoreCase);

            return resultado;
        }

        public SolicitarRedefinirSenha(string nome, string link, string empresa)
        {
            Nome = nome;
            Link = link;
            Empresa = empresa;
        }
    }
}
