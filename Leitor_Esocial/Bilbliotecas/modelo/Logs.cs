using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bilbliotecas.modelo
{
    class Logs
    {
        public string Processo { get; set; }
        public string Pasta_logs { get; set; }

        /// <summary>
        /// Gera a pasta de logs na caminho raiz da aplicação
        /// </summary>
        /// <param name="user"></param>
        /// <param name="processo"></param>
        public Logs(string processo)
        {
            this.Processo = processo;
        }

        private string caminhoArquivoLog()
        {
            if (this.Pasta_logs == null)
            {
                string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Contando");
                this.Pasta_logs = appDir + "\\log_processos\\" + this.Processo + "\\";
            }

            if (Directory.Exists(this.Pasta_logs) == false)
                Directory.CreateDirectory(this.Pasta_logs);

            DateTime hoje = DateTime.Now;
            string nome_file = hoje.Day.ToString() + "-" + hoje.Month.ToString() + "-" + hoje.Year.ToString() + ".log";

            if (this.Processo != null)
            {
                nome_file = this.Processo + " ;; " + nome_file;
            }

            return this.Pasta_logs + nome_file;
        }

        public void log(string msg)
        {
            //monta o cabeçalho do log com data, arquivo que gerou o erro e número da linha
            DateTime hoje = DateTime.Now;
            string cabecalho = hoje.ToString() + " | " + this.Processo + " | ";

            string caminho = caminhoArquivoLog();

            if (File.Exists(caminho) == false)
            {
                File.Create(@caminho).Close();

                using (StreamWriter writer = new StreamWriter(@caminho, true))
                {
                    writer.WriteLine("Data Hora | Tipo Processo | Menssagem de logs. Esse é o formato de erro desse arquivo de log.");
                    writer.WriteLine(cabecalho + msg);
                }
            }

            using (StreamWriter writer = new StreamWriter(@caminho, true))
                writer.WriteLine(cabecalho + msg);

            Console.WriteLine(cabecalho + msg);
        }
    }
}
