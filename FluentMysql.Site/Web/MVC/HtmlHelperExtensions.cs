using FluentMysql.Site.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentMysql.Site.Web.Mvc
{
    /// <summary>
    /// Represents a HTML div in an Mvc View
    /// </summary>
    public class HtmlTag : IDisposable
    {
        private string _tag;
        private bool _disposed;
        private readonly ViewContext _viewContext;
        private readonly TextWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlTag"/> class.
        /// </summary>
        /// <param name="viewContext">The view context.</param>
        public HtmlTag(ViewContext viewContext, string tag)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("tag");
            }

            _tag = tag;
            _viewContext = viewContext;
            _writer = viewContext.Writer;
        }

        /// <summary>
        /// Performs application-defined tasks associated with 
        /// freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both 
        /// managed and unmanaged resources; <c>false</c> 
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                _writer.Write("</" + _tag + ">");
            }
        }

        /// <summary>
        /// Ends the div.
        /// </summary>
        public void EndTag()
        {
            Dispose(true);
        }
    }

    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Begins the div.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns></returns>
        public static HtmlTag BsBeginFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            // generates <div> ... </div>>
            return BsFormGroupFor(htmlHelper, expression, null);
        }

        /// <summary>
        /// Begins the div.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static HtmlTag BsBeginFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var attr = ObjectToDictionaryHelper.ToDictionary(htmlAttributes);
            return BsFormGroupFor(htmlHelper, expression, attr);
        }

        /// <summary>
        /// Begins the div.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static HtmlTag BsBeginFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            // generates <div> ... </div>>
            return BsFormGroupFor(htmlHelper, expression, htmlAttributes);
        }

        /// <summary>
        /// Ends the div.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        public static void BsEndFormGroupFor(this HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</div>");
        }

        /// <summary>
        /// Helps build a html div element
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        private static HtmlTag BsFormGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData
                .TemplateInfo.GetFullHtmlFieldName(expressionText);
            var state = htmlHelper.ViewData.ModelState[fullHtmlFieldName];

            TagBuilder tagBuilder = new TagBuilder("div");

            if (object.Equals(htmlAttributes, null))
                htmlAttributes = new Dictionary<string, object>();

            if (!htmlAttributes.ContainsKey("class"))
                htmlAttributes.Add("class", "form-group");
            else if (htmlAttributes["class"] == null || string.IsNullOrEmpty(htmlAttributes["class"].ToString()))
                htmlAttributes["class"] = "form-group";
            else
                htmlAttributes["class"] =  htmlAttributes["class"].ToString() + " form-group";

            if (state != null && state.Errors.Count == 0)
            {
                htmlAttributes["class"] += " has-success";
            }
            else if (state != null && state.Errors.Count > 0)
            {
                htmlAttributes["class"] += " has-error";
            }

            tagBuilder.MergeAttributes(htmlAttributes);

            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            HtmlTag div = new HtmlTag(htmlHelper.ViewContext, "div");

            return div;
        }

        public static MvcHtmlString BsValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return BsValidationMessageFor(htmlHelper, expression, false);
        }

        public static MvcHtmlString BsValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool feedback)
        {
            StringBuilder message = new StringBuilder();
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData
                .TemplateInfo.GetFullHtmlFieldName(expressionText);
            var state = htmlHelper.ViewData.ModelState[fullHtmlFieldName];

            if (state != null && state.Errors.Count == 0)
            {
                if (feedback)
                    message.Append("<span class=\"glyphicon glyphicon-ok form-control-feedback\" aria-hidden=\"true\"></span>");
            }
            else if (state != null && state.Errors.Count > 0)
            {
                if (feedback)
                    message.Append("<span class=\"glyphicon glyphicon-remove form-control-feedback\" aria-hidden=\"true\"></span>");
                
                for(int i = 0; i < state.Errors.Count; i++) {
                    message.Append("<p class=\"text-danger\">" + state.Errors[i].ErrorMessage.ToString() + "</p>");
                }
            }

            return new MvcHtmlString(message.ToString());
        }

    }
}
