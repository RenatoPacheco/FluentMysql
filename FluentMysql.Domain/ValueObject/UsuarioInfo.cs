using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Domain.ValueObject
{
    public class UsuarioInfo
    {
        public virtual bool Autenticado { get; protected set; }

        public virtual bool Subordinado { get; protected set; }

        public UsuarioInfo(Usuario usuario)
            : this(usuario, new UsuarioPermissao(usuario)) { }

        public UsuarioInfo(Usuario usuario, UsuarioPermissao responsavel)
        {
            Autenticado = !string.IsNullOrWhiteSpace(usuario.CPF)
                && !string.IsNullOrWhiteSpace(usuario.Senha)
                && !string.IsNullOrWhiteSpace(usuario.Login);

            Subordinado = (int)responsavel.Nivel > (int)Nivel.Indefinido
                && responsavel.Operador
                && (int)responsavel.Nivel < (int)usuario.Nivel;
        }
    }
}
