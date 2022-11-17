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

        protected override AulaViewModel MontaModel(DataRow registro, bool comJoin=false)
        {
            AulaViewModel a = new AulaViewModel()
            {
                ID = Convert.ToInt32(registro["id"]),
                Conteudo = registro["conteudo"].ToString(),
                DataHoraAula = Convert.ToDateTime(registro["datahoraaula"]),
                CodMateria = Convert.ToInt32(registro["codmateria"])
            };
            if (comJoin)
                a.Materia = registro["descricao"].ToString();
            return a;
        }

        protected override void SetTabela()
        {
            Tabela = "Aula";
        }

        public List<AulaViewModel> ListagemAulas()
        {
            var tabela = HelperDAO.ExecutaProcSelect("spListagemAulas", null);
            List<AulaViewModel> lista = new List<AulaViewModel>();
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));
            return lista;
        }

        public AulaViewModel ConsultaPorData(DateTime horarioPresenca)
        {
            DateTime horarioParam;
            if ((horarioPresenca.Hour == 19 && horarioPresenca.Minute < 30) || (horarioPresenca.Hour == 18 && horarioPresenca.Minute >= 15))
                horarioParam = new DateTime(horarioPresenca.Year, horarioPresenca.Month, horarioPresenca.Day, 19, 15, 00, 00);
            if ((horarioPresenca.Hour == 21 && horarioPresenca.Minute <30) || (horarioPresenca.Hour == 20 && horarioPresenca.Minute >= 55))
                horarioParam = new DateTime(horarioPresenca.Year, horarioPresenca.Month, horarioPresenca.Day, 21, 5, 00, 00);
            else
                horarioParam = new DateTime(horarioPresenca.Year, horarioPresenca.Month, horarioPresenca.Day, 16, 00, 00, 00);

            SqlParameter[] p =
            {
                new SqlParameter("data", horarioParam)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spConsultaAulaPorData", p);
            foreach (DataRow dr in tabela.Rows)
            {
                return MontaModel(dr);
            }
            return null;
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
