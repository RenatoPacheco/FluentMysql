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
        public static bool SobreUsuario(Usuario minhaConta, Usuario usuario)
        {
            return SobreUsuario(minhaConta, new List<Usuario>() { usuario }, false);
        }

        public static bool SobreUsuario(Usuario minhaConta, IList<Usuario> usuarios)
        {
            return SobreUsuario(minhaConta, usuarios, false);
        }

        public static bool SobreUsuario(Usuario minhaConta, Usuario usuario, bool afetarAutenticado)
        {
            return SobreUsuario(minhaConta, new List<Usuario>() { usuario }, afetarAutenticado);
        }

        public static bool SobreUsuario(Usuario minhaConta, IList<Usuario> usuarios, bool afetarAutenticado)
        {
            if (object.Equals(minhaConta, null))
                throw new ArgumentNullException("O valor não pode ser nulo", "minhaConta");

            if (object.Equals(usuarios, null) || usuarios.Count <= 0)
                throw new HttpException(404, "O usuário solicitado não foi encontrado");

            PermissaoInfo minhaPermissao = new PermissaoInfo(minhaConta);
            UsuarioInfo usuatioInfo;

            foreach (Usuario usuario in usuarios)
            {
                if (object.Equals(usuario, null) || usuario.Id <= 0)
                    throw new HttpException(404, "O usuário solicitado não foi encontrado");

                usuatioInfo = new UsuarioInfo(usuario, minhaPermissao);

                if (!usuatioInfo.Subordinado)
                    throw new HttpException(406, "Sua permissão não dá acesso a essa ação para este registro");

                if (usuatioInfo.Autenticado && !afetarAutenticado)
                    throw new HttpException(406, "Ação não permitida para registro já autenticado");
            }

            return true;
        }
    }
}
