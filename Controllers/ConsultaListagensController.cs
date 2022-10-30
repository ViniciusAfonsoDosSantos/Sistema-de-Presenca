using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System;
using TrabalhoInterdisciplinar.Models;
using TrabalhoInterdisciplinar.DAO;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class ConsultaListagensController : Controller
    {
        protected bool ExigeAutenticacao { get; set; } = true;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (ExigeAutenticacao && !HelperControllers.VerificaProfessorLogado(HttpContext.Session))
                context.Result = RedirectToAction("Index", "Home");
            else
            {
                ViewBag.LogadoProfessor = true;
                base.OnActionExecuting(context);
            }
        }

        public IActionResult Index()
        {
            try
            {
                return View("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }
        }

        public IActionResult ObtemDadosConsultaAvancada(int codigo, int tipoTabela)
        {
            try
            {
                switch (tipoTabela)
                {
                    case 1:
                        return ConsultaAvancadaAluno(codigo);
                    //case 2:
                    //    return ConsultaAvancadaProfessor(codigo);
                    //case 3:
                    //    return ConsultaAvancadaMateria(codigo);
                    //case 4:
                    //    return ConsultaAvancadaProfessor(codigo);
                    default:
                        throw new Exception("A opção não existe");
                        
                }

            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }

        public IActionResult ConsultaAvancadaAluno(int codigo)
        {
            try
            {

                AlunoDAO dao = new AlunoDAO();
                if (string.IsNullOrEmpty(codigo.ToString()))
                    codigo = 0;
                var lista = dao.ConsultaAvancada(codigo);
                return PartialView("pvGridAluno", lista);
            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }

        //public IActionResult ConsultaAvancadaProfessor(int codigo)
        //{
        //    try
        //    {

        //        ProfessorDAO dao = new ProfessorDAO();
        //        if (string.IsNullOrEmpty(codigo.ToString()))
        //            codigo = 0;
        //        var lista = dao.ConsultaAvancada(codigo);
        //        return PartialView("pvGridProfessor", lista);
        //    }
        //    catch (Exception erro)
        //    {
        //        return Json(new { erro = true, msg = erro.Message });
        //    }
        //}

        //public IActionResult ConsultaAvancadaMateria(int codigo)
        //{
        //    try
        //    {

        //        MateriaDAO dao = new MateriaDAO();
        //        if (string.IsNullOrEmpty(codigo.ToString()))
        //            codigo = 0;
        //        var lista = dao.ConsultaAvancada(codigo);
        //        return PartialView("pvGridMateria", lista);
        //    }
        //    catch (Exception erro)
        //    {
        //        return Json(new { erro = true, msg = erro.Message });
        //    }
        //}

        //public IActionResult ConsultaAvancadaAula(int codigo)
        //{
        //    try
        //    {

        //        AulaDAO dao = new AulaDAO();
        //        if (string.IsNullOrEmpty(codigo.ToString()))
        //            codigo = 0;
        //        var lista = dao.ConsultaAvancada(codigo);
        //        return PartialView("pvGridAula", lista);
        //    }
        //    catch (Exception erro)
        //    {
        //        return Json(new { erro = true, msg = erro.Message });
        //    }
        //}

    }
}
