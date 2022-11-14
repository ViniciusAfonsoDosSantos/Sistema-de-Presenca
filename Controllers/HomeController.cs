using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Helpers;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            HelperDAO.StringCX = _config.GetConnectionString("SQLServerDbConnectionString");
        }

        public IActionResult Index()
        {
            //TestaMongoDB();
            if (HelperControllers.VerificaProfessorLogado(HttpContext.Session))
                ViewBag.LogadoProfessor = true;
            else if (HelperControllers.VerificaAlunoLogado(HttpContext.Session))
                ViewBag.LogadoAluno = true;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*
        public void TestaMongoDB()
        {
            var client = new RestClient("http://191.233.28.24:1026/v2/entities/urn:ngsi-ld:aluno:040/attrs/presenca");
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("fiware-service", "helixiot");
            request.AddHeader("fiware-servicepath", "/");
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);
            string teste = response.Content;
            string presente = teste

        }
        */
    }
}
