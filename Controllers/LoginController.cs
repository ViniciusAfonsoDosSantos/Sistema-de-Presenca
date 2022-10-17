using Microsoft.AspNetCore.Http;
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

        public IActionResult FazLogin(string ID, string SenhaHash)
        {
            if(DAO.Consulta(Convert.ToInt32(ID)) == null)
            {
                TempData["LoginMessage"] = "Usuário ou senha inválidos!";
                return RedirectToAction("index", "Home");
            }

            if (Convert.ToInt32(ID) == DAO.Consulta(Convert.ToInt32(ID)).ID && 
                SenhaHash == DAO.Consulta(Convert.ToInt32(ID)).SenhaHash)
            {
                HttpContext.Session.SetString("Logado", "true");
                return RedirectToAction("index", "Home");
            }
            else
            {
                TempData["LoginMessage"] = "Usuário ou senha inválidos!";
                return RedirectToAction("index", "Home");
            }
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "Home");
        }


    }
}
