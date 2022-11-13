using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrabalhoInterdisciplinar.DAO;
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
            TestingMongoDB();
            //ViewBag.LogadoProfessor = HelperControllers.VerificaProfessorLogado(HttpContext.Session);
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
        public async void TestingMongoDB()
        {
            string conn = "mongodb://helix:H3l1xNG@20.195.194.68:27000/?authMechanism=SCRAM-SHA-1";
            var client = new MongoClient(conn);
            var db = client.GetDatabase("sth_helixiot");
            var entity = db.GetCollection<BsonDocument>("sth_/_urn:ngsi-ld:aluno:043_Aluno").DocumentSerializer.Deserialize(new MongoDB.Bson.Serialization.BsonDeserializationContext());

            var values = await MongoCollection.
            //using (BsonReader reader = new BsonReader(entity))
            //{
            //    JsonSerializer serializer = new JsonSerializer();
            //    ComandoViewModel e = serializer.Deserialize<ComandoViewModel>(reader);
            //    Console.WriteLine(e._id);
            //    // Movie Premiere
            //}
            //List<ComandoViewModel> list = entity.Find(_ => true).ToList();
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item);
            //}
            //int id = elements[0].Value.ToString().IndexOf("aluno:0");
            //var aluno = elements[0].Value.ToString().Substring(id + 7, 2);
            //var presenca = elements[2].Value.AsBsonDocument.Elements.ToList()[1].Value.AsBsonDocument.Elements.ToList()[3].Value.ToString();
            //ver se os dados do historico estao no mesmo modelo, fazer a mesma coisa para pegar a data
            //Pegar a data, procurar uma aula com o mesmo DateTime e pegar esse valor --> já tenho id do aluno, presenca e a aula, só colocar na tabela
        }
    }
}
