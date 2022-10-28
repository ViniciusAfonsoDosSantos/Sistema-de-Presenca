using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class AlunoController : PadraoController<AlunoViewModel>
    {
        public AlunoController()
        {
            DAO = new AlunoDAO();
            GeraProximoId = true;
        }

        /// <summary>
        /// Converte a imagem recebida no form em um vetor de bytes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            else
                return null;
        }

        [HttpPost]
        public override IActionResult Save(AlunoViewModel model, string Operacao)
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
                        LoginViewModel modelLogin = new LoginViewModel()
                        {
                            ID = model.ID,
                            SenhaHash = Helpers.PasswordHasher.HashPassword("VaiCurintia")
                        };
                        LoginDAO login = new LoginDAO();
                        login.Insert(modelLogin);
                    }
                    else
                        DAO.Update(model);
                    TempData["AlertMessage"] = "Dado salvo com sucesso...! ";
                    return RedirectToAction("Create");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public async override Task<IActionResult> SalvaAssincrono(AlunoViewModel model, string Operacao)
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
                      LoginViewModel modelLogin = new LoginViewModel()
                      {
                          ID = model.ID,
                          SenhaHash = Helpers.PasswordHasher.HashPassword("VaiCurintia")
                      };
                      LoginDAO login = new LoginDAO();
                      login.Insert(modelLogin);
                      await TesteMongoDB();
                  }
                  else
                      DAO.Update(model);
                  TempData["AlertMessage"] = "Dado salvo com sucesso...! ";
                  return RedirectToAction("Create");
              }
          }
          catch (Exception erro)
          {
              return View("Error", new ErrorViewModel(erro.ToString()));
          }
          
        }

        private async Task TesteMongoDB()
        {            
            var url = "<your url>";

            var request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("fiware-service", "helixiot");
            request.Headers.Add("fiware-servicepath", "/");

            var json = JsonConvert.SerializeObject(new { id = "007", type = "vasco" });
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            using var reqStream = request.GetRequestStream();
            reqStream.Write(byteArray, 0, byteArray.Length);

            using var response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using var respStream = response.GetResponseStream();

            using var reader = new StreamReader(respStream);
            string data = reader.ReadToEnd();
            Console.WriteLine(data);
        }

        protected override void ValidaDados(AlunoViewModel aluno, string operacao)
        {
            base.ValidaDados(aluno, operacao);
            if (string.IsNullOrEmpty(aluno.Nome))
                ModelState.AddModelError("Nome", "Campo obrigatório.");
            if (string.IsNullOrEmpty(aluno.Email))
                ModelState.AddModelError("Email", "Campo obrigatório.");
            else
            {
                Regex validaEmailRegex = new Regex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");
                if (!validaEmailRegex.IsMatch(aluno.Email))
                    ModelState.AddModelError("Email", "Email Inválido.");
            }
            if (string.IsNullOrEmpty(aluno.Telefone))
                ModelState.AddModelError("Telefone", "Campo obrigatório.");
            else
            {
                Regex validaNumeroTelefoneRegex = new Regex("^\\([1-9]{2}\\) (?:[2-8]|9[1-9])[0-9]{3}\\-[0-9]{4}$");
                if (!validaNumeroTelefoneRegex.IsMatch(aluno.Telefone))
                    ModelState.AddModelError("Telefone", "Telefone Inválido.");
            }
            if (string.IsNullOrEmpty(aluno.Cpf))
                ModelState.AddModelError("Cpf", "Campo obrigatório.");
            else
            {
                Regex validaNumeroCPFRegex = new Regex("^\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2}$");
                if (!validaNumeroCPFRegex.IsMatch(aluno.Cpf))
                    ModelState.AddModelError("Cpf", "CPF Inválido.");
            }
            //Falta validar se CPF é valido ou não
            // Talvez usar API ou AJAX para consultar na receita federal

            //Imagem será obrigatio apenas na inclusão. 
            //Na alteração iremos considerar a que já estava salva.
            if (aluno.Imagem == null && operacao == "I")
                ModelState.AddModelError("Imagem", "Escolha uma imagem.");
            if (aluno.Imagem != null && aluno.Imagem.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Imagem", "Imagem limitada a 2 mb.");
            if (ModelState.IsValid)
            {
                //na alteração, se não foi informada a imagem, iremos manter a que já estava salva.
                if (operacao == "A" && aluno.Imagem == null)
                {
                    AlunoViewModel alun = DAO.Consulta(aluno.ID);
                    aluno.ImagemEmByte = alun.ImagemEmByte;
                }
                else
                {
                    aluno.ImagemEmByte = ConvertImageToByte(aluno.Imagem);
                }
            }

        }

    }
}
