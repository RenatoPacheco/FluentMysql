using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.ValueObject
{
    public enum Nivel
    {
        Indefinido,
        Sistema,
        Administrador,
        Operador,
        Usuario,
        Visitante
    }
}
