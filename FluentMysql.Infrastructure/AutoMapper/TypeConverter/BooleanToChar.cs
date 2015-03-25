using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.AutoMapper.TypeConverter
{
    public class BooleanToChar : ITypeConverter<Boolean, char>
    {
        public char Convert(ResolutionContext context)
        {
            Boolean value = (Boolean)context.SourceValue;

            return value ? '1' : '0';
        }
    }
}
