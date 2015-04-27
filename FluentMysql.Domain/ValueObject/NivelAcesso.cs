using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Domain.ValueObject
{
    public class NivelAcesso
    {
        public virtual bool Operador { get; protected set; }

        public virtual bool Usuario { get; protected set; }

        public virtual bool Visitante { get; protected set; }

        public virtual bool Administrador { get; protected set; }

        public virtual bool Sistema { get; protected set; }

        public virtual Nivel Nivel { get; protected set; }

        private int NivelInt { get; set; }

        public bool AutorizadoSobre(Usuario usuario)
        {
            if (object.Equals(usuario, null))
                throw new ArgumentNullException("usuario", string.Format("Valor de {0} não pode ser nulo", typeof(Usuario)));

            return this.Administrador && (this.NivelInt < (int)usuario.Nivel || (int)usuario.Nivel <= 0);
        }

        public bool AutorizadoSobre(Nivel nivel)
        {
            int indefindo = (int)Nivel.Indefinido;
            int compara = (int)nivel;
            return this.NivelInt > indefindo && (this.NivelInt <= compara || compara <= indefindo);
        }

        public NivelAcesso(Usuario usuario)
        {
            if (!object.Equals(usuario, null))
            {
                this.Nivel = usuario.Nivel;
                this.NivelInt = (int)usuario.Nivel;
                if (NivelInt > (int)Nivel.Indefinido)
                {
                    this.Visitante = this.AutorizadoSobre(Nivel.Visitante);
                    this.Usuario = this.AutorizadoSobre(Nivel.Usuario);
                    this.Operador = this.AutorizadoSobre(Nivel.Operador);
                    this.Administrador = this.AutorizadoSobre(Nivel.Administrador);
                    this.Sistema = this.AutorizadoSobre(Nivel.Sistema);
                }
            }
        }
    }
}
