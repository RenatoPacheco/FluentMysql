using FluentMysql.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FluentMysql.Site.Areas.Admin.ViewsData.Artigo
{
    public class UploadForm
    {
        [Display(Name = "Arquivo")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [IsFileImageAttribute(ErrorMessage = "{0} não é válido")]
        public HttpPostedFileBase Upload { get; set; }

        public string CKEditor { get; set; }

        public string CKEditorFuncNum { get; set; }

        public string LangCode { get; set; }
    }
}
