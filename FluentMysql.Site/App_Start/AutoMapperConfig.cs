using AutoMapper;
using FluentMysql.Infrastructure.AutoMapper.TypeConverter;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site
{
    public class AutoMapperConfig
    {
        public static void RegisterAutoMappers()
        {
            Mapper.CreateMap<string, DateTime>().ConvertUsing(new StringToDate());
            Mapper.CreateMap<string, DateTime?>().ConvertUsing(new StringToDateNullable());
            Mapper.CreateMap<DateTime, string>().ConvertUsing(new DateToString());
            Mapper.CreateMap<DateTime?, string>().ConvertUsing(new DateNullableToString());
            Mapper.CreateMap<Boolean, char>().ConvertUsing(new BooleanToChar());
            Mapper.CreateMap<char, Boolean>().ConvertUsing(new CharToBoolean());
            Mapper.CreateMap<Boolean, Status>().ConvertUsing(new BooleanToEnumStatus());
            Mapper.CreateMap<Status, Boolean>().ConvertUsing(new EnumStatusToBoolean());
        }
    }
}
