using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leitor_Esocial
{
    public partial class ModalLog : Form
    {
        private string nome_processo_esocial;
        private string nome_processo_consulta;

        public ModalLog()
        {
            this.nome_processo_esocial = "Processo ESocial";
            this.nome_processo_consulta = "Consulta lotes";
            InitializeComponent();
        }

        private void ModalLog_Shown(object sender, EventArgs e)
        {
            atualizaTexto();
        }

        private void atualizaTexto()
        {
            string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Contando");
            string pasta_logs = appDir + "\\log_processos\\" + this.nome_processo_esocial + "\\";
            DateTime hoje = DateTime.Now;
            string nome_file = hoje.Day.ToString() + "-" + hoje.Month.ToString() + "-" + hoje.Year.ToString() + ".log";
            nome_file = this.nome_processo_esocial + " ;; " + nome_file;
            string path_file = pasta_logs + nome_file;

            if (File.Exists(path_file))
            {
                this.richTextBox1.Text = File.ReadAllText(path_file);
            }
        }

        private void btn_atualiza_Click(object sender, EventArgs e)
        {
            atualizaTexto();
        }

        private void btn_atualiza_consulta_Click(object sender, EventArgs e)
        {
            string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Contando");
            string pasta_logs = appDir + "\\log_processos\\" + this.nome_processo_consulta + "\\";
            DateTime hoje = DateTime.Now;
            string nome_file = hoje.Day.ToString() + "-" + hoje.Month.ToString() + "-" + hoje.Year.ToString() + ".log";
            nome_file = this.nome_processo_consulta + " ;; " + nome_file;
            string path_file = pasta_logs + nome_file;

            if (File.Exists(path_file))
            {
                this.richTextBox1.Text = File.ReadAllText(path_file);
            }
        }
    }
}
