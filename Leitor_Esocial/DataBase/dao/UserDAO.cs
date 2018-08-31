using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.dao
{
    public static class UserDAO
    {
        public static void salvarUser(int id_servidor, string nome, string hash, string email, string senha, string documento,
                                      int status_certificado, int educont)
        {
            try
            {
                string sql_insert =
                    "INSERT INTO Users(id_servidor, nome, hash, email, senha, documento, educont, status_certificado)" +
                    "VALUES(" + id_servidor + ", '" 
                    + nome + "', '" 
                    + hash + "', '" 
                    + email + "', '" 
                    + senha + "', '" 
                    + documento + "', "
                    + educont + ", "
                    + status_certificado + ")";

                string sql_update =
                    "UPDATE Users SET " +
                    "id_servidor = " + id_servidor + ", " +
                    "nome = '" + nome + "', " +
                    "hash = '" + hash + "', " +
                    "email = '" + email + "', " +
                    "senha = '" + senha + "', " +
                    "documento = '" + documento + "', " +
                    "educont = '" + educont + "', " +
                    "status_certificado = " + status_certificado + " " +
                    "WHERE id = " + 1;

                using (Conexao bd = new Conexao())
                {
                    string query = "";
                    if (existeUserSalvo())
                        query = sql_update;
                    else
                        query = sql_insert;

                    SQLiteCommand cmd = new SQLiteCommand(query, bd.Conector);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataSet getUser()
        {
            string sql_busca =
                     "SELECT * FROM Users " +
                     "WHERE id = " + 1;

            using (Conexao bd = new Conexao())
            {
                DataSet ds = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(sql_busca, bd.Conector);
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(ds);
                return ds;
            }
        }

        public static void salvarInfoCertificado(int status_certificado, string caminho_certificado, 
                                                 string serial_certificado, string senha_certificado)
        {
            try
            {
                string sql_update =
                   "UPDATE Users SET " +
                   "status_certificado = " + status_certificado + ", " +
                   "caminho_certificado = '" + caminho_certificado + "', " +
                   "serial_certificado = '" + serial_certificado + "', " +
                   "senha_certificado = '" + senha_certificado + "' " +
                   "WHERE id = " + 1;

                using (Conexao bd = new Conexao())
                {
                    string query = "";
                    if (existeUserSalvo())
                        query = sql_update;
                    else
                        throw new Exception("Não há nenhum usuário salvo atualmente no banco");

                    SQLiteCommand cmd = new SQLiteCommand(query, bd.Conector);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool existeUserSalvo()
        {
            string sql_busca =
                     "SELECT id FROM Users " +
                     "WHERE id = " + 1;

            using (Conexao bd = new Conexao())
            {
                DataSet ds = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(sql_busca, bd.Conector);
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
