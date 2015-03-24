using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.AutoMapper.TypeConverter
{
    public class DateToString : ITypeConverter<DateTime, string>
    {
        public string Convert(ResolutionContext context)
        {
            DateTime value = (DateTime)context.SourceValue;
            StringBuilder result = new StringBuilder();
            
            result.Append(string.Format("{0}{1}/", value.Day >= 10 ? string.Empty : "0", value.Day));
            result.Append(string.Format("{0}{1}/", value.Month >= 10 ? string.Empty : "0", value.Month));
            result.Append(string.Format("{0} ", value.Year));
            result.Append(string.Format("{0}{1}:", value.Hour >= 10 ? string.Empty : "0", value.Hour));
            result.Append(string.Format("{0}{1}:", value.Minute >= 10 ? string.Empty : "0", value.Minute));
            result.Append(string.Format("{0}{1}", value.Second >= 10 ? string.Empty : "0", value.Second));

            return result.ToString();
        }
    }
}
