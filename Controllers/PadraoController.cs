using Microsoft.AspNetCore.Mvc;
using System;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class PadraoController<T>: Controller where T: PadraoViewModel
    {
        protected PadraoDAO<T> DAO { get; set; }
        protected bool GeraProximoId { get; set; }
        protected string NomeViewIndex { get; set; } = "index";
        protected string NomeViewForm { get; set; } = "Form";
        protected bool ExigeAutenticacao { get; set; } = true;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (ExigeAutenticacao && !HelperControllers.VerificaProfessorLogado(HttpContext.Session))
                context.Result = RedirectToAction("Index", "Home");
            else
            {
                ViewBag.LogadoProfessor = true;
                base.OnActionExecuting(context);
            }
        }

        public virtual IActionResult Index()
        {
            try
            {
                var lista = DAO.Listagem();
                return View(NomeViewIndex, lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public virtual IActionResult Create()
        {
            try
            {
                ViewBag.Operacao = "I";
                T model = Activator.CreateInstance(typeof(T)) as T;
                PreencheDadosParaView("I", model);
                return View(NomeViewForm, model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        protected virtual void PreencheDadosParaView(string Operacao, T model)
        {
            if (GeraProximoId && Operacao == "I")
                model.ID = DAO.ProximoId();
        }

        public virtual IActionResult Save(T model, string Operacao)
        {
            try
            {
                ValidaDados(model, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreencheDadosParaView(Operacao, model);
                    return View(NomeViewForm, model);
                }
                else
                {
                    if (Operacao == "I")
                    {
                        DAO.Insert(model);
                        TempData["AlertMessage"] = "Dado salvo com sucesso...!";
                    }
                    else
                    {
                        DAO.Update(model);
                        TempData["AlertMessage"] = "Dado alterado com sucesso...!";
                    }
                    return RedirectToAction("Create");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        
        public async virtual Task<IActionResult> SalvaAssincrono (T model, string Operacao)
        {
          try
          {
              ValidaDados(model, Operacao);
              if (ModelState.IsValid == false)
              {
                  ViewBag.Operacao = Operacao;
                  PreencheDadosParaView(Operacao, model);
                  return View(NomeViewForm, model);
              }
              else
              {
                  if (Operacao == "I")
                      DAO.Insert(model);
                  else
                      DAO.Update(model);
                  TempData["AlertMessage"] = "Dado salvo com sucesso...!           ";
                  return RedirectToAction("Create");
              }
          }
          catch (Exception erro)
          {
              return View("Error", new ErrorViewModel(erro.ToString()));
          }
          return View("Error", new ErrorViewModel("VAI CURINTIA"));
        }

        protected virtual void ValidaDados(T model, string operacao)
        {
            ModelState.Clear();
            if (operacao == "I" && DAO.Consulta(model.ID) != null)
                ModelState.AddModelError("Id", "ID já está em uso!");
            if (operacao == "A" && DAO.Consulta(model.ID) == null)
                ModelState.AddModelError("Id", "Este registro não existe!");
            if (model.ID <= 0)
                ModelState.AddModelError("Id", "Id inválido!");
        }
        public IActionResult Edit(int id)
        {
            try
            {
                ViewBag.Operacao = "A";
                var model = DAO.Consulta(id);
                if (model == null)
                    return RedirectToAction("Index", "ConsultaListagens");
                else
                {
                    PreencheDadosParaView("A", model);
                    return View(NomeViewForm, model);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public IActionResult Delete(int id)
        {
            try
            {
                DAO.Delete(id);
                return RedirectToAction("Index", "ConsultaListagens");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
    }
}
