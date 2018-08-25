using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.dao
{
    public static class ESocialDAO
    {
        public static bool existeNoBanco(int id_servidor)
        {
            string sql_busca =
                     "SELECT id FROM ESocial " +
                     "WHERE id_servidor = " + id_servidor;

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

        public static DataSet getDocumentosNaoProcessados(int limite)
        {
            string sql_busca =
                      "SELECT * FROM ESocial " +
                      "WHERE status = 0 " +
                      "ORDER BY data DESC " +
                      "LIMIT " + limite;

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

        public static void novo(int id_servidor, int id_empresa, int status, int ambiente, DateTime data, string xml_base64)
        {
            try
            {
                string sql_insert =
                    "INSERT INTO ESocial(id_servidor, id_empresa, status, ambiente, data, xml_base64)" +
                    "VALUES(" + id_servidor + ", " + id_empresa + ", " + status + ", " + ambiente + ", '" + data.ToString("yyyy-MM-dd") + "', '" + xml_base64 + "')";

                if (existeNoBanco(id_servidor) == false)
                {
                    using (Conexao bd = new Conexao())
                    {
                        SQLiteCommand cmd = new SQLiteCommand(sql_insert, bd.Conector);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
