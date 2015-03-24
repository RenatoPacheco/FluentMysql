using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.DataAnnotations
{
    public abstract class BaseValidationAttribute : ValidationAttribute
    {
        public virtual bool IsValid<T>(IList<T> value)
        {
            if (!object.Equals(value, null))
            {
                foreach (object item in value)
                {
                    if (!this.IsValid(item))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected string Input;

        public override bool IsValid(object value)
        {
            return (object.Equals(value, null) || string.IsNullOrEmpty(this.Input = Convert.ToString(value)));
        }
    }
}
