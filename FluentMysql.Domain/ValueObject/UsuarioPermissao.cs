using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Domain.ValueObject
{
    public class UsuarioPermissao
    {
        public virtual bool Operador { get; protected set; }

        public virtual bool Usuario { get; protected set; }

        public virtual bool Administrador { get; protected set; }

        public virtual bool Sistema { get; protected set; }
        
        public virtual Nivel Nivel { get; protected set; }

        public UsuarioPermissao(Usuario usuario)
        {
            int nivel = (int)usuario.Nivel;
            Nivel = usuario.Nivel;
            if(nivel > (int)Nivel.Indefinido)
            {
                Usuario = nivel <= (int)Nivel.Usuario;
                Operador = nivel <= (int)Nivel.Operador;
                Administrador = nivel <= (int)Nivel.Administrador;
                Sistema = nivel <= (int)Nivel.Sistema;
            }
        }
    }
}
