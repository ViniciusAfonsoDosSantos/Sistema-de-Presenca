using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class ConsultaPresencasController:Controller
    {
        protected bool ExigeAutenticacao { get; set; } = true;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (ExigeAutenticacao && !HelperControllers.VerificaProfessorLogado(HttpContext.Session))
                context.Result = RedirectToAction("Index", "Home");
            else
            {
                if (HelperControllers.VerificaProfessorLogado(HttpContext.Session))
                    ViewBag.LogadoProfessor = true;
                else if (HelperControllers.VerificaAlunoLogado(HttpContext.Session))
                    ViewBag.LogadoAluno = true;
                base.OnActionExecuting(context);
            }
        }
        //Verificar se existem novas presenças
        //Pegar o dado e que está no histórico do orion e passar para a tabela de presença no sql (pegar a data e hora desse dado
        //e colocar )
        public IActionResult Index()
        {
            PresencaDAO dao = new PresencaDAO();
            PegaDadosMongoDB();
            var lista = dao.Listagem();
            return View("Index", lista);
        }
        public void PegaDadosMongoDB()
        {
            string conn = "mongodb://helix:H3l1xNG@191.233.28.24:27000/?authMechanism=SCRAM-SHA-1";
            var client = new MongoClient(conn);
            var db = client.GetDatabase("sth_helixiot");
            var entity = db.GetCollection<ComandosModel>("sth_/_urn:ngsi-ld:aluno:043_Aluno");

            //na entidade do aluno, pegar os documentos dos comandos de presença
            //pegar as novas presencas do aluno, ordenar pelas mais recentes
            PresencaDAO dao = new PresencaDAO();
            List<DateTime> listaDatasPresencas = dao.NovasPresencas(31);
            var docs = entity.Find(x => x.attrName == "presenca").ToList();
            docs.Reverse();
            foreach (var doc in docs)
            {
                foreach(var dataPresenca in listaDatasPresencas)
                {
                    if (doc.recvTime.ToString("dd/MM/yyyy HH:mm") == dataPresenca.ToString("dd/MM/yyyy HH:mm"))
                    {
                        return;
                    }
                }
                Console.WriteLine(doc.recvTime.ToString());
                AulaViewModel aula = ObterAulaDia(doc.recvTime);
                if (aula != null)
                {
                    //Achar aluno pelo IdBiometria
                    int idAluno = 31;
                    AtribuiPresencaAluno(idAluno, aula.ID, doc.attrValue, doc.recvTime);
                }
            }
        }

        private void AtribuiPresencaAluno(int idAluno, int codAula, string situacao, DateTime horarioPresenca)
        {
            //verificar ultimas presencas desse aluno;
            PresencaDAO presenca = new PresencaDAO();
            presenca.Insert(new PresencaViewModel { CodAluno = idAluno, CodAula = codAula, Presente = situacao, DataHoraPresenca = horarioPresenca });
        }

        private AulaViewModel ObterAulaDia(DateTime recvTime)
        {
            AulaDAO aula = new AulaDAO();
            return aula.ConsultaPorData(recvTime);
        }
    }
}
