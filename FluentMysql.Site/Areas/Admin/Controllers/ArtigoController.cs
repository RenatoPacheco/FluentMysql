using AutoMapper;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Models.Services;
using FluentMysql.Site.Areas.Admin.ViewsData.Artigo;
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
    public class ArtigoController : Controller
    {
        public ActionResult Upload(UploadForm dados = null)
        {
            if (ModelState.IsValid)
            {
                string dir = string.Format("/Images/{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                
                if (!Directory.Exists(Server.MapPath(string.Format("~{0}", dir))))
                        Directory.CreateDirectory(Server.MapPath(string.Format("~{0}", dir)));
                
                dados.Upload.SaveAs(Server.MapPath(string.Format("~{0}/{1}", dir, dados.Upload.FileName)));

                ViewBag.Url = string.Format("{0}/{1}", dir, dados.Upload.FileName);
                ViewBag.Mensagem = "Arquivo enviado com sucesso";
            }
            else
            {
                ViewBag.Url = "";
                ViewBag.Mensagem = "Ocorreu um erro ao enviar o arquivo";
            }

            return View(dados);
        }

        public ActionResult Index(FiltroForm filtro = null)
        {
            IList<Artigo> lista = new List<Artigo>();

            if (ModelState.IsValid)
            {
                try
                {
                    if (filtro.Acao == "ativar")
                    {
                        ArtigoService.Ativar(filtro.Selecionados, (Usuario)ViewBag.MinhaConta);
                        ViewBag.Mensagem += AlertsMessages.Success("Registro(s) ativado(s) com sucesso");
                        filtro.Selecionados = null;
                    }
                    else if (filtro.Acao == "desativar")
                    {
                        ArtigoService.Desativar(filtro.Selecionados, (Usuario)ViewBag.MinhaConta);
                        ViewBag.Mensagem += AlertsMessages.Success("Registro(s) desativado(s) com sucesso");
                        filtro.Selecionados = null;
                    }
                    else if (filtro.Acao == "remover")
                    {
                        ArtigoService.Excluir(filtro.Selecionados, (Usuario)ViewBag.MinhaConta);
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

                lista = ArtigoService.Filtrar(filtro);
            }

            ViewBag.Lista = lista;
            return View(filtro);
        }

        [HttpGet]
        public ActionResult Insere()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insere(InsereForm dados)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Artigo info = ArtigoService.Inserir(dados, (Usuario)ViewBag.MinhaConta);
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

            Artigo info = ArtigoService.Info(id);
            AlteraForm dados = info == null ? new AlteraForm() : Mapper.Map<Artigo, AlteraForm>(info);

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
                    var Artigo = ArtigoService.Alterar(dados, (Usuario)ViewBag.MinhaConta);
                    TempData["Mensagem"] = AlertsMessages.Success("Registro alterado com sucesso");
                    return RedirectToAction("Altera", new { @Id = Artigo.Id });
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

            Artigo info = ArtigoService.Info(dados.Id);
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
                ArtigoService.Ativar(id, (Usuario)ViewBag.MinhaConta);
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
                ArtigoService.Desativar(id, (Usuario)ViewBag.MinhaConta);
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
                ArtigoService.Excluir(id, (Usuario)ViewBag.MinhaConta);
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