using FluentMysql.Site.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.DataAnnotations
{
    public interface IFormatarViewAttribute
    {
        string Tipo { get; }
        
        string PalavraChave { get; }

        string ViewName { get; }

        string MasterName { get; }

        string[] ViewData { get; }

        string[] TempData { get; }

    }
}
