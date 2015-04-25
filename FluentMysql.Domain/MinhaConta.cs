using FluentMysql.Domain.ValueObject;
using FluentMysql.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Domain
{
    public class MinhaConta : ContaAcesso
    {
        private static MinhaConta _Instance { get; set; }

        private MinhaConta(Usuario usuario)
            : base(usuario) { }

        public static MinhaConta Instance
        {
            get
            {
                if (object.Equals(_Instance, null))
                    _Instance = new MinhaConta(new Usuario());

                return _Instance;
            }
        }

        public static MinhaConta Factory(Usuario usuario)
        {
            _Instance = new MinhaConta(usuario);
            return _Instance;
        }
    }
}
