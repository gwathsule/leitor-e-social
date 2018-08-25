using Bilbliotecas.processos;
using OnContabilLibrary.Models.Sistema;
using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Leitor_Esocial
{
    public partial class Principal : Form
    {
        //variáveis do sistema
        private X509Certificate2 certificado;
        private bool arrastando;
        private Point arrastando_cursor;
        private Point arrastando_form;
        private ESocialProcesso processo;

        //para fins de teste sem API
        private FolderBrowserDialog dlgDiretorioESocial;

        public Principal()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            //para fins de teste sem API
            this.dlgDiretorioESocial = new FolderBrowserDialog();
        }


        //=============/ METODOS MANUAIS /===============//
        
        private void iniciarProcessos()
        {
            if(processo == null)
            {
                processo = new ESocialProcesso(this.certificado, "Processo ESocial", 5, this.icon_principal);
            }
            else
            {
                if(processo.Thread.IsAlive == false)
                {
                    try
                    {
                        processo.Thread.Abort();
                    }catch(Exception){}
                    processo.Thread.Start();
                }
            }
        }

        private void fecharAplicacao(object sender, EventArgs e)
        {
            string pergunta = "Deseja realmente finalizar o Leitor Esocial?";
            DialogResult confirm = MessageBox.Show(pergunta, "Finalizar aplicação?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (confirm.ToString().ToUpper() == "YES")
            {
                Environment.Exit(0);
            }
                
        }

        private void adicionarLinhaTabela(string id, string status)
        {
            var index = eSocialDataGrid.Rows.Add();
            eSocialDataGrid.Rows[index].Cells["id"].Value = id;
            eSocialDataGrid.Rows[index].Cells["status"].Value = status;
        }

        public void adicionarLinhaTabelaExternamente(string id, string status)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => adicionarLinhaTabela(id, status)));
                return;
            }
        }

        private void abrirInterface(object sender, EventArgs e)
        {
            this.Show();
        }

        //=============/ METODOS AUTOMÁTICOS /===============//

        private void btnAdicionarEmpresa_Click(object sender, EventArgs e)
        {
            try
            {
               
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
        }

        private void btnConfigCNPJ_Click(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("Coleção foi modificada") == false)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnNFeEntrada_Click(object sender, EventArgs e)
        {
            try
            {
               
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listaCnpj_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                icon_principal.ShowBalloonTip(2, "Assinador ESocial", "Continuando em segundo plano", ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            try
            {
                ContextMenu menu_icon = new ContextMenu();
                menu_icon.MenuItems.Add(new MenuItem("Fechar Assinador", fecharAplicacao));
                menu_icon.MenuItems.Add(new MenuItem("Abrir interface", abrirInterface));
                icon_principal.ContextMenu = menu_icon;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Principal_Shown(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void icon_principal_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            arrastando = true;
            arrastando_cursor = Cursor.Position;
            arrastando_form = this.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (arrastando)
            {
                Point diferenca = Point.Subtract(Cursor.Position, new Size(arrastando_cursor));
                this.Location = Point.Add(arrastando_form, new Size(diferenca));
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            arrastando = false;
        }

        private void btn_folder_esocial_Click(object sender, EventArgs e)
        {
            if (this.certificado != null)
            {
                iniciarProcessos();
            }
            else
            {
                MessageBox.Show("Antes de iniciar, configure o certificado A3");
            }
        }

        private void btn_cnf_certificado_Click(object sender, EventArgs e)
        {
            try
            {
                string pedido = "Entre com a senha do certificado";
                X509Certificate2 certificado = CertificadoDigital.ListareObterDoRepositorio();
                string senha_a3 = Prompt.ShowDialog(pedido, "");
                CertificadoDigital.getA3Certificado(certificado.SerialNumber, senha_a3);
                this.certificado = certificado;
                MessageBox.Show("O certificado foi configurado com êxito");
            } catch(Exception ex)
            {
                if(ex.Message.Contains("Nome do parâmetro: index"))
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
