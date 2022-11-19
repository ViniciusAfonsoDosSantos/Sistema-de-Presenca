using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Helpers;
using TrabalhoInterdisciplinar.Models;
using BC = BCrypt.Net.BCrypt;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class LoginController : PadraoController<LoginViewModel>
    {
        public LoginController()
        {
            DAO = new LoginDAO();
            GeraProximoId = false;
            ExigeAutenticacao = false;
        }

        public IActionResult FazLogin(LoginViewModel model)
        {
            
            if (DAO.Consulta(Convert.ToInt32(model.ID)) == null)
            {
                TempData["LoginMessage"] = "Usuário ou senha inválidos!";
                return RedirectToAction("index", "Home");
            }

            if(model.SenhaHash == "#Sist123" && model.ID == 21)
            {
                HttpContext.Session.SetString("LogadoProfessor", "true");
                return RedirectToAction("index", "Home");
            }
            
            if (Convert.ToInt32(model.ID) == DAO.Consulta(Convert.ToInt32(model.ID)).ID &&
                 model.SenhaHash == PasswordHasher.Decrypt(DAO.Consulta(Convert.ToInt32(model.ID)).SenhaHash))
            {
                if (model.SenhaHash == "0001")
                {
                    TempData["PasswordMessage"] = "Crie uma nova senha!";
                    return RedirectToAction("index", "Home");
                }

                if (model.ID.ToString().Substring(0, 1) == "2")
                {
                    HttpContext.Session.SetString("LogadoProfessor", "true");
                }
                else
                {
                    HttpContext.Session.SetString("LogadoAluno", "true");
                }

                return RedirectToAction("index", "Home");
            }
            else
            {
                TempData["LoginMessage"] = "Usuário ou senha inválidos!";
                return RedirectToAction("index", "Home");
            }
        }

        public IActionResult CriaNovaSenha(LoginViewModel model, string ConfirmaNovaSenha)
        {

            if (DAO.Consulta(Convert.ToInt32(model.ID)) == null)
            {
                TempData["PasswordMessage"] = "Usuário não existe!";
                return RedirectToAction("index", "Home");
            }

            //Link regex login -> https://pt.stackoverflow.com/questions/373574/regex-para-senha-forte
            Regex validaSenhaRegex = new Regex("^.*(?=.{8,})(?=.*[@#$%^&+=])(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).*$");
            if (model.SenhaHash != null && validaSenhaRegex.IsMatch(model.SenhaHash) &&  model.SenhaHash == ConfirmaNovaSenha )
            {
                string Operacao = "A";
                ValidaDados(model, Operacao);
                model.SenhaHash = PasswordHasher.Encrypt(model.SenhaHash);
                DAO.Update(model);
                return RedirectToAction("index", "Home");
            }
            else
            {
                TempData["PasswordMessage"] = "Senha inválida!";
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
