using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.DataAnnotations
{

    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IsRequeridIfOtherNotNull : BaseValidationAttribute
    {
        public string[] Property { get; set; }

        public IsRequeridIfOtherNotNull(string[] property)
        {
            if (object.Equals(property, null))
                throw new ArgumentNullException("property", "O valor não pode ser nulo");

            Property = property;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool requerid = false;
            PropertyInfo info;
            foreach (string item in Property)
            {
                info = validationContext.ObjectType.GetProperty(item);

                if (info == null)
                    return new ValidationResult(String.Format("Propriedade não encontrada: {0}.", item));

                if (!object.Equals(info.GetValue(validationContext.ObjectInstance, null), null))
                {
                    requerid = true;
                    break;
                }
            }

            if (!requerid || !object.Equals(value, null))
                return null;

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
