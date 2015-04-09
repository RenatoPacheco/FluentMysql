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
    public static class BSSummaryExtensions
    {
        #region BSValidationSummary

        public static MvcHtmlString BSValidationSummary(this HtmlHelper htmlHelper)
        {
            return BSValidationSummary(htmlHelper, string.Empty, new RouteValueDictionary(new { }));
        }

        public static MvcHtmlString BSValidationSummary(this HtmlHelper htmlHelper, string tag)
        {
            return BSValidationSummary(htmlHelper, tag, new RouteValueDictionary(new { }));
        }

        public static MvcHtmlString BSValidationSummary(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            return BSValidationSummary(htmlHelper, string.Empty, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString BSValidationSummary(this HtmlHelper htmlHelper, IDictionary<string, object> htmlAttributes)
        {
            return BSValidationSummary(htmlHelper, string.Empty, htmlAttributes);
        }

        public static MvcHtmlString BSValidationSummary(this HtmlHelper htmlHelper, string tag, object htmlAttributes)
        {
            return BSValidationSummary(htmlHelper, tag, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString BSValidationSummary(this HtmlHelper htmlHelper, string tag, IDictionary<string, object> htmlAttributes)
        {
            var state = htmlHelper.ViewData.ModelState;
            bool submit = !object.Equals(state, null);
            bool error = submit && state.Values.Any(x => x.Errors.Count > 0);
            bool sucess = submit && !error && state.Values.Any(x => x.Errors.Count == 0);

            if (!submit || sucess)
                return MvcHtmlString.Empty;

            if (string.IsNullOrWhiteSpace(tag))
                tag = "div";

            TagBuilder div = new TagBuilder(tag);
            div.MergeAttributes(htmlAttributes);
            div.Attributes.Add("role", "alert");
            div.AddCssClass("alert alert-danger alert-dismissible");

            TagBuilder button = new TagBuilder("button");
            button.Attributes.Add("type", "button");
            button.Attributes.Add("data-dismiss", "alert");
            button.Attributes.Add("aria-label", "Close");
            button.AddCssClass("close");
            button.InnerHtml = "<span aria-hidden=\"true\">&times;</span>";

            div.InnerHtml = button.ToString(TagRenderMode.Normal);
            foreach (ModelState modelState in state.Values)
            {
                foreach (ModelError item in modelState.Errors)
                {
                    div.InnerHtml += string.Format("<p>{0}</p>", item.ErrorMessage.ToString());
                }
            }

            return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
        }

        #endregion BSValidationSummary

        #region BSValidationSummaryFor

        public static MvcHtmlString BSValidationSummaryFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            return BSValidationSummaryFor(htmlHelper, expression, string.Empty, new RouteValueDictionary(new { }));
        }

        public static MvcHtmlString BSValidationSummaryFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string tag)
        {
            return BSValidationSummaryFor(htmlHelper, expression, tag, new RouteValueDictionary(new { }));
        }

        public static MvcHtmlString BSValidationSummaryFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return BSValidationSummaryFor(htmlHelper, expression, string.Empty, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString BSValidationSummaryFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            return BSValidationSummaryFor(htmlHelper, expression, string.Empty, htmlAttributes);
        }

        public static MvcHtmlString BSValidationSummaryFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string tag, object htmlAttributes)
        {
            return BSValidationSummaryFor(htmlHelper, expression, tag, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString BSValidationSummaryFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string tag, IDictionary<string, object> htmlAttributes)
        {
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var state = htmlHelper.ViewData.ModelState[htmlFieldName];
            bool submit = !object.Equals(state, null);
            bool error = submit && state.Errors.Count > 0;
            bool sucess = submit && state.Errors.Count == 0;

            if(!submit || !error)
                return MvcHtmlString.Empty;

            if (string.IsNullOrWhiteSpace(tag))
                tag = "div";

            TagBuilder div = new TagBuilder(tag);
            div.MergeAttributes(htmlAttributes);
            div.AddCssClass("alert alert-danger");

            foreach (ModelError item in state.Errors)
            {
                div.InnerHtml += string.Format("<p>{0}</p>", item.ErrorMessage.ToString());
            }
            
            return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
        }

        #endregion BSValidationSummary
    }
}
