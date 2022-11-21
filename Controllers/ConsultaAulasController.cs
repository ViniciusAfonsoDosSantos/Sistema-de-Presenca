using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Enumeradores;
using TrabalhoInterdisciplinar.Helpers;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class ConsultaAulasController : Controller
    {
        protected bool ExigeAutenticacao { get; set; } = true;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (ExigeAutenticacao && (!HelperControllers.VerificaAlunoLogado(HttpContext.Session) &&
                !HelperControllers.VerificaProfessorLogado(HttpContext.Session)))
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

        public IActionResult Index()
        {
            try
            {
                PreparaDadosParaFiltros();
                //Listagem -> retorna uma lista
                //Pegar a lista do listagem e fazer um foreach
                AulaDAO aulaDAO = new AulaDAO();
                double ativo = 0;
                double finalizado = 0;
                double futura = 0;
                if(aulaDAO.Listagem().Count!= 0)
                {
                    foreach (var item in aulaDAO.Listagem())
                    {
                        if (DateTime.Now < item.DataHoraAula.AddMinutes(15) && DateTime.Now > item.DataHoraAula.AddMinutes(-15))
                        {
                            ativo++;
                        }
                        else if (DateTime.Now > item.DataHoraAula.AddMinutes(15))
                        {
                            finalizado++;
                        }
                        else if (DateTime.Now < item.DataHoraAula.AddMinutes(-15))
                        {
                            futura++;
                        }
                    }

                    if (ativo != 0)
                    {
                        ViewBag.Ativo = Convert.ToInt16((ativo / aulaDAO.Listagem().Count) * 100);
                    }
                    else
                    {
                        ViewBag.Ativo = 0;
                    }
                    ViewBag.Finalizado = Convert.ToInt16((finalizado / aulaDAO.Listagem().Count) * 100);
                    ViewBag.Futura = Convert.ToInt16((futura / aulaDAO.Listagem().Count) * 100);
                }
                

                return View("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }
        }

        public IActionResult ConsultaAulaAvancada(DateTime dataInicial, DateTime dataFinal, int idMateria, EnumSituacaoAula situacao)
        {
            try
            {
                AulaDAO aulaDAO = new AulaDAO();
                if (dataInicial.Date == Convert.ToDateTime("01/01/0001"))
                    dataInicial = SqlDateTime.MinValue.Value;
                if (dataFinal.Date == Convert.ToDateTime("01/01/0001"))
                    dataFinal = SqlDateTime.MaxValue.Value;
                if (string.IsNullOrEmpty(idMateria.ToString()))
                    idMateria = 0;
                var lista = aulaDAO.ConsultaAvancada(dataInicial, dataFinal, idMateria);

                foreach (var item in lista)
                {
                    if (DateTime.Now < item.DataHoraAula.AddMinutes(15) && DateTime.Now > item.DataHoraAula.AddMinutes(-15))
                    {
                        item.Situacao = EnumSituacaoAula.Ativo;
                    }
                    else if (DateTime.Now > item.DataHoraAula.AddMinutes(15))
                    {
                        item.Situacao = EnumSituacaoAula.Finalizada;
                    }
                    else if (DateTime.Now < item.DataHoraAula.AddMinutes(-15))
                    {
                        item.Situacao = EnumSituacaoAula.Futura;
                    }
                }

                lista = VerificaSituacao(lista, situacao);

                if (lista.Count != 0)
                {
                    return PartialView("pvGridAula", lista);
                }
                else
                {
                    return PartialView("pvGridSemResultado");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public List<AulaViewModel> VerificaSituacao(List<AulaViewModel> lista, EnumSituacaoAula situacao)
        {
            int situacaoInt = (int)situacao;
            var listaFinal = new List<AulaViewModel>();
            foreach (var item in lista)
            {
                if (situacaoInt == 1 && (int)item.Situacao == 1)
                {
                    listaFinal.Add(item);
                }
                else if (situacaoInt == 2 && (int)item.Situacao == 2)
                {
                    listaFinal.Add(item);
                }
                else if (situacaoInt == 3 && (int)item.Situacao == 3)
                {
                    listaFinal.Add(item);
                }
                else if (situacaoInt == 0)
                {
                    listaFinal.Add(item);
                }
            }
            return listaFinal;
        }

        private void PreparaDadosParaFiltros()
        {
            try
            {
                MateriaDAO materiaDAO = new MateriaDAO();
                var materias = materiaDAO.Listagem();
                List<SelectListItem> listaMaterias = new List<SelectListItem>();
                listaMaterias.Add(new SelectListItem("Selecione uma matéria...", "0"));
                foreach (var materia in materias)
                {
                    SelectListItem item = new SelectListItem(materia.Descricao, materia.ID.ToString());
                    listaMaterias.Add(item);
                }
                ViewBag.Materias = listaMaterias;
            }
            catch(Exception err)
            {
                throw new Exception("Problemas ao listar o filtro... " + err.Message);
            }
        }
    }
}
