using AutoMapper;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.AutoMapper.TypeConverter
{
    public class BooleanToEnumStatus : ITypeConverter<Boolean, Status>
    {
        public Status Convert(ResolutionContext context)
        {
            Boolean value = (Boolean)context.SourceValue;

            return value ? (Status)1 : (Status)0;
        }
    }
}
