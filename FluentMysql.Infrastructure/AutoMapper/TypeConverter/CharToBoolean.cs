using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.AutoMapper.TypeConverter
{
    public class CharToBoolean : ITypeConverter<char, Boolean>
    {
        public Boolean Convert(ResolutionContext context)
        {
            char value = (char)context.SourceValue;

            return value == '1';
        }
    }
}
