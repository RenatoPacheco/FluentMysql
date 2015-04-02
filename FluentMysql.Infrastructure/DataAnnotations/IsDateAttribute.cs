using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.DataAnnotations
{
    public class IsDateAttribute : BaseValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string pattern;
            DateTime date = DateTime.Now;
            
            if (!base.IsValid(value))
            {
                pattern = @"^([0-9]{1,2})(/)([0-9]{1,2})(/)([0-9]{1,})$";
                if(Regex.IsMatch(this.Input, pattern))
                {
                    Input = Regex.Replace(Input, pattern, "$5-$3-$1");
                    return DateTime.TryParse(Input, out date);
                }
                return false;
            }
            return true;
        }
    }
}
