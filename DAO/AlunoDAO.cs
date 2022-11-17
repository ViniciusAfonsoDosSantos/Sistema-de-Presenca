using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.DAO
{
    public class AlunoDAO : PadraoDAO<AlunoViewModel>
    {
        protected override SqlParameter[] CriaParametros(AlunoViewModel model)
        {
            object imgByte = model.ImagemEmByte;
            if (imgByte == null)
                imgByte = DBNull.Value;


            SqlParameter[] parametros = new SqlParameter[7];
            parametros[0] = new SqlParameter("id", model.ID);
            parametros[1] = new SqlParameter("nome", model.Nome);
            parametros[2] = new SqlParameter("email", model.Email);
            parametros[3] = new SqlParameter("telefone", model.Telefone);
            parametros[4] = new SqlParameter("cpf", model.Cpf);
            parametros[5] = new SqlParameter("imagem", imgByte);
            parametros[6] = new SqlParameter("IdBiometria", model.IdBiometria);

            return parametros;
        }

        protected override AlunoViewModel MontaModel(DataRow registro, bool comJoin = false)
        {
            AlunoViewModel a = new AlunoViewModel()
            {
                ID = Convert.ToInt32(registro["id"]),
                Nome = registro["nome"].ToString(),
                Email = registro["email"].ToString(),
                Telefone = registro["telefone"].ToString(),
                Cpf = registro["cpf"].ToString()
            };
            if (registro["idBiometria"] != DBNull.Value)
                a.IdBiometria = Convert.ToInt32(registro["IdBiometria"]);
            if (registro["imagem"] != DBNull.Value)
                a.ImagemEmByte = registro["imagem"] as byte[];
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

        public int ProximoIdBiometria()
        {   
            var tabela = HelperDAO.ExecutaProcSelect("spProximoIdBiometria", null);
            return Convert.ToInt32(tabela.Rows[0][0].ToString());
        }

        //public List<AlunoViewModel> ConsultaAvancada(int id)
        //{
        //    SqlParameter[] p =
        //    {
        //        new SqlParameter("ID", id)
        //    };

        //    var tabela = HelperDAO.ExecutaProcSelect("spConsultaAvancadaAluno", p);
        //    var lista = new List<AlunoViewModel>();
        //    foreach (DataRow dr in tabela.Rows)
        //    {
        //        lista.Add(MontaModel(dr));
        //    }
        //    return lista;
        //}
    }
}
