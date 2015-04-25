using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Domain.ValueObject
{
    public class ContaAcesso
    {        
        public ContaAcesso(Usuario usuario)
        {
            this.Info = object.Equals(usuario, null) ? new Usuario() : usuario;
            this.Acesso = new NivelAcesso(usuario);
            this.Autenticado = !object.Equals(usuario, null)
                && !string.IsNullOrWhiteSpace(usuario.CPF)
                && !string.IsNullOrWhiteSpace(usuario.Senha)
                && !string.IsNullOrWhiteSpace(usuario.Login);
        }

        public Usuario Info { get; private set; }

        public NivelAcesso Acesso { get; private set; }

        public virtual bool Autenticado { get; protected set; }
    }
}
