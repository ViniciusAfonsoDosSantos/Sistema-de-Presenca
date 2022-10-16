using Microsoft.AspNetCore.Mvc;
using System;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Helpers;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class LoginController : PadraoController<LoginViewModel>
    {
        public LoginController()
        {
            DAO = new LoginDAO();
            GeraProximoId = false;
        }

        public override IActionResult Save(LoginViewModel model, string Operacao)
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
                    model.SenhaHash = PasswordHasher.HashPassword(model.SenhaHash);
                    if (Operacao == "I")
                    {
                        DAO.Insert(model);
                    }
                    else
                    {
                        DAO.Update(model);
                    }
                    TempData["AlertMessage"] = "Dado salvo com sucesso...!           ";
                    return RedirectToAction("Create"); //redirecionar para outra view em outro controller
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }


    }
}
