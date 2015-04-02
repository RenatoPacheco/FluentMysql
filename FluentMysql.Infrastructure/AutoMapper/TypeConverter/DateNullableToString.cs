using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.AutoMapper.TypeConverter
{
    public class DateNullableToString : ITypeConverter<DateTime?, string>
    {
        public string Convert(ResolutionContext context)
        {
            DateTime? value = (DateTime?)context.SourceValue;
            string result = null;
            StringBuilder creat = new StringBuilder();

            if(!object.Equals(value, null))
            {
                creat.Append(string.Format("{0}{1}/", value.Value.Day >= 10 ? string.Empty : "0", value.Value.Day));
                creat.Append(string.Format("{0}{1}/", value.Value.Month >= 10 ? string.Empty : "0", value.Value.Month));
                creat.Append(string.Format("{0} ", value.Value.Year));
                creat.Append(string.Format("{0}{1}:", value.Value.Hour >= 10 ? string.Empty : "0", value.Value.Hour));
                creat.Append(string.Format("{0}{1}:", value.Value.Minute >= 10 ? string.Empty : "0", value.Value.Minute));
                creat.Append(string.Format("{0}{1}", value.Value.Second >= 10 ? string.Empty : "0", value.Value.Second));
                result = creat.ToString();
            }

            return result;
        }
    }
}
