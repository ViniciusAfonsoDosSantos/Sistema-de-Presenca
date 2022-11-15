using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Enumeradores;
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
            //PresencaDAO dao = new PresencaDAO();
            //PegaDadosMongoDB();
            //var lista = dao.Listagem();
            PreparaDadosParaFiltros();
            return View("Index");
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
            List<string> listaDatasPresencas = dao.NovasPresencas(31);
            var docs = entity.Find(x => x.attrName == "presenca").ToList();
            docs.Reverse();
            foreach (var doc in docs)
            {
                if (listaDatasPresencas.Contains(doc.recvTime.ToString("dd/MM/yyyy HH:mm")))
                {
                    return;
                }
                ColocarPresencaSQL(doc);
            }
        }

        private void ColocarPresencaSQL(ComandosModel doc)
        {
            AulaViewModel aula = ObterAulaDia(doc.recvTime);
            if (aula != null)
            {
                //Achar aluno pelo IdBiometria
                int idAluno = 31;
                AtribuiPresencaAluno(idAluno, aula.ID, doc.attrValue, doc.recvTime);
            }
        }

        private void AtribuiPresencaAluno(int idAluno, int codAula, string situacao, DateTime horarioPresenca)
        {
            //verificar ultimas presencas desse aluno;
            //try
            //{
                PresencaDAO presenca = new PresencaDAO();
                presenca.Insert(new PresencaViewModel { CodAluno = idAluno, CodAula = codAula, Presente = situacao, DataHoraPresenca = horarioPresenca });
            //}
            //catch (Exception)
            //{

            //}
            //exception aqui, preciso fazer todo o caminho dos métodos para retornar à página de erro
        }

        private AulaViewModel ObterAulaDia(DateTime recvTime)
        {
            AulaDAO aula = new AulaDAO();
            return aula.ConsultaPorData(recvTime);
        }

        public IActionResult ConsultaPresencaAvancada(int idAluno, int idAula)
        {
            PresencaDAO presencaDAO = new PresencaDAO();
            
            if (string.IsNullOrEmpty(idAluno.ToString()))
            {
                idAluno = 0;
            }
            if (string.IsNullOrEmpty(idAula.ToString()))
            {
                idAula = 0;
            }
            var lista = presencaDAO.ConsultaAvancada(idAluno, idAula);
            return PartialView("pvGridPresenca", lista);

        }
        private void PreparaDadosParaFiltros()
        {
            AulaDAO aulaDAO = new AulaDAO();
            var aulas = aulaDAO.Listagem();

            AlunoDAO alunoDAO = new AlunoDAO();
            var alunos = alunoDAO.Listagem();

            List<SelectListItem> listaAulas = new List<SelectListItem>();
            listaAulas.Add(new SelectListItem("Selecione uma aula...", "0"));
            foreach (var aula in aulas)
            {
                MateriaDAO materiaDAO = new MateriaDAO();
                SelectListItem item = new SelectListItem($"{aula.Conteudo} - {materiaDAO.Consulta(aula.CodMateria).Descricao}", aula.ID.ToString());
                listaAulas.Add(item);
            }

            List<SelectListItem> listaAlunos = new List<SelectListItem>();
            listaAlunos.Add(new SelectListItem("Selecione um aluno...", "0"));
            foreach (var aluno in alunos)
            {
                SelectListItem item = new SelectListItem(aluno.Nome, aluno.ID.ToString());
                listaAlunos.Add(item);
            }
            ViewBag.Alunos = listaAlunos;
            ViewBag.Aulas = listaAulas;
        }
    }
}
