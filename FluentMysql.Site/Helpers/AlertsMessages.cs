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
            return string.Format("<p class=\"alert alert-success\" role=\"alert\">{0}</p>", text);
        }

        public static string Info(string text)
        {
            return string.Format("<p class=\"alert alert-info\" role=\"alert\">{0}</p>", text);
        }

        public static string Warning(string text)
        {
            return string.Format("<p class=\"alert alert-warning\" role=\"alert\">{0}</p>", text);
        }

        public static string Danger(string text)
        {
            return string.Format("<p class=\"alert alert-danger\" role=\"alert\">{0}</p>", text);
        }
    }
}
