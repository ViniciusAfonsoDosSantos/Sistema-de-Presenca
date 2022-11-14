using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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

        //Verificar se existem novas presenças
        //Pegar o dado e que está no histórico do orion e passar para a tabela de presença no sql (pegar a data e hora desse dado
        //e colocar )
        public void TestingMongoDB()
        {
            string conn = "";
            var client = new MongoClient(conn);
            var db = client.GetDatabase("sth_helixiot");
            var entity = db.GetCollection<ComandosModel>("sth_/_urn:ngsi-ld:aluno:043_Aluno");

            //na entidade do aluno, pegar os documentos dos comandos de presença
            var docs = entity.Find(x => x.attrName=="presenca").ToList();
            foreach (var doc in docs)
            {
                //filtrar a aula da presença e colocar a no banco do SQL
                Console.WriteLine(doc.recvTime.ToString());
                //aula sempre começa 19:15 ou 21:05, pegar o que está mais próximo
            }
        }
    }
}
