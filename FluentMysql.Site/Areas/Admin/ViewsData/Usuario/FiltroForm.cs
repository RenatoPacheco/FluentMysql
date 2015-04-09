using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData.Usuario
{
    public class FiltroForm
    {
        [Display(Name = "Palavra Chave")]
        public string PalavraChave { get; set; }

        [Display(Name = "Ação")]
        public string Acao { get; set; }

        [Display(Name = "Selecionados")]
        public IList<long> Selecionados { get; set; }

        [Display(Name = "Página")]
        public int Pagina { get; set; }
    }
}
