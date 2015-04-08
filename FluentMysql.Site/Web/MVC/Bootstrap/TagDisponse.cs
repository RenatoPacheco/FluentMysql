using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FluentMysql.Site.Web.Mvc.Bootstrap
{
    internal class TagDisponse : IDisposable
    {
        protected TagBuilder Tag { get; set; }
        protected HtmlHelper HtmlHelper { get; set; }

        TagDisponse(HtmlHelper htmlHelper, string tag)
        {
            Tag = new TagBuilder(tag);
            HtmlHelper = htmlHelper;
        }

        public static IDisposable Disponse(HtmlHelper htmlHelper, string tag)
        {
            return new TagDisponse(htmlHelper, tag);
        }

        public void Dispose()
        {
            HtmlHelper.ViewContext.Writer.Write(Tag.ToString(TagRenderMode.EndTag));
        }
    }
}
