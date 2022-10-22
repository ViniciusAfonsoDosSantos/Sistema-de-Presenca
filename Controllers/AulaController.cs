using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class AulaController: PadraoController<AulaViewModel>
    {
        public AulaController()
        {
            DAO = new AulaDAO();
            GeraProximoId = true;
        }

        protected override void ValidaDados(AulaViewModel aula, string operacao)
        {
            base.ValidaDados(aula, operacao);
            if (string.IsNullOrEmpty(aula.Conteudo))
                ModelState.AddModelError("Conteudo", "Campo obrigatório.");
            if (aula.DataHoraAula < DateTime.Now)
                ModelState.AddModelError("DataHoraAula", "Não é permitido criar aulas no passado.");
            if (aula.DataHoraAula > DateTime.MaxValue)
                ModelState.AddModelError("DataHoraAula", "Data inválida.");
            if (aula.CodMateria <= 0)
                ModelState.AddModelError("CodMateria", "Informe a Matéria.");

        }
        protected override void PreencheDadosParaView(string Operacao, AulaViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            model.DataHoraAula = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            PreparaListaProfessoresParaCombo();

        }

        private void PreparaListaProfessoresParaCombo()
        {
            MateriaDAO materiaDao = new MateriaDAO();
            var materias = materiaDao.Listagem();
            List<SelectListItem> listaMaterias = new List<SelectListItem>();

            listaMaterias.Add(new SelectListItem("Selecione uma Matéria...", "0"));
            foreach (var materia in materias)
            {
                SelectListItem item = new SelectListItem(materia.Descricao, materia.ID.ToString());
                listaMaterias.Add(item);
            }
            ViewBag.Materias = listaMaterias;
        }
        
    }
}
