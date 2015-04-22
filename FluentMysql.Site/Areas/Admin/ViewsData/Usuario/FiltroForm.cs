using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData.Usuario
{
    public class FiltroForm : BaseFiltroForm
    {
        [Display(Name = "Ação")]
        public AcaoView Acao { get; set; }
        
        [Display(Name = "Selecionados")]
        public IList<long> Selecionados { get; set; }

        [Display(Name = "Página")]
        public int Pagina { get; set; }

        [Display(Name = "Altera Nível Index")]
        public string AlteraNivelIndex { get; set; }

        [Display(Name = "Altera Nível Valor")]
        public IList<Nivel> AlteraNivelValor { get; set; }

        [Display(Name = "Altera Usuário Id")]
        public IList<long> AlteraUsuarioId { get; set; }
    }
}
