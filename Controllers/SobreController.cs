using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TrabalhoInterdisciplinar.Helpers;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class SobreController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                if (HelperControllers.VerificaProfessorLogado(HttpContext.Session))
                    ViewBag.LogadoProfessor = true;
                else if (HelperControllers.VerificaAlunoLogado(HttpContext.Session))
                    ViewBag.LogadoAluno = true;
                return View();
            }
            catch(Exception err)
            {
                return View("Error", new ErrorViewModel(err.ToString()));
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
