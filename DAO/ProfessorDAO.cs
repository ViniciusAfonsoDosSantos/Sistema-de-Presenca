using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.DAO
{
    public class ProfessorDAO: PadraoDAO<ProfessorViewModel>
    {
        protected override SqlParameter[] CriaParametros(ProfessorViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("id", model.ID);
            parametros[1] = new SqlParameter("nome", model.Nome);
            parametros[2] = new SqlParameter("email", model.Email);
            parametros[3] = new SqlParameter("telefone", model.Telefone);
            parametros[4] = new SqlParameter("cpf", model.CPF);
            return parametros;
        }

        protected override ProfessorViewModel MontaModel(DataRow registro, bool comJoin = false)
        {
            ProfessorViewModel a = new ProfessorViewModel()
            {
                ID = Convert.ToInt32(registro["id"]),
                Nome = registro["nome"].ToString(),
                Email = registro["email"].ToString(),
                Telefone = registro["telefone"].ToString(),
                CPF = registro["cpf"].ToString()
            };
            return a;
        }

        protected override void SetTabela()
        {
            Tabela = "Professor";
        }

        public override int ProximoId()
        {
            var codigo = 0;
            var p = new SqlParameter[] {
                new SqlParameter("tabela", Tabela)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spProximoId", p);

            if (tabela.Rows[0][0].ToString() != "1")
                codigo = Convert.ToInt32("2" + Convert.ToString(Convert.ToInt32(tabela.Rows[0][0].ToString().Substring(1)) + 1));
            else
                codigo = 21;
            return codigo;

        }

        //public List<ProfessorViewModel> ConsultaAvancada(int id)
        //{
        //    SqlParameter[] p =
        //    {
        //        new SqlParameter("ID", id)
        //    };

        //    var tabela = HelperDAO.ExecutaProcSelect("spConsultaAvancadaProfessor", p);
        //    var lista = new List<ProfessorViewModel>();
        //    foreach (DataRow dr in tabela.Rows)
        //    {
        //        lista.Add(MontaModel(dr));
        //    }
        //    return lista;
        //}
    }
}
