using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FluentMysql.Infrastructure.Factory
{
    public static class MessageFactory
    {
        public static IList<string> ErrorsToString(ModelStateDictionary modelStateDictionary)
        {
            return ErrorsToString(modelStateDictionary, "{0}");
        }

        public static IList<string> ErrorsToString(ModelStateDictionary modelStateDictionary, string message)
        {
            IList<string> result = new List<string>();

            foreach (ModelState modelState in modelStateDictionary.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    result.Add(string.Format(message, error.ErrorMessage.ToString()));
                }
            }

            return result;
        }
    }
}
