using Bilbliotecas.controlador;
using OnContabilLibrary.Models.Sistema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leitor_Esocial
{
    public partial class ModalConfig : Form
    {
        private UsersControl user_control;
        private OpenFileDialog dlgcertificadoArquivoCaminho;

        public ModalConfig(UsersControl user_control)
        {
            InitializeComponent();
            this.user_control = user_control;
            this.dlgcertificadoArquivoCaminho = new OpenFileDialog();
            this.dlgcertificadoArquivoCaminho.Title = "Selecione o certificado digital em formato arquivo.";
            this.dlgcertificadoArquivoCaminho.Filter = "Certificados|*.pfx; *.p12" +
                                                       "|Todos|*.*";
        }

        private void btn_certificado_arquivo_Click(object sender, EventArgs e)
        {
            string path_arquivo = "";
            try
            {
                if (dlgcertificadoArquivoCaminho.ShowDialog() == DialogResult.OK)
                {
                    path_arquivo = this.dlgcertificadoArquivoCaminho.FileName;
                }
                string senha = Prompt.ShowDialog("Insira a senha do certificado", "");
                X509Certificate2 certificado = CertificadoDigital.ObterDeArquivo(path_arquivo, senha);
                user_control.salvarCertificadoUserLogado(certificado, 1, path_arquivo, senha);
                MessageBox.Show("O certificado foi configurado com êxito");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao selecionar certificado: " + ex.Message);
            }
        }

        private void btn_certificado_windows_Click(object sender, EventArgs e)
        {
            try
            {
                X509Certificate2 certificado = CertificadoDigital.ListareObterDoRepositorio();
                if (certificado.IsA3())
                {
                    string senha = Prompt.ShowDialog("Entre com a senha do certificado", "");
                    certificado = CertificadoDigital.getA3Certificado(certificado.SerialNumber, senha);
                    certificado.VerificaValidade();
                    user_control.salvarCertificadoUserLogado(certificado, 3, "", senha);
                }
                else
                {
                    certificado.VerificaValidade();
                    user_control.salvarCertificadoUserLogado(certificado, 2, "", "");
                }
                MessageBox.Show("O certificado foi configurado com êxito");
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Nome do parâmetro: index"))
                    MessageBox.Show("Certificado não selecionado.");
                else
                    MessageBox.Show("Erro ao obter certificado: " + ex.Message);
            }
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string pedido, string defaut_value_text)
        {
            Form prompt = new Form();
            prompt.StartPosition = FormStartPosition.CenterParent;
            prompt.FormBorderStyle = FormBorderStyle.Sizable;
            prompt.MaximizeBox = false;
            prompt.MinimizeBox = false;
            prompt.Width = 330;
            prompt.Height = 140;
            prompt.Text = "";
            Label textLabel = new Label() { Left = 5, Top = 5, Text = pedido, Width = 320 };
            TextBox inputBox = new TextBox() { Left = 5, Top = 40, Width = 305, Text = defaut_value_text };
            inputBox.PasswordChar = '*';
            Button confirmation = new Button() { Text = "Ok", Left = 125, Width = 75, Top = 70 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.ShowDialog();
            return inputBox.Text;
        }
    }
}
