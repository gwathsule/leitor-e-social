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

        /// <summary>
        /// checa se existe uma nota de certo status no banco
        /// </summary>
        /// <param name="sinal"> <, >, =, <= ou >= </param>
        /// <param name="status">0, 1 ou 2</param>
        /// <returns></returns>
        public static bool checaStatusNoBanco(int id, string sinal, int status)
        {
            string sql_busca =
                     "SELECT id FROM ESocial " +
                     "WHERE id = " + id + " " + 
                     "AND status " + sinal + " " + status;

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

        public static DataSet getDocumentosNaoProcessados(int limite, int id_empresa_servidor)
        {
            string sql_busca =
                      "SELECT * FROM ESocial " +
                      "WHERE status = 0 " +
                      "AND id_empresa = " + id_empresa_servidor + " " +
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

        public static DataSet getDocumentos(int limite, int id_empresa_servidor)
        {
            string sql_busca =
                      "SELECT status, id_servidor, data FROM ESocial " +
                      "WHERE id_empresa = " + id_empresa_servidor + " " +
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

        public static DataSet getDocumentosProcessados(int limite, int id_empresa_servidor)
        {
            string sql_busca =
                      "SELECT * FROM ESocial " +
                      "WHERE status = 1 " +
                      "AND id_empresa = " + id_empresa_servidor + " " +
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

        public static void marcarComoProcessado(int id, string xml_assinado_base64, string resposta_base64)
        {

            string sql_update =
                "UPDATE ESocial SET " +
                    "xml_base64 = '" + xml_assinado_base64 + "', " +
                    "resposta_xml_base64 = '" + resposta_base64 + "', " +
                    "status = " + 1 + " " +
                    "WHERE id = " + id;

            if(checaStatusNoBanco(id, "=", 0))
            {
                using (Conexao bd = new Conexao())
                {
                    SQLiteCommand cmd = new SQLiteCommand(sql_update, bd.Conector);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void novo(int id_servidor, int id_empresa, int status, int ambiente, DateTime data, string xml_base64, int user_id)
        {
            try
            {
                string sql_insert =
                    "INSERT INTO ESocial(id_servidor, id_empresa, status, ambiente, data, xml_base64, id_user)" +
                    "VALUES(" + id_servidor + ", " + id_empresa + ", " + status + ", " + ambiente + ", '" + data.ToString("yyyy-MM-dd") + "', '" + xml_base64 + "', " + user_id + ")";

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

        public static void novoDocumentoProcessado(int id_servidor, int id_empresa, int status, int ambiente, DateTime data, 
            string xml_base64, string resposta_xml_base64, int user_id)
        {
            string sql_insert =
                    "INSERT INTO ESocial(id_servidor, id_empresa, status, ambiente, data, xml_base64, resposta_xml_base64, id_user)" +
                    "VALUES(" + 
                    id_servidor + ", " + 
                    id_empresa + ", " + 
                    status + ", " + 
                    ambiente + ", '" + 
                    data.ToString("yyyy-MM-dd") + "', '" + 
                    xml_base64 + "', '" +
                    resposta_xml_base64 + "', " +
                    user_id + ")";

            if (existeNoBanco(id_servidor) == false)
            {
                using (Conexao bd = new Conexao())
                {
                    SQLiteCommand cmd = new SQLiteCommand(sql_insert, bd.Conector);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void marcarComoProcessado(int documento_id)
        {
            string sql_update =
                "UPDATE ESocial SET " +
                    "status = " + 2 + " " +
                    "WHERE id = " + documento_id;

            if (checaStatusNoBanco(documento_id, "=", 1))
            {
                using (Conexao bd = new Conexao())
                {
                    SQLiteCommand cmd = new SQLiteCommand(sql_update, bd.Conector);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
