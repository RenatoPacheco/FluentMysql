using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Helpers
{
    public static class AlertsMessages
    {
        public static string Success(string text)
        {
            return Write(text, "success", "p");
        }

        public static string Success(string text, string tag)
        {
            return Write(text, "success", tag);
        }

        public static string Info(string text)
        {
            return Write(text, "info", "p");
        }

        public static string Info(string text, string tag)
        {
            return Write(text, "info", tag);
        }

        public static string Warning(string text)
        {
            return Write(text, "warning", "p");
        }

        public static string Warning(string text, string tag)
        {
            return Write(text, "warning", tag);
        }

        public static string Danger(string text)
        {
            return Write(text, "danger", "p");
        }

        public static string Danger(string text, string tag)
        {
            return Write(text, "danger", tag);
        }

        public static string Write(string text, string type, string tag)
        {
            return string.Format("<{2} class=\"alert alert-{1}\" role=\"alert\">{0}</{2}>", text, type, tag);
        }
    }
}
