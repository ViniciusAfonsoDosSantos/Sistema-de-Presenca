using System;
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
    }
}
