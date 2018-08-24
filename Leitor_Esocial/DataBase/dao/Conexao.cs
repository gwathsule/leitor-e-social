using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.dao
{
    public class Conexao : IDisposable
    {
        private const string nome_bd = "database";
        //private const string senha_bd = "OnContabil#2018";
        private const int versao_atual = 2;

        private string pasta_db = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OnContabil") + @"\database";
        private string path;
        private string str_conn;
        public SQLiteConnection Conector { get; private set; }

        public Conexao()
        {
            this.path = pasta_db + @"\" + nome_bd + ".db";
            //this.str_conn = "Data Source=" + path + ";Version=3;Password="+senha_bd;
            this.str_conn = "Data Source=" + path + ";Version=3";
            if (File.Exists(path))
            {
                this.Conector = new SQLiteConnection(this.str_conn);
                atualizar_bd();
            }
            else
            {
                criarBD();
            }
            this.Conector.Open();
        }

        private void criarBD()
        {
            try
            {
                if (Directory.Exists(this.pasta_db) == false)
                    Directory.CreateDirectory(this.pasta_db);

                SQLiteConnection.CreateFile(path);
                string str_conn_sem_senha = "Data Source=" + path + ";Version=3";
                SQLiteConnection conn = new SQLiteConnection(str_conn_sem_senha);
                //conn.Open();
                //conn.ChangePassword(senha_bd);
                //conn.Close();

                this.Conector = new SQLiteConnection(this.str_conn);
                iniciaBancoDeDados();

                for (int i = 0; i <= versao_atual; i++)
                {
                    migrar(i);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void atualizar_bd()
        {
            if (File.Exists(this.path) == false)
                criarBD();

            int versao_bd = pegarUltimaVersaoBancoDeDados();
            int qtd_migracao = versao_atual - versao_bd;
            if (qtd_migracao > 0)
            {
                for (int i = versao_bd + 1; i <= versao_atual; i++)
                {
                    migrar(i);
                }
            }
            else
            {
                //nada a ser migrado
            }

        }

        /// <summary>
        /// Essa função faz algo semelhante as migrations do Laravel ou alguns framework que usam esse sistema, só que mais automático, sem a necessidade de executar 
        /// algum comando para rodar as alterações. 
        /// Toda vez que essa classe for chamada, ela verifica a versão atual do banco de dados exsitente na máquina do cliente
        /// e sendo verificada alguma diferença, ela roda todas as alterações de forma sequencial até a versão do banco ficar com o número
        /// descrito na constante "versao_atual"
        /// 
        /// Para manter os bancos atualizados, seguir os seguintes passos:
        /// 1 . Toda vez que ocorrer uma mudança no banco de dados, é necessário acresentar em 1 a constante "versao_atual"
        /// dessa classe.
        /// 2 . A alteração no banco de dados deve ser feita num num if que condiz com a versão atual do BD (if (versao_bd == versao_atual))
        /// 3 . TESTAR E RETESTAR ANTES DE EMITIR UMA ATUALIZAÇÃO PARA OS CLIENTES, POIS QUALQUER ALTERAÇÃO MALFEITA PODE SER FATAL E HAVER A NECESSIDADE
        /// DE SAIR REINSTALANDO EM TROCENTOS CLIENTES!
        /// </summary>
        /// <param name="versao_bd"></param>
        public void migrar(int versao_bd)
        {
            string comando_sql = "";

            if (versao_bd == 0)
            {
                comando_sql =
                    "CREATE TABLE ESocial(" +
                        "id                         INTEGER             PRIMARY KEY     AUTOINCREMENT," +
                        "id_servidor                INTEGER             NOT NULL," +
                        "status                     INTEGER             NOT NULL," +
                        "data_emissao               DATETIME            NOT NULL," +
                        "cnpj                       CHAR (14)           NOT NULL        UNIQUE," +
                        "webService                 VARCHAR (255)       NOT NULL," +
                        "monitorarNFeEntrada        INTEGER             NOT NULL," +
                        "monitorarNFeEmitida        INTEGER             NOT NULL," +
                        "monitorarNFCeEmitida       INTEGER             NOT NULL," +
                        "monitorarCTe               INTEGER             NOT NULL" +
                    ")";

                efetuarMigracao(comando_sql, versao_bd);
                return;
            }

            if (versao_bd == 1)
            {
                //alterações da versao 1
                return;
            }

            if (versao_bd == 2)
            {
                //alterações da versao 2
                return;
            }

            if (versao_bd == 3)
            {
                //alterações da versao 3
                return;
            }

            //e por aí vai.. :)
        }

        private int pegarUltimaVersaoBancoDeDados()
        {
            string comando_sql = "SELECT versao FROM Informacao_db WHERE id = 1";
            this.Conector.Open();

            SQLiteCommand cmd = new SQLiteCommand(comando_sql, this.Conector);
            cmd.ExecuteNonQuery();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            this.Conector.Close();

            int versao = (Int32)ds.Tables[0].Rows[0]["versao"];
            return versao;
        }

        private void iniciaBancoDeDados()
        {
            string comando_sql =
                "CREATE TABLE Informacao_db(" +
                    "id INTEGER NOT NULL PRIMARY KEY," +
                    "versao INT NOT NULL" +
                ")";

            this.Conector.Open();

            SQLiteCommand cmd = new SQLiteCommand(comando_sql, this.Conector);
            cmd.ExecuteNonQuery();
            cmd.CommandText =
            "INSERT INTO Informacao_db(id, versao) " +
            "VALUES(1, -1)";
            cmd.ExecuteNonQuery();

            this.Conector.Close();
        }

        private void efetuarMigracao(string comando_sql, int versao_bd)
        {
            this.Conector.Open();
            SQLiteCommand cmd = new SQLiteCommand(comando_sql, this.Conector);

            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE Informacao_db SET versao = " + versao_bd + " WHERE id = 1";
            cmd.ExecuteNonQuery();

            this.Conector.Close();
        }

        public void Dispose()
        {
            this.Conector.Close();
        }
    }
}
