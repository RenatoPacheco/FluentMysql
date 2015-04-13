﻿using AutoMapper;
using FluentMysql.Domain.Services;
using FluentMysql.Domain.ValueObject;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Models.Services;
using FluentMysql.Site.Areas.Admin.ViewsData.Usuario;
using FluentMysql.Site.Filters;
using FluentMysql.Site.Helpers;
using FluentMysql.Site.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FluentMysql.Site.Areas.Admin.Controllers
{
    public class teste
    {
        public IList<long> id { get; set; }
    }

    [AuthorizeUser(Nivel = new Nivel[] { Nivel.Administrador })]
    public class UsuarioController : Controller
    {
        public ActionResult Index(FiltroForm filtro = null, bool voltar = false, bool json = false, bool xml = false)
        {
            int index;
            string ids;
            IList<Usuario> lista = new List<Usuario>();
            CommonRouteData routeData = new CommonRouteData(ControllerContext);
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(filtro.Acao)) {
                        if (Regex.IsMatch(filtro.Acao, @"^nivel-[0-9]+$", RegexOptions.IgnoreCase))
                        {
                            index = int.Parse(Regex.Match(filtro.Acao, "[0-9]+$", RegexOptions.IgnoreCase).Value);
                            return RedirectToAction("AlteraNivel", new { @Id = filtro.AlteraUsuarioId[index], @Nivel = filtro.AlteraNivelValor[index] });
                        }
                        else if (filtro.Acao == "ativar")
                        {
                            ids = string.Join("&", filtro.Selecionados.Select(x => string.Format("id={0}", x)));
                            return Redirect(string.Format("Usuario/Ativa/?{0}", ids));
                        }
                        else if (filtro.Acao == "desativar")
                        {
                            ids = string.Join("&", filtro.Selecionados.Select(x => string.Format("id={0}", x)));
                            return Redirect(string.Format("Usuario/Desativa/?{0}", ids));
                        }
                        else if (filtro.Acao == "excluir")
                        {
                            ids = string.Join("&", filtro.Selecionados.Select(x => string.Format("id={0}", x)));
                            return Redirect(string.Format("Usuario/Exclue/?{0}", ids));
                        }
                    }
                }
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
                catch (ArgumentException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }

                if (voltar && !object.Equals(Session[ViewBag.ActionRef], null))
                    filtro = (FiltroForm)Session[ViewBag.ActionRef];

                Session[ViewBag.ActionRef] = filtro;
                lista = Models.Services.UsuarioService.Filtrar(filtro);
            }

            if (xml)
                return ConverteResultadoService.ParaXml(lista, filtro, (string)ViewBag.Memsagem);
            else if (json)
                return ConverteResultadoService.ParaJson(lista, filtro, (string)ViewBag.Memsagem);

            ViewBag.Lista = lista;
            return View(filtro);
        }

        [HttpGet]
        public ActionResult Insere()
        {
            return View(new InsereForm());
        }
        
        [HttpPost]
        public ActionResult Insere(InsereForm dados)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Usuario info = Models.Services.UsuarioService.Inserir(dados, (Usuario)ViewBag.MinhaConta);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro inserido com sucesso");
                    try
                    {
                        Models.Services.MinhaContaService.SolicitarAutenticar(info);
                        TempData["Mensagem"] += AlertsMessages.Success("E-mail de autenticação enviado com sucesso");
                    }
                    catch (Exception ex)
                    {
                        TempData["Mensagem"] += AlertsMessages.Danger(string.Format("<p>Ocorreu um erro ao enviar o e-mail de autenticação: <strong>{0}</strong><p><pre>{1}</pre>", ex.Message.ToString(), ex.StackTrace.ToString()), "div");
                    }
                    
                    return RedirectToAction("Altera", new { @Id = info.Id });
                }
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
                catch (ArgumentException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
            }

            return View(dados);
        }

        [HttpGet]
        public ActionResult Altera(long id = 0)
        {
            Usuario info = Models.Services.UsuarioService.Info(id);

            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            AlteraForm dados = Mapper.Map<Usuario, AlteraForm>(info);
            PermissaoService.SobreUsuario((Usuario)ViewBag.MinhaConta, info);
            
            ViewBag.Info = info;
            return View(dados);
        }
        
        [HttpPost]
        public ActionResult Altera(AlteraForm dados)
        {
            Usuario info = Models.Services.UsuarioService.Info(dados.Id);
            
            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            PermissaoService.SobreUsuario((Usuario)ViewBag.MinhaConta, info);

            if (ModelState.IsValid)
            {
                try
                {
                    var Usuario = Models.Services.UsuarioService.Alterar(dados, (Usuario)ViewBag.MinhaConta);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro alterado com sucesso");
                    return RedirectToAction("Altera", new { @Id = Usuario.Id });
                }
                catch (ValidationException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
                catch (ArgumentException ex)
                {
                    ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
                }
            }

            ViewBag.Info = info;
            return View(dados);
        }

        [HttpGet]
        public ActionResult Info(long id = 0)
        {
            Usuario info = Models.Services.UsuarioService.Info(id);
            
            if (object.Equals(info, null) || info.Id <= 0)
                throw new HttpException(404, "O registro solicitado não foi encontrado");

            return View(info);
        }
        
        [HttpGet]
        public ActionResult SolicitaAutenticacao(long id = 0)
        {
            try
            {
                Usuario usuario = Models.Services.UsuarioService.Info(id);

                if (object.Equals(usuario, null) || usuario.Id <= 0)
                    throw new HttpException(404, "O registro solicitado não foi encontrado");
                
                Models.Services.MinhaContaService.SolicitarAutenticar(usuario);                
                TempData["Mensagem"] = AlertsMessages.Success("E-mail solicitando autenticação enviado com sucesso");
            }
            catch (ValidationException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(string.Format("Atenção: <strong>{0}</strong>", ex.Message.ToString()));
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = AlertsMessages.Danger(string.Format("<p>Ocorreu um erro ao enviar o e-mail de autenticação: <strong>{0}</strong><p><pre>{1}</pre>", ex.Message.ToString(), ex.StackTrace.ToString()), "div");
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }
        
        public ActionResult Ativa(IList<long> id)
        {            
            try
            {
                IList<Usuario> usuarios = Models.Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario((Usuario)ViewBag.MinhaConta, usuarios, true);

                Models.Services.UsuarioService.Ativar(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) ativado(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (HttpException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        public ActionResult Desativa(IList<long> id)
        {
            try
            {
                IList<Usuario> usuarios = Models.Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario((Usuario)ViewBag.MinhaConta, usuarios, true);

                Models.Services.UsuarioService.Desativar(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) desativado(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (HttpException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        public ActionResult Exclue(IList<long> id)
        {
            try
            {
                IList<Usuario> usuarios = Models.Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario((Usuario)ViewBag.MinhaConta, usuarios, true);

                Models.Services.UsuarioService.Excluir(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) excluído(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (HttpException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        public ActionResult AlteraNivel(IList<long> id, Nivel nivel)
        {
            try
            {
                Usuario minhaConta = (Usuario)ViewBag.MinhaConta;
                IList<Usuario> usuarios = Models.Services.UsuarioService.Info(id);
                PermissaoService.SobreUsuario(minhaConta, usuarios, true);


                if (nivel == Nivel.Indefinido)
                    throw new ValidationException("Um valor de nível não foi selecionado");

                Usuario modelo = (Usuario)minhaConta.Clone();
                modelo.Nivel = nivel;
                UsuarioInfo info = new UsuarioInfo(modelo, new UsuarioPermissao(minhaConta));
                if (!info.Subordinado)
                    throw new ValidationException("Seu nível de acesso não permite aplicar esse nível de acesso");

                Models.Services.UsuarioService.AlterarNivel(id, nivel, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Nível dos registro(s) alterados(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (HttpException ex)
            {
                TempData["Mensagem"] = AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }
    }
}