using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class ConsultaAulasController: Controller
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
                PreparaDadosParaFiltros();
                return View("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }
        }

        public IActionResult ConsultaAulaAvancada(DateTime dataInicial, DateTime dataFinal, int idMateria)
        {
            AulaDAO aulaDAO = new AulaDAO();
            if (string.IsNullOrEmpty(idMateria.ToString()))
                idMateria = 0;
            var lista = aulaDAO.ConsultaAvancada(dataInicial, dataFinal, idMateria);
            return PartialView("pvGridAula", lista);
        }
         
        private void PreparaDadosParaFiltros()
        {
            MateriaDAO materiaDAO = new MateriaDAO();
            var  materias = materiaDAO.Listagem();
            List<SelectListItem> listaMaterias = new List<SelectListItem>();
            listaMaterias.Add(new SelectListItem("Selecione uma matéria...", "0"));
            foreach (var materia in materias)
            {
                SelectListItem item = new SelectListItem(materia.Descricao, materia.ID.ToString());
                listaMaterias.Add(item);
            }
            ViewBag.Materias = listaMaterias;
        }
    }
}
