using FluentMysql.Domain.Repository;
using FluentMysql.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Domain.Services
{
    public static class MinhaContaService
    {
        public static Usuario AlterarSenha(Usuario usuario, string senhaAtual, string novaSenha)
        {
            if (object.Equals(usuario, null))
                throw new ArgumentException("Info é obrigatório", "usuario");

            if (string.IsNullOrEmpty(senhaAtual))
                throw new ArgumentException("Valor não pode ser nulo ou vazio", "senhaAtual");

            if (string.IsNullOrEmpty(novaSenha))
                throw new ArgumentException("Valor não pode ser nulo ou vazio", "novaSenha");

            using(UsuarioRepository acao = new UsuarioRepository())
            {
                usuario = acao.Query().Where(x => x.Id == usuario.Id && x.Senha == senhaAtual).FirstOrDefault();

                if (object.Equals(usuario, null))
                    throw new ValidationException("Usuário ou senha inválido");

                usuario.Senha = novaSenha;
                usuario.DataAlteracao = DateTime.Now;
                usuario.Responsavel = usuario;

                acao.Edit(usuario);
            }

            return usuario;
        }
    }
}
