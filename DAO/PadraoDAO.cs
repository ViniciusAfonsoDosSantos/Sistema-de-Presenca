using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.DAO
{
    public abstract class PadraoDAO<T> where T: PadraoViewModel
    {
        public PadraoDAO()
        {
            SetTabela();
        }
        protected abstract void SetTabela();

        protected string Tabela { get; set; }
        protected string NomeSpListagem { get; set; } = "spListagem";
        protected abstract SqlParameter[] CriaParametros(T model);
        protected abstract T MontaModel(DataRow registro, bool comJoin);

        public virtual void Insert(T model)
        {
            HelperDAO.ExecutaProc("spInsert_" + Tabela, CriaParametros(model));
        }
        public virtual void Update(T model)
        {
            HelperDAO.ExecutaProc("spUpdate_" + Tabela, CriaParametros(model));
        }

        public virtual void Delete(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("tabela", Tabela)
            };

            HelperDAO.ExecutaProc("spDelete", p);
        }

        public virtual T Consulta(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("tabela", Tabela)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spConsulta", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0], false);
        }

        public virtual int ProximoId()
        {
            var p = new SqlParameter[] {
                new SqlParameter("tabela", Tabela)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spProximoId", p);
            
            return Convert.ToInt32(tabela.Rows[0][0]) + 1;
        }

        public virtual List<T> Listagem()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("tabela", Tabela),
                new SqlParameter("Ordem", "1")
            };
            var tabela = HelperDAO.ExecutaProcSelect(NomeSpListagem, p);
            List<T> lista = new List<T>();
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro,false));
            return lista;
        }

        
        public virtual List<T> ConsultaAvancada(int id)
        {
            SqlParameter[] p =
            {
                new SqlParameter("ID", id),
                new SqlParameter("tabela", Tabela)
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsultaAvancadaGenerica", p);
            var lista = new List<T>();
            foreach (DataRow dr in tabela.Rows)
            {
                lista.Add(MontaModel(dr, false));
            }
            return lista;
        }
        
    }
}
