using FluentMysql.Infrastructure.ValueObject;
using Entities = FluentMysql.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FluentMysql.Site.Areas.Admin.ViewsData.Usuario
{
    public class AlteraNivelForm
    {
        [Display(Name = "Usuário")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} é obrigatório")]
        public virtual long Id { get; set; }

        [Display(Name = "Nível")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public virtual Nivel Nivel { get; set; }

        public virtual IEnumerable<SelectListItem> NivelOpcoes(Entities.Usuario usuario)
        {
            if (object.Equals(usuario, null))
                throw new ArgumentException("O nalor não pode ser nulo", "usuario");

            IEnumerable<SelectListItem> resultado = new List<SelectListItem>() { new SelectListItem() { Text="Selecione um item", Value="0" } };
            IEnumerable<SelectListItem> enumerador = Enum.GetValues(typeof(Nivel)).Cast<Nivel>().Where(x => x > usuario.Nivel).Select(x => new SelectListItem() { Text = x.ToString(), Value = ((int)x).ToString() });

            resultado = resultado.Concat(enumerador);

            return resultado;
        }
    }
}
