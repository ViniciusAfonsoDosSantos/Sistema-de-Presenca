using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class MateriaController : PadraoController<MateriaViewModel>
    {
        public MateriaController()
        {
            DAO = new MateriaDAO();
            GeraProximoId = true;
        }

        protected override void ValidaDados(MateriaViewModel materia, string operacao)
        {
            base.ValidaDados(materia, operacao);
            if (string.IsNullOrEmpty(materia.Descricao))
                ModelState.AddModelError("Descricao", "Campo obrigatório.");
            if (materia.CargaHoraria <= 0)
                ModelState.AddModelError("CargaHoraria", "Campo obrigatório.");
            if (materia.CodProfessor <= 0)
                ModelState.AddModelError("CodProfessor", "Informe o Professor.");

        }
        protected override void PreencheDadosParaView(string Operacao, MateriaViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            PreparaListaProfessoresParaCombo();

        }

        private void PreparaListaProfessoresParaCombo()
        {

            ProfessorDAO professorDao = new ProfessorDAO();
            var professores = professorDao.Listagem();
            List<SelectListItem> listaProfessores = new List<SelectListItem>();

            listaProfessores.Add(new SelectListItem("Selecione um Professor...", "0"));
            foreach (var professor in professores)
            {
                SelectListItem item = new SelectListItem(professor.Nome, professor.ID.ToString());
                listaProfessores.Add(item);
            }
            ViewBag.Professores = listaProfessores;
        }

        public override IActionResult Delete(int id)
        {
            try
            {
                AulaDAO aula = new AulaDAO();
                foreach (var item in aula.Listagem())
                    if (item.CodMateria == id)
                    {
                        TempData["AlertMessage"] = "Não foi possivel deletar. Matéria possui aulas em aberto.";
                        return RedirectToAction("Index", "ConsultaListagens");
                    }
                DAO.Delete(id);
                return RedirectToAction("Index", "ConsultaListagens");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

    }
}
