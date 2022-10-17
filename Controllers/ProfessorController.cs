using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class ProfessorController : PadraoController<ProfessorViewModel>
    {
        public ProfessorController()
        {
            DAO = new ProfessorDAO();
            GeraProximoId = true;
            ExigeAutenticacao = true;
        }

        protected override void ValidaDados(ProfessorViewModel professor, string operacao)
        {
            base.ValidaDados(professor, operacao);
            if (string.IsNullOrEmpty(professor.Nome))
                ModelState.AddModelError("Nome", "Campo obrigatório.");
            if (string.IsNullOrEmpty(professor.Email))
                ModelState.AddModelError("Email", "Campo obrigatório.");
            else
            {
                if (professor.Email.Length < 5)
                    ModelState.AddModelError("Email", "Email Inválido.");
                else
                {
                    if (professor.Email.Substring((professor.Email.Length - 4), 4) != ".com" || professor.Email.Substring((professor.Email.Length - 5), 1) == "@"
                    || professor.Email.IndexOf("@") == -1 || professor.Email.IndexOf("@") == 0)
                        ModelState.AddModelError("Email", "Email Inválido.");
                }
            }
            if (string.IsNullOrEmpty(professor.Telefone))
                ModelState.AddModelError("Telefone", "Campo obrigatório.");
            else
            {
                Regex validaNumeroTelefoneRegex = new Regex("^\\([1-9]{2}\\) (?:[2-8]|9[1-9])[0-9]{3}\\-[0-9]{4}$");
                if (!validaNumeroTelefoneRegex.IsMatch(professor.Telefone))
                    ModelState.AddModelError("Telefone", "Telefone Inválido.");
            }
            if (string.IsNullOrEmpty(professor.CPF))
                ModelState.AddModelError("CPF", "Campo obrigatório.");
            else
            {
                Regex validaNumeroCPFRegex = new Regex("^\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2}$");
                if (!validaNumeroCPFRegex.IsMatch(professor.CPF))
                    ModelState.AddModelError("CPF", "CPF Inválido.");
            }
            //Falta validar se CPF é valido ou não
            // Talvez usar API ou AJAX para consultar na receita federal
        }
    }
}
