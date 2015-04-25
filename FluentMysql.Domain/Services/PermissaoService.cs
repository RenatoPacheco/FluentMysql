using FluentMysql.Domain.ValueObject;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FluentMysql.Domain.Services
{
    public static class PermissaoService
    {
        public static bool SobreUsuario(Usuario usuario)
        {
            return SobreUsuario(new List<Usuario>() { usuario }, false);
        }

        public static bool SobreUsuario(IList<Usuario> usuarios)
        {
            return SobreUsuario(usuarios, false);
        }

        public static bool SobreUsuario(Usuario usuario, bool afetarAutenticado)
        {
            return SobreUsuario(new List<Usuario>() { usuario }, afetarAutenticado);
        }

        public static bool SobreUsuario(IList<Usuario> usuarios, bool afetarAutenticado)
        {
            if (object.Equals(usuarios, null) || usuarios.Count <= 0)
                throw new HttpException(404, "O usuário solicitado não foi encontrado");

            ContaAcesso acesso;

            foreach (Usuario usuario in usuarios)
            {
                if (object.Equals(usuario, null) || usuario.Id <= 0)
                    throw new HttpException(404, "O usuário solicitado não foi encontrado");

                acesso = new ContaAcesso(usuario);

                if (!MinhaConta.Instance.Acesso.AutorizadoSobre(acesso.Info))
                    throw new HttpException(406, "Sua permissão não dá acesso a essa ação para este registro");

                if (acesso.Autenticado && !afetarAutenticado)
                    throw new HttpException(406, "Ação não permitida para registro já autenticado");
            }

            return true;
        }
    }
}
