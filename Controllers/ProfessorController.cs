using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class ProfessorController : PadraoController<ProfessorViewModel>
    {
        public ProfessorController()
        {
            DAO = new ProfessorDAO();
            GeraProximoId = true;
        }

        public override IActionResult Save(ProfessorViewModel model, string Operacao)
        {
            try
            {
                ValidaDados(model, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreencheDadosParaView(Operacao, model);
                    return View(NomeViewForm, model);
                }
                else
                {
                    if (Operacao == "I")
                    {
                        DAO.Insert(model);
                        LoginViewModel modelLogin = new LoginViewModel()
                        {
                            ID = model.ID,
                            SenhaHash = "0001"
                        };
                        LoginDAO login = new LoginDAO();
                        login.Insert(modelLogin);

                    }
                    else
                        DAO.Update(model);
                    TempData["AlertMessage"] = "Dado salvo com sucesso...!           ";
                    return RedirectToAction("Create");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected override void ValidaDados(ProfessorViewModel professor, string operacao)
        {
            base.ValidaDados(professor, operacao);
            if (string.IsNullOrEmpty(professor.Nome))
                ModelState.AddModelError("Nome", "Campo obrigatório.");
            if (string.IsNullOrEmpty(professor.Email))
                ModelState.AddModelError("Email", "Campo obrigatório.");
            else
            {
                Regex validaEmailRegex = new Regex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");
                if (!validaEmailRegex.IsMatch(professor.Email))
                    ModelState.AddModelError("Email", "Email Inválido.");
            }
            if (string.IsNullOrEmpty(professor.Telefone))
                ModelState.AddModelError("Telefone", "Campo obrigatório.");
            else
            {
                Regex validaNumeroTelefoneRegex = new Regex("^\\([1-9]{2}\\) (?:[2-8]|9[1-9])[0-9]{3}\\-[0-9]{4}$");
                if (!validaNumeroTelefoneRegex.IsMatch(professor.Telefone))
                    ModelState.AddModelError("Telefone", "Telefone Inválido.");
            }
            if (string.IsNullOrEmpty(professor.CPF))
                ModelState.AddModelError("CPF", "Campo obrigatório.");
            else
            {
                Regex validaNumeroCPFRegex = new Regex("^\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2}$");
                if (!validaNumeroCPFRegex.IsMatch(professor.CPF))
                    ModelState.AddModelError("CPF", "CPF Inválido.");
            }
            //Falta validar se CPF é valido ou não
            // Talvez usar API ou AJAX para consultar na receita federal
        }
    }
}
