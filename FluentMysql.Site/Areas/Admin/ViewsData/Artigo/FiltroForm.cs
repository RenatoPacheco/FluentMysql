using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData.Artigo
{
    public class FiltroForm : BaseFiltroForm
    {
        [Display(Name = "Ação")]
        public string Acao { get; set; }

        [Display(Name = "Selecionados")]
        public IList<long> Selecionados { get; set; }
    }
}
