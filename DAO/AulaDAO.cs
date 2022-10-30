using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.DAO
{
    public class AulaDAO: PadraoDAO<AulaViewModel>
    {
        protected override SqlParameter[] CriaParametros(AulaViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("id", model.ID);
            parametros[1] = new SqlParameter("conteudo", model.Conteudo);
            parametros[2] = new SqlParameter("datahoraaula", model.DataHoraAula);
            parametros[3] = new SqlParameter("codmateria", model.CodMateria);
            return parametros;
        }

        protected override AulaViewModel MontaModel(DataRow registro)
        {
            AulaViewModel a = new AulaViewModel()
            {
                ID = Convert.ToInt32(registro["id"]),
                Conteudo = registro["conteudo"].ToString(),
                DataHoraAula = Convert.ToDateTime(registro["datahoraaula"]),
                CodMateria = Convert.ToInt32(registro["codmateria"])
            };
            return a;
        }

        protected override void SetTabela()
        {
            Tabela = "Aula";
        }

        public List<AulaViewModel> ConsultaAvancada(int id)
        {
            SqlParameter[] p =
            {
                new SqlParameter("ID", id)
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsultaAvancadaAula", p);
            var lista = new List<AulaViewModel>();
            foreach (DataRow dr in tabela.Rows)
            {
                lista.Add(MontaModel(dr));
            }
            return lista;
        }
        public List<AulaViewModel> ConsultaAvancada(DateTime dataInicio, DateTime dataFim, int idMateria)
        {
            SqlParameter[] p =
            {
                new SqlParameter("data_inicial", dataInicio),
                new SqlParameter("data_final", dataFim),
                new SqlParameter("id_materia", idMateria)
            };

            string query = "SELECT * from fnConsultaAvancada (@data_inicial, @data_final, @id_materia)";
            var tabela = HelperDAO.ExecutaFunctionSelect(query, p);
            var lista = new List<AulaViewModel>();
            foreach (DataRow dr in tabela.Rows)
            {
                lista.Add(new AulaViewModel
                {
                    Conteudo = dr["conteudo"].ToString(),
                    DataHoraAula = Convert.ToDateTime(dr["datahoraaula"]),
                    Materia = dr["materia"].ToString()
                });
            }
            return lista;
        }
    }
}
