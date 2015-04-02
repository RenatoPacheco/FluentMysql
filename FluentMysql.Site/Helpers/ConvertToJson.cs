using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FluentMysql.Site.Helpers
{
    public static class ConvertToJson
    {
        public static SortedList<string, string> ModelState(ModelStateDictionary value)
        {
            SortedList<string, string> erros = new SortedList<string, string>();
            string nome, erro;

            for (int index = 0; index < value.Values.Count; index++)
            {
                nome = value.Keys.ElementAt(index);
                if (value.Values.ElementAt(index).Errors.Count > 0)
                {
                    erro = value.Values.ElementAt(index).Errors[0].ErrorMessage.ToString();
                    erros.Add(nome, erro);
                }
            }

            return erros;
        }
    }
}
