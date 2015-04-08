using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentMysql.Site.Web.Mvc.Bootstrap
{
    public static class HtmlHelperExtensions
    {
        public static IDisposable BSFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return BSFormGroupFor(htmlHelper, expression, false, new RouteValueDictionary(new { }), string.Empty);
        }

        public static IDisposable BSFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess)
        {
            return BSFormGroupFor(htmlHelper, expression, showSucess, new RouteValueDictionary(new { }), string.Empty);
        }

        public static IDisposable BSFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, string tag)
        {
            return BSFormGroupFor(htmlHelper, expression, showSucess, new RouteValueDictionary(new { }), tag);
        }

        public static IDisposable BSFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, object htmlAttributes)
        {
            return BSFormGroupFor(htmlHelper, expression, showSucess, new RouteValueDictionary(htmlAttributes), string.Empty);
        }

        public static IDisposable BSFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, IDictionary<string, object> htmlAttributes)
        {
            return BSFormGroupFor(htmlHelper, expression, showSucess, htmlAttributes, string.Empty);
        }

        public static IDisposable BSFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, object htmlAttributes, string tag)
        {
            return BSFormGroupFor(htmlHelper, expression, showSucess, new RouteValueDictionary(htmlAttributes), tag);
        }

        public static IDisposable BSFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, IDictionary<string, object> htmlAttributes, string tag)
        {
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var state = htmlHelper.ViewData.ModelState[htmlFieldName];
            bool submit = !object.Equals(state, null);
            bool error = submit && state.Errors.Count > 0;
            bool sucess = submit && state.Errors.Count == 0;

            if (string.IsNullOrWhiteSpace(tag))
                tag = "div";

            TagBuilder div = new TagBuilder(tag);
            div.MergeAttributes(htmlAttributes);
            div.AddCssClass("form-group");

            if (error)
                div.AddCssClass("has-error");
            else if (sucess && showSucess)
                div.AddCssClass("has-success");

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.StartTag));

            return TagDisponse.Disponse(htmlHelper, tag);
        }
    }
}
