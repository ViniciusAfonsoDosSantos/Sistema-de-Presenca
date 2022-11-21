using MongoDB.Driver;
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
                lista.Add(Convert.ToDateTime(dr["HorarioPresenca"]).ToString("dd/MM/yyyy HH:mm"));
            }
            return lista;
        }

        protected override PresencaViewModel MontaModel(DataRow registro, bool comJoin = false)
        {
            PresencaViewModel p = new PresencaViewModel()
            {
                ID = Convert.ToInt32(registro["id"]),
                CodAluno = Convert.ToInt32(registro["codAluno"]),
                CodAula = Convert.ToInt32(registro["codAula"]),
                Presente = registro["presente"].ToString(),
                DataHoraPresenca = Convert.ToDateTime(registro["HorarioPresenca"]),
                AlunoNome = registro["nome"].ToString(),
                AulaDescricao = registro["Conteudo"].ToString(),
                MateriaDescricao = registro["descricao"].ToString()
            };
            return p;
        }

        protected override void SetTabela()
        {
            Tabela = "Presenca";
        }

        public void PegaDadosMongoDB()
        {
            string conn = "mongodb://helix:H3l1xNG@191.233.28.24:27000/?authMechanism=SCRAM-SHA-1";
            var client = new MongoClient(conn);
            var db = client.GetDatabase("sth_helixiot");
            foreach (var aluno in new AlunoDAO().Listagem())
            {
                if (aluno.IdBiometria != 0)
                {
                    var entity = db.GetCollection<ComandosModel>($"sth_/_urn:ngsi-ld:aluno:{aluno.IdBiometria.ToString("D3")}_Aluno");

                    //na entidade do aluno, pegar os documentos dos comandos de presença
                    //pegar as novas presencas do aluno, ordenar pelas mais recentes
                    PresencaDAO dao = new PresencaDAO();
                    List<string> listaDatasPresencas = dao.NovasPresencas(31);
                    var docs = entity.Find(x => x.attrName == "presenca").ToList();
                    docs.Reverse();
                    foreach (var doc in docs)
                    {
                        if (listaDatasPresencas.Contains(doc.recvTime.ToString("dd/MM/yyyy HH:mm")))
                        {
                            return;
                        }
                        ColocarPresencaSQL(doc);
                    }
                }
            }
            
        }
        private void ColocarPresencaSQL(ComandosModel doc)
        {
            AulaViewModel aula = ObterAulaDia(doc.recvTime);
            if (aula != null)
            {
                //Achar aluno pelo IdBiometria
                int idAluno = 31;
                AtribuiPresencaAluno(idAluno, aula.ID, doc.attrValue, doc.recvTime);
            }
        }

        private void AtribuiPresencaAluno(int idAluno, int codAula, string situacao, DateTime horarioPresenca)
        {
            PresencaDAO presenca = new PresencaDAO();
            presenca.Insert(new PresencaViewModel { CodAluno = idAluno, CodAula = codAula, Presente = situacao, DataHoraPresenca = horarioPresenca });
            
        }
        private AulaViewModel ObterAulaDia(DateTime recvTime)
        {
            AulaDAO aula = new AulaDAO();
            return aula.ConsultaPorData(recvTime);
        }

        public List<PresencaViewModel> ConsultaAvancada(int idAluno, int idAula)
        {
            SqlParameter[] p =
            {
                new SqlParameter("idAluno", idAluno),
                new SqlParameter("idAula", idAula)
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsultaPresencaParametrizada", p);
            var listaPresencas = new List<PresencaViewModel>();
            foreach (DataRow dr in tabela.Rows)
            {
                listaPresencas.Add(MontaModel(dr));
            }
            var lista = ColocaNaoPresentes(idAluno, idAula, listaPresencas);
            
            return lista;
        }

        private List<PresencaViewModel> ColocaNaoPresentes(int idAluno, int idAula, List<PresencaViewModel> listaPresencas)
        {
            //situacoes => filtro de aula com todos os alunos -> colocar não presentes na aula
            //             filtro de aluno com todas as aulas -> colocar não presente na aula
            //             filtro de aluno com aula específica -> colocar não presente se não achar
            AlunoDAO alunoDAO = new AlunoDAO();
            AulaDAO aulaDAO = new AulaDAO();
            MateriaDAO materiaDAO = new MateriaDAO();
            if(idAluno == 0 && idAula != 0)
            {
                foreach (var aluno in alunoDAO.Listagem())
                {
                    if(listaPresencas.Find(a => a.CodAluno == aluno.ID) == null)
                    {
                        listaPresencas.Add(new PresencaViewModel
                        {
                            CodAluno = aluno.ID,
                            CodAula = idAula,
                            Presente = "Não Presente",
                            AlunoNome = aluno.Nome,
                            AulaDescricao = aulaDAO.Consulta(idAula).Conteudo,
                            MateriaDescricao = materiaDAO.Consulta(aulaDAO.Consulta(idAula).CodMateria).Descricao
                        });
                    }
                }
                return listaPresencas;
            }
            if (idAluno != 0 && idAula == 0)
            {
                foreach (var aula in aulaDAO.Listagem())
                {
                    if (listaPresencas.Find(a => a.CodAula == aula.ID) == null)
                    {
                        listaPresencas.Add(new PresencaViewModel
                        {
                            CodAluno = idAluno,
                            CodAula = aula.ID,
                            Presente = "Não Presente",
                            AlunoNome = alunoDAO.Consulta(idAluno).Nome,
                            AulaDescricao = aula.Conteudo,
                            MateriaDescricao = materiaDAO.Consulta(aula.CodMateria).Descricao
                        });
                    }
                }
                return listaPresencas;
            }
            if(idAluno!=0 && idAula != 0)
            {
                if (listaPresencas.Find(a => a.CodAluno == idAluno) == null)
                {
                    listaPresencas.Add(new PresencaViewModel
                    {
                        CodAluno = idAluno,
                        CodAula = idAula,
                        Presente = "Não Presente",
                        AlunoNome = alunoDAO.Consulta(idAluno).Nome,
                        AulaDescricao = aulaDAO.Consulta(idAula).Conteudo,
                        MateriaDescricao = materiaDAO.Consulta(aulaDAO.Consulta(idAula).CodMateria).Descricao
                    });
                }
                return listaPresencas;
            }
            else
            {
                return listaPresencas;
            }
        }

        public List<int> PegaQuantidadePresenteEQuantidadeDeMaterias(int idMateria)
        {
            SqlParameter[] pAula =
            {
                new SqlParameter("id", idMateria)
            };

            SqlParameter[] pAluno =
            {
                new SqlParameter("id", idMateria)
            };
            var qntdPresente = HelperDAO.ExecutaProcSelect("spPegaQuantidadePresente", pAula);
            var spPegaQuantidadedeAulasMaterias = HelperDAO.ExecutaProcSelect("spPegaQuantidadedeAulasMaterias", pAluno);

            var lista = new List<int>();
            lista.Add(Convert.ToInt32(qntdPresente.Rows[0][0]));
            lista.Add(Convert.ToInt32(spPegaQuantidadedeAulasMaterias.Rows[0][0]));
            
            return lista;
        }
    }
}
