using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.Interfaces
{
    public interface IEntities
    {
        long Id { get; set; }

        Guid Guid { get; set; }

        DateTime DataCriacao { get; set; }

        DateTime DataAlteracao { get; set; }

        Status Status { get; set; }

        Usuario Responsavel { get; set; }
    }
}
