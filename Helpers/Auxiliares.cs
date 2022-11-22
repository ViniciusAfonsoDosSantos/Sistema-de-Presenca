using TrabalhoInterdisciplinar.DAO;

namespace TrabalhoInterdisciplinar.Helpers
{
    public class Auxiliares
    {
        public static bool VerificaCPFExistente(string cpf)
        {
            AlunoDAO alunoDAO = new AlunoDAO();
            ProfessorDAO profDAO = new ProfessorDAO();
            var cpfAluno = alunoDAO.Listagem().Find(a => a.Cpf == cpf);
            if(cpfAluno == null)
            {
                var cpfProfessor = profDAO.Listagem().Find(p => p.CPF == cpf);
                if (cpfProfessor == null)
                    return true;
                return false;
            }
            return false;
        }

        public static bool VerificaTelefone(string telefone)
        {
            AlunoDAO alunoDAO = new AlunoDAO();
            ProfessorDAO profDAO = new ProfessorDAO();
            var telAluno = alunoDAO.Listagem().Find(a => a.Telefone == telefone);
            if (telAluno == null)
            {
                var telProfessor = profDAO.Listagem().Find(p => p.Telefone == telefone);
                if (telProfessor == null)
                    return true;
                return false;
            }
            return false;
        }
        public static bool ValidaCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            switch (cpf)
            {
                case "11111111111":
                    return false;
                case "00000000000":
                    return false;
                case "2222222222":
                    return false;
                case "33333333333":
                    return false;
                case "44444444444":
                    return false;
                case "55555555555":
                    return false;
                case "66666666666":
                    return false;
                case "77777777777":
                    return false;
                case "88888888888":
                    return false;
                case "99999999999":
                    return false;
            }

            tempCpf = cpf.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}
