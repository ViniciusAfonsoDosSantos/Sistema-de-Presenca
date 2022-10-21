using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class SobreController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.LogadoProfessor = HelperControllers.VerificaProfessorLogado(HttpContext.Session);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
