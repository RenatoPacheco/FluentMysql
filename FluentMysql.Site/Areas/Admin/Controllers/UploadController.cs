using AutoMapper;
using FluentMysql.Infrastructure.Entities;
using FluentMysql.Infrastructure.ValueObject;
using FluentMysql.Site.Areas.Admin.Services;
using FluentMysql.Site.Areas.Admin.ViewsData.Upload;
using FluentMysql.Site.Filters;
using FluentMysql.Site.Helpers;
using FluentMysql.Site.ValueObject;
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
    public class UploadController : Controller
    {
        public ActionResult CKEditor(CKEditorForm dados = null)
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
    }
}