using AutoMapper;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.AutoMapper.TypeConverter
{
    public class EnumStatusToBoolean : ITypeConverter<Status, Boolean>
    {
        public Boolean Convert(ResolutionContext context)
        {
            Status value = (Status)context.SourceValue;

            return (int)value == 1;
        }
    }
}
