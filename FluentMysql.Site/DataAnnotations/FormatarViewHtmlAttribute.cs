using FluentMysql.Site.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.DataAnnotations
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class FormatarViewHtmlAttribute : Attribute, IFormatarViewAttribute
    {
        public string PalavraChave { get; protected set; }

        public string ViewName { get; set; }

        public string MasterName { get; set; }
        
        public FormatarViewHtmlAttribute(string palavraChave)
            : this(palavraChave, null, null) { }

        public FormatarViewHtmlAttribute(string palavraChave, string viewName)
            : this(palavraChave, viewName, null) { }

        public FormatarViewHtmlAttribute(string palavraChave, string viewName, string masterName)
        {
            if (string.IsNullOrWhiteSpace(palavraChave))
                throw new ArgumentNullException("palavraChave", "Valor não pode ser nulo ou vazio");

            PalavraChave = palavraChave;
            ViewName = viewName;
            MasterName = masterName;
        }
        
        string[] IFormatarViewAttribute.ViewData { get { return null; } }

        string[] IFormatarViewAttribute.TempData { get { return null; } }

        string IFormatarViewAttribute.Tipo { get { return "html"; } }
    }
}
