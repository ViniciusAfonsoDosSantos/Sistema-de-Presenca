using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System;
using TrabalhoInterdisciplinar.Models;
using TrabalhoInterdisciplinar.DAO;
using Microsoft.AspNetCore.Mvc.Filters;
using TrabalhoInterdisciplinar.Enumeradores;

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

        public IActionResult ObtemDadosConsultaAvancada(EnumTipoTabela tipoTabela, int codigo)
        {
            try
            {
                int tipo = (int)tipoTabela;
                switch (tipoTabela)
                {
                    case EnumTipoTabela.Aluno:
                        return ConsultaAvancadaAluno(codigo);
                    case EnumTipoTabela.Professor:
                        return ConsultaAvancadaProfessor(codigo);
                    case EnumTipoTabela.Materia:
                        return ConsultaAvancadaMateria(codigo);
                    case EnumTipoTabela.Aula:
                        return ConsultaAvancadaAula(codigo);
                    default:
                        throw new Exception("A opção não existe");
                }
            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }

        private IActionResult ConsultaAvancadaMateria(int codigo)
        {
            MateriaDAO dao = new MateriaDAO();
            if (string.IsNullOrEmpty(codigo.ToString()))
                codigo = 0;
            var lista = dao.ConsultaAvancada(codigo);
            return PartialView("pvGridMateria", lista);
        }

        private IActionResult ConsultaAvancadaProfessor(int codigo)
        {
            ProfessorDAO dao = new ProfessorDAO();
            if (string.IsNullOrEmpty(codigo.ToString()))
                codigo = 0;
            var lista = dao.ConsultaAvancada(codigo);
            return PartialView("pvGridProfessor", lista);
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

        public IActionResult ConsultaAvancadaAula(int codigo)
        {
            try
            {

                AulaDAO dao = new AulaDAO();
                if (string.IsNullOrEmpty(codigo.ToString()))
                    codigo = 0;
                var lista = dao.ConsultaAvancada(codigo);
                return PartialView("pvGridAula", lista);
            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }
    }
}
