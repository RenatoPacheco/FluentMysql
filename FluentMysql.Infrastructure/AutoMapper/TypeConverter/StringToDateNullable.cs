using AutoMapper;
using FluentMysql.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.AutoMapper.TypeConverter
{
    public class StringToDateNullable : ITypeConverter<string, DateTime?>
    {
        public DateTime? Convert(ResolutionContext context)
        {
            IsDateAttribute valid = new IsDateAttribute();
            string value = object.Equals(context.SourceValue, null) ? null : (string)context.SourceValue;
            DateTime? result = null;
            DateTime reference = new DateTime();

            if (!object.Equals(value, null) && valid.IsValid(value))
            {
                value = Regex.Replace(value, @"^([0-9]{1,2})(/)([0-9]{1,2})(/)([0-9]{1,})$", "$5-$3-$1");
                if (DateTime.TryParse(value, out reference))
                {
                    result = reference;
                }
            }

            return result;
        }
    }
}
