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
    public static class BSValidationIconExtensions
    {
        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return BSValidationIconFor(htmlHelper, expression, false, string.Empty, string.Empty, new RouteValueDictionary(new { }), string.Empty);
        }

        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess)
        {
            return BSValidationIconFor(htmlHelper, expression, showSucess, string.Empty, string.Empty, new RouteValueDictionary(new { }), string.Empty);
        }

        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, object htmlAttributes)
        {
            return BSValidationIconFor(htmlHelper, expression, showSucess, string.Empty, string.Empty, new RouteValueDictionary(htmlAttributes), string.Empty);
        }

        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, IDictionary<string, object> htmlAttributes)
        {
            return BSValidationIconFor(htmlHelper, expression, showSucess, string.Empty, string.Empty, htmlAttributes, string.Empty);
        }

        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, IDictionary<string, object> htmlAttributes, string tag)
        {
            return BSValidationIconFor(htmlHelper, expression, showSucess, string.Empty, string.Empty, htmlAttributes, tag);
        }

        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, string iconError)
        {
            return BSValidationIconFor(htmlHelper, expression, showSucess, iconError, string.Empty, new RouteValueDictionary(new { }), string.Empty);
        }

        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, string iconError, string iconSucess)
        {
            return BSValidationIconFor(htmlHelper, expression, showSucess, iconError, iconSucess, new RouteValueDictionary(new { }), string.Empty);
        }

        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, string iconError, string iconSucess, object htmlAttributes)
        {
            return BSValidationIconFor(htmlHelper, expression, showSucess, iconError, iconSucess, new RouteValueDictionary(htmlAttributes), string.Empty);
        }

        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, string iconError, string iconSucess, string tag)
        {
            return BSValidationIconFor(htmlHelper, expression, showSucess, iconError, iconSucess, new RouteValueDictionary(new { }), tag);
        }

        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, string iconError, string iconSucess, IDictionary<string, object> htmlAttributes)
        {
            return BSValidationIconFor(htmlHelper, expression, showSucess, iconError, iconSucess, htmlAttributes, string.Empty);
        }

        public static MvcHtmlString BSValidationIconFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showSucess, string iconError, string iconSucess, IDictionary<string, object> htmlAttributes, string tag)
        {
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var state = htmlHelper.ViewData.ModelState[htmlFieldName];
            bool submit = !object.Equals(state, null);
            bool error = submit && state.Errors.Count > 0;
            bool sucess = submit && state.Errors.Count == 0;

            if (!submit || (sucess && !showSucess))
                return MvcHtmlString.Empty;

            if (string.IsNullOrWhiteSpace(tag))
                tag = "span";

            if (string.IsNullOrWhiteSpace(iconError))
                iconError = "glyphicon glyphicon-remove";

            if (string.IsNullOrWhiteSpace(iconSucess))
                iconSucess = "glyphicon glyphicon-ok";

            TagBuilder span = new TagBuilder(tag);
            span.MergeAttributes(htmlAttributes);
            span.AddCssClass("form-control-feedback");

            if (sucess)
                span.AddCssClass(iconSucess);
            else
                span.AddCssClass(iconError);

            return MvcHtmlString.Create(span.ToString(TagRenderMode.Normal));
        }
    }
}
