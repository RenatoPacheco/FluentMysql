using FluentMysql.Infrastructure;
using FluentMysql.Site.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FluentMysql.Site.Areas.Admin.Models.Services
{
    public static class ConverteResultadoService
    {
        public static void ParaXml(object valor, object filtro, string mensagem)
        {
            ParaXml(new List<object>() { valor }, filtro, mensagem);
        }

        public static ActionResult ParaXml<T>(IList<T> valor, object filtro, string mensagem)
        {            
            XElement xElem = new XElement("Resultado");
            XDocument xDoc = new XDocument(new XElement("Sistema"));

            if (string.IsNullOrEmpty(mensagem))
                mensagem = string.Empty;

            xDoc.Element("Sistema").Add(ObjectUtility.PropertyToXElement(filtro));
            xDoc.Element("Sistema").Add(new XElement("Mensagem", new XCData(mensagem)));
            foreach (var item in valor)
            {
                xElem.Add(ObjectUtility.PropertyToXElement(item, "Item"));
            }
            xDoc.Element("Sistema").Add(xElem);

            return new XmlResult() { XmlContent = xDoc.ToString() };
        }

        public static void FiltroParaJson(object valor, object filtro, string mensagem)
        {
            ParaJson(new List<object>() { valor }, filtro, mensagem);
        }

        public static ActionResult ParaJson<T>(IList<T> valor, object filtro, string mensagem)
        {

            SortedList<string, object> jDoc = new SortedList<string, object>();
            IList<IDictionary<string, string>> jElem = new List<IDictionary<string, string>>();

            if (string.IsNullOrEmpty(mensagem))
                mensagem = string.Empty;

            jDoc.Add("Filtro", ObjectUtility.PropertyToDictionary(filtro));
            jDoc.Add("Mensagem", mensagem);
            foreach (var item in valor)
            {
                jElem.Add(ObjectUtility.PropertyToDictionary(item));
            }
            jDoc.Add("Resultado", jElem);

            return new JsonResult() { ContentType = "utf-8", Data = jDoc, JsonRequestBehavior = JsonRequestBehavior.AllowGet, ContentEncoding = Encoding.UTF8 };
        }
    }
}
