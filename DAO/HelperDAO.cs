using System.Data;
using System.Data.SqlClient;

namespace TrabalhoInterdisciplinar.DAO
{
    public static class HelperDAO
    {
        public static string StringCX;
        private static SqlConnection GetConexao()
        {
            SqlConnection conexao = new SqlConnection(StringCX);
            conexao.Open();
            return conexao;
        }
        public static DataTable ExecutaProcSelect(string nomeProc, SqlParameter[] parametros)
        {

            using (SqlConnection conexao = GetConexao())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(nomeProc, conexao))
                {
                    if (parametros != null)
                        adapter.SelectCommand.Parameters.AddRange(parametros);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    conexao.Close();
                    return tabela;
                }
            }
        }
        public static void ExecutaProc(string nomeProc, SqlParameter[] parametros)
        {
            using (SqlConnection conexao = GetConexao())
            {
                using (SqlCommand comando = new SqlCommand(nomeProc, conexao))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    if (parametros != null)
                        comando.Parameters.AddRange(parametros);
                    comando.ExecuteNonQuery();
                }
                conexao.Close();
            }
        }
    }
}
