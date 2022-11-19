using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TrabalhoInterdisciplinar.ConexãoHelix;
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

        public IActionResult LerPresenca()
        {
            try
            {
                ConexaoMQTT conectaHelix = new ConexaoMQTT();
                conectaHelix.PublishReadMQTT();
                return RedirectToAction("Index", "ConsultaPresencas");
            }
            catch(Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
            
        }

        //Verificar se existem novas presenças
        //Pegar o dado e que está no histórico do orion e passar para a tabela de presença no sql (pegar a data e hora desse dado
        //e colocar )
        public IActionResult Index()
        {
            try
            {
                AlunoDAO alunodao = new AlunoDAO();
                MateriaDAO materiadao = new MateriaDAO();
                PresencaDAO presencadao = new PresencaDAO();
                int qntdAlunos = alunodao.Listagem().Count;
                ViewBag.QntdDePresenca = new List<object>();

                foreach (var item in materiadao.Listagem())
                {
                    List<int> quantidadespresenca = presencadao.PegaQuantidadePresenteEQuantidadeDeMaterias(item.ID);
                    int valor = Convert.ToInt32((qntdAlunos * quantidadespresenca[1]) / quantidadespresenca[0]);
                    ViewBag.QntdDePresenca.Add(new { valor, item.Descricao });
                }
                presencadao.PegaDadosMongoDB();
                PreparaDadosParaFiltros();
                return View("Index");
            }
            catch(Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
            
        }
        
        private void PreparaDadosParaFiltros()
        {
            try
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
                    if (aula.DataHoraAula < DateTime.Now)
                    {
                        SelectListItem item = new SelectListItem($"{aula.Conteudo} - {materiaDAO.Consulta(aula.CodMateria).Descricao}", aula.ID.ToString());
                        listaAulas.Add(item);
                    }
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
            catch(Exception err)
            {
                throw new Exception("Problemas ao listar os dados do filtro... " + err.Message);
            }
            
        }

        public IActionResult ConsultaPresencaAvancada(int idAluno, int idAula)
        {
            try
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
            catch(Exception err)
            {
                return View("Error", new ErrorViewModel(err.ToString()));
            }
        }
    }
}
