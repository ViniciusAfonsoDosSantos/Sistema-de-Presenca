using System;
using System.Data;
using System.Data.SqlClient;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.DAO
{
    public class AlunoDAO : PadraoDAO<AlunoViewModel>
    {
        protected override SqlParameter[] CriaParametros(AlunoViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("id", model.ID);
            parametros[1] = new SqlParameter("nome", model.Nome);
            parametros[2] = new SqlParameter("email", model.Email);
            parametros[3] = new SqlParameter("telefone", model.Telefone);
            parametros[4] = new SqlParameter("cpf", model.Cpf);
            return parametros;
        }

        protected override AlunoViewModel MontaModel(DataRow registro)
        {
            AlunoViewModel a = new AlunoViewModel()
            {
                ID = Convert.ToInt32(registro["id"]),
                Nome = registro["nome"].ToString(),
                Email = registro["email"].ToString(),
                Telefone = registro["telefone"].ToString(),
                Cpf = registro["cpf"].ToString()
            };
            return a;
        }

        protected override void SetTabela()
        {
            Tabela = "Aluno";
        }

        public override int ProximoId()
        {
            var codigo = 0;
            var p = new SqlParameter[] {
                new SqlParameter("tabela", Tabela)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spProximoId", p);

            if (tabela.Rows[0][0].ToString() != "1")
                codigo = Convert.ToInt32("3" + Convert.ToString(Convert.ToInt32(tabela.Rows[0][0].ToString().Substring(1)) + 1));
            else
                codigo = 31;
            return codigo;
         
        }
    }
}
