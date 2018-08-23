using Bilbliotecas.modelo;
using Bilbliotecas.sincronizador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leitor_Esocial
{
    public partial class Principal : Form
    {
        //variáveis do sistema
        private string enderecoWebService = "http://app.oncontabil.com.br/webService_v1/server";
        private Assinador assinador;
        private bool arrastando;
        private Point arrastando_cursor;
        private Point arrastando_form;


        //para fins de teste sem API
        private FolderBrowserDialog dlgDiretorioESocial;
        private ESocialSincronizador sincronizador;

        public Principal()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();

            //para fins de teste sem API
            this.dlgDiretorioESocial = new FolderBrowserDialog();
            this.iniciarSicronizador(@"E:\Rafael Projetos\esocial\teste");
        }


        //=============/ METODOS MANUAIS /===============//

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

        private void iniciarSicronizador(string caminho)
        {
            this.sincronizador = new ESocialSincronizador(caminho, this, this.assinador);
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
            if (this.dlgDiretorioESocial.ShowDialog() == DialogResult.OK)
            {
                string caminho = this.dlgDiretorioESocial.SelectedPath;
                this.iniciarSicronizador(caminho);
            }
        }
    }
}
