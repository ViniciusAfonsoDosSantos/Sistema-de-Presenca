using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class HelperControllers 
    {
        public static Boolean VerificaAlunoLogado(ISession session)
        {
            string logado = session.GetString("Logado");
            if (logado == null)
                return false;
            else
                return true;
        }

        public static Boolean VerificaProfessorLogado(ISession session)
        {
            string logado = session.GetString("Logado");
            if (logado == null)
                return false;
            else
                return true;
        }

    }
}
