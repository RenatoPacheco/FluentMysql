using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FluentMysql.Infrastructure.DataAnnotations
{
    public class IsFileImageAttribute : BaseValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (!object.Equals(value, null))
            {
                try
                {
                    var file = value as HttpPostedFileBase;
                    using (var img = Image.FromStream(file.InputStream))
                    {
                        return img != null;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
