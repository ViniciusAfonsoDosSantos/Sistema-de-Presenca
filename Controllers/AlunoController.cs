using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
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

        protected override void ValidaDados(AlunoViewModel aluno, string operacao)
        {
            base.ValidaDados(aluno, operacao);
            if (string.IsNullOrEmpty(aluno.Nome))
                ModelState.AddModelError("Nome", "Campo obrigatório.");
            if (string.IsNullOrEmpty(aluno.Email))
                ModelState.AddModelError("Email", "Campo obrigatório.");
            else
            {
                if (aluno.Email.Length < 5)
                    ModelState.AddModelError("Email", "Email Inválido.");
                else
                {
                    if (aluno.Email.Substring((aluno.Email.Length - 4), 4) != ".com" || aluno.Email.Substring((aluno.Email.Length - 5), 1) == "@"
                    || aluno.Email.IndexOf("@") == -1 || aluno.Email.IndexOf("@") == 0)
                        ModelState.AddModelError("Email", "Email Inválido.");
                }
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
        }

    }
}
