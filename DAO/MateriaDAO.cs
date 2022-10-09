using System;
using System.Data;
using System.Data.SqlClient;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.DAO
{
    public class MateriaDAO: PadraoDAO<MateriaViewModel>
    {
        protected override SqlParameter[] CriaParametros(MateriaViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("id", model.ID);
            parametros[1] = new SqlParameter("descricao", model.Descricao);
            parametros[2] = new SqlParameter("cargahoraria", model.CargaHoraria);
            parametros[3] = new SqlParameter("codprofessor", model.CodProfessor);
            return parametros;
        }

        protected override MateriaViewModel MontaModel(DataRow registro)
        {
            MateriaViewModel a = new MateriaViewModel()
            {
                ID = Convert.ToInt32(registro["id"]),
                Descricao = registro["descricao"].ToString(),
                CargaHoraria = Convert.ToDouble(registro["cargahoraria"]),
                CodProfessor = Convert.ToInt32(registro["codprofessor"])
            };
            return a;
        }

        protected override void SetTabela()
        {
            Tabela = "Materia";
        }
    }
}
