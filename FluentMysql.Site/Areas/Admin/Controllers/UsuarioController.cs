﻿using AutoMapper;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Models.Services;
using FluentMysql.Site.Areas.Admin.ViewsData.Usuario;
using FluentMysql.Site.Filters;
using FluentMysql.Site.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentMysql.Site.Areas.Admin.Controllers
{
    [AuthorizeUser(Nivel = new Nivel[] { Nivel.Operador })]
    public class UsuarioController : Controller
    {
        public ActionResult Index(FiltroForm filtro = null)
        {
            IList<Usuario> lista = new List<Usuario>();

            if (ModelState.IsValid)
            {
                try
                {
                    if (filtro.Acao == "ativar")
                    {
                        UsuarioService.Ativar(filtro.Selecionados, (Usuario)ViewBag.MinhaConta);
                        ViewBag.Mensagem += AlertsMessages.Success("Registro(s) ativado(s) com sucesso");
                        filtro.Selecionados = null;
                    }
                    else if (filtro.Acao == "desativar")
                    {
                        UsuarioService.Desativar(filtro.Selecionados, (Usuario)ViewBag.MinhaConta);
                        ViewBag.Mensagem += AlertsMessages.Success("Registro(s) desativado(s) com sucesso");
                        filtro.Selecionados = null;
                    }
                    else if (filtro.Acao == "remover")
                    {
                        UsuarioService.Excluir(filtro.Selecionados, (Usuario)ViewBag.MinhaConta);
                        ViewBag.Mensagem += AlertsMessages.Success("Registro(s) excluído(s) com sucesso");
                        filtro.Selecionados = null;
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

                lista = UsuarioService.Filtrar(filtro);
            }

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
                    Usuario info = UsuarioService.Inserir(dados, (Usuario)ViewBag.MinhaConta);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro inserido com sucesso");
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

            Usuario info = UsuarioService.Info(id);
            AlteraForm dados = info == null ? new AlteraForm() : Mapper.Map<Usuario, AlteraForm>(info);

            ViewBag.Info = info;

            string view = "Altera";

            if (object.Equals(info, null))
                view = "../Error/404";

            return View(view, dados);
        }

        [HttpPost]
        public ActionResult Altera(AlteraForm dados)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var Usuario = UsuarioService.Alterar(dados, (Usuario)ViewBag.MinhaConta);
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

            Usuario info = UsuarioService.Info(dados.Id);
            ViewBag.Info = info;

            string view = "Altera";

            if (object.Equals(info, null))
                view = "../Error/404";

            return View(view, dados);
        }

        public ActionResult Ativa(IList<long> id)
        {
            try
            {
                UsuarioService.Ativar(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) ativado(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        public ActionResult Desativa(IList<long> id)
        {
            try
            {
                UsuarioService.Desativar(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) desativado(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }

        public ActionResult Exclue(IList<long> id)
        {
            try
            {
                UsuarioService.Excluir(id, (Usuario)ViewBag.MinhaConta);
                TempData["Mensagem"] = AlertsMessages.Success("Registro(s) excluído(s) com sucesso");
            }
            catch (ValidationException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                ViewBag.Mensagem += AlertsMessages.Warning(ex.Message.ToString());
            }

            return RedirectToAction("Index", new { @Voltar = true });
        }
    }
}