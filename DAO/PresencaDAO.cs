using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.DAO
{
    public class PresencaDAO : PadraoDAO<PresencaViewModel>
    {
        protected override SqlParameter[] CriaParametros(PresencaViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("id", model.ID);
            parametros[1] = new SqlParameter("codAluno", model.CodAluno);
            parametros[2] = new SqlParameter("codAula", model.CodAula);
            parametros[3] = new SqlParameter("presente", model.Presente);
            parametros[4] = new SqlParameter("dataHoraPresenca", model.DataHoraPresenca);
            return parametros;
        }

        public List<string> NovasPresencas(int codAluno)
        {
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("codAluno", codAluno);
            var tabela = HelperDAO.ExecutaProcSelect("spConsultaDataPresenca", p);
            var lista = new List<string>(); //colocar só para DateTime
            foreach (DataRow dr in tabela.Rows)
            {
                //lista.Add(MontaModel(dr[]
                lista.Add(Convert.ToDateTime(dr["HorarioPresenca"]).ToString("dd/MM/yyyy HH:mm"));
            }
            return lista;
        }

        protected override PresencaViewModel MontaModel(DataRow registro)
        {
            PresencaViewModel p = new PresencaViewModel()
            {
                ID = Convert.ToInt32(registro["id"]),
                CodAluno = Convert.ToInt32(registro["codAluno"]),
                CodAula = Convert.ToInt32(registro["codAula"]),
                Presente = registro["presente"].ToString(),
                DataHoraPresenca = Convert.ToDateTime(registro["HorarioPresenca"])
            };
            return p;
        }

        protected override void SetTabela()
        {
            Tabela = "Presenca";
        }
    }
}
