using FluentMysql.Site.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.DataAnnotations
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class FormatarViewXmlAttribute : Attribute, IFormatarViewAttribute
    {
        public string PalavraChave { get; protected set; }

        public string[] ViewData { get; set; }

        public string[] TempData { get; set; }
        
        public FormatarViewXmlAttribute(string palavraChave)
            : this(palavraChave, null, null) { }

        public FormatarViewXmlAttribute(string palavraChave, string[] viewData)
            : this(palavraChave, viewData, null) { }

        public FormatarViewXmlAttribute(string palavraChave, string[] viewData, string[] tempData)
        {
            if (string.IsNullOrWhiteSpace(palavraChave))
                throw new ArgumentNullException("palavraChave", "Valor não pode ser nulo ou vazio");

            PalavraChave = palavraChave;
            ViewData = viewData;
            TempData = tempData;
        }

        string IFormatarViewAttribute.ViewName { get { return null; } }

        string IFormatarViewAttribute.MasterName { get { return null; } }
        string IFormatarViewAttribute.Tipo { get { return "xml"; } }

        
    }
}
