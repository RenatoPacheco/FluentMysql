using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Areas.Admin.ViewsData
{
    public abstract class BaseFiltroForm
    {

        [Display(Name = "Id Referêcia")]
        public long IdReferencia { get; set; }

        [Display(Name = "Ordem Crescente")]
        public bool OrdemCrescente { get; set; }


        private int _MaximoPorConsulta;
        [Display(Name = "Máximo por Consulta")]
        public int MaximoPorConsulta { 
            get { return _MaximoPorConsulta; }
            set { _MaximoPorConsulta = value > 1000 || value < 1 ? 1000 : value;  }
        }

        [Display(Name = "Palavra Chave")]
        public string PalavraChave { get; set; }

        [Display(Name = "Status")]
        public IList<Status> Status { get; set; }

        public BaseFiltroForm()
        {
            IdReferencia = -1;
            MaximoPorConsulta = 50;
        }
    }
}
