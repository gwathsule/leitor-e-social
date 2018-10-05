using Bilbliotecas.app;
using Bilbliotecas.controlador;
using Bilbliotecas.modelo;
using Bilbliotecas.processos;
using OnContabilLibrary.Models.Sistema;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;

namespace Leitor_Esocial
{
    public partial class Principal : Form
    {
        //variáveis do sistema
        private ESocialProcesso processo_esocial;
        private ConsultaLoteProcesso processo_consulta;
        private ModalConfig modal_config;
        private UsersControl user_control;
        private ModalLogin modal_login;
        private ModalLog modal_log;

        //variáveis form
        private bool arrastando;
        private Point arrastando_cursor;
        private Point arrastando_form;

        //para fins de teste sem API
        private FolderBrowserDialog dlgDiretorioESocial;

        public Principal()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            inicializaAutomatico();
            this.modal_log = new ModalLog();
            atualizaProcessos();
            //para fins de teste sem API
            this.dlgDiretorioESocial = new FolderBrowserDialog();
        }


        //=============/ METODOS MANUAIS /===============//

        private void atualizarTabela()
        {
            if (this.user_control.User_logado != null)
            {
                List<ESocial> ultimos = ESocialApp.getDocumentosUltimosDocumentos(this.user_control.User_logado.Id_servidor, 30);

                eSocialDataGrid.Rows.Clear();

                foreach (ESocial documento in ultimos)
                {
                    var index = eSocialDataGrid.Rows.Add();
                    eSocialDataGrid.Rows[index].Cells["id"].Value = documento.Id_servidor;
                    eSocialDataGrid.Rows[index].Cells["data"].Value = documento.Data.ToString();

                    switch (documento.Status)
                    {
                        case 0:
                            eSocialDataGrid.Rows[index].Cells["status"].Value = "nao assinada";
                            break;
                        case 1:
                            eSocialDataGrid.Rows[index].Cells["status"].Value = "processada";
                            break;
                        case 2:
                            eSocialDataGrid.Rows[index].Cells["status"].Value = "em nuvem";
                            break;
                        default:
                            eSocialDataGrid.Rows[index].Cells["status"].Value = "indefinido";
                            break;
                    }
                }
            }
        }

        private void inicializaAutomatico()
        {
            try
            {
                try
                {
                    this.user_control = new UsersControl(true);
                    if (this.user_control.User_logado != null)
                    {
                        if (this.user_control.User_logado.Status_certificado == 1)
                        {
                            this.user_control.User_logado.Certificado = new X509Certificate2(this.user_control.User_logado.Caminho_certificado, this.user_control.User_logado.Senha_certificado);
                        }
                        else if (this.user_control.User_logado.Status_certificado == 2)
                        {
                            this.user_control.User_logado.Certificado = CertificadoDigital.getA1CertificadoWindows(this.user_control.User_logado.Serial_certificado);
                        }
                        else if (this.user_control.User_logado.Status_certificado == 3)
                        {
                            this.user_control.User_logado.Certificado = CertificadoDigital.getA3Certificado(this.user_control.User_logado.Serial_certificado, this.user_control.User_logado.Senha_certificado);
                        }

                        if (this.user_control.User_logado.Certificado != null)
                        {
                            try
                            {
                                this.user_control.User_logado.Certificado.VerificaValidade();
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Erro no certificado: " + e.Message);
                                this.user_control.User_logado.Certificado = null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar user local: " + ex.Message);
                    this.user_control = new UsersControl();
                }
                this.modal_config = new ModalConfig(this.user_control);
                this.modal_login = new ModalLogin(this.user_control);
                atualizarInterface();
            } catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar user local: " + ex.Message);
            }
        }

        private void atualizarInterface()
        {
            if(user_control.User_logado != null)//usuario logado
            {
                btn_login.Text = "Mudar Usuário";
                lbl_nome_usuario.Text = user_control.User_logado.Nome;
                if(user_control.User_logado.Certificado != null)
                {
                    string nome_cert = user_control.User_logado.Certificado.SubjectName.Name;
                    if(nome_cert.Length > 30)
                    {
                        nome_cert = nome_cert.Substring(0, 29);
                    }
                    if (user_control.User_logado.Status_certificado == 1)
                        lbl_status_certificado.Text = "A1 (Arquivo) - " + nome_cert;
                    if (user_control.User_logado.Status_certificado == 2)
                        lbl_status_certificado.Text = "A1 (Windows)- " + nome_cert;
                    if (user_control.User_logado.Status_certificado == 3)
                        lbl_status_certificado.Text = "A3 - " + nome_cert;
                } else
                {
                    lbl_status_certificado.Text = "não configurado";
                }
                atualizarTabela();
            }
            else
            {
                lbl_status_sincronizador.Text = "desligado";
                lbl_status_certificado.Text = "não configurado";
                lbl_nome_usuario.Text = "nenhum usuário logado";
            }
        }

        private void atualizaProcessos()
        {
            try
            {
                processo_consulta = new ConsultaLoteProcesso("Consulta lotes", 5, this.icon_principal, this.user_control.User_logado);

                if (this.user_control.User_logado == null || this.user_control.User_logado.Certificado == null)
                {
                    lbl_status_sincronizador.Text = "desligado";
                    return;
                }

                atualizarTabela();

                if (processo_esocial == null)
                {
                    processo_esocial = new ESocialProcesso("Processo ESocial", 5, this.icon_principal, this.user_control.User_logado);
                }
                else
                {
                    try { processo_esocial.Thread.Abort(); } catch (Exception) { }
                    processo_esocial = new ESocialProcesso("Processo ESocial", 5, this.icon_principal, this.user_control.User_logado);
                }

                if (processo_consulta == null)
                {
                    processo_consulta = new ConsultaLoteProcesso("Consulta lotes", 5, this.icon_principal, this.user_control.User_logado);
                }
                else
                {
                    try { processo_consulta.Thread.Abort(); } catch (Exception) { }
                    processo_consulta = new ConsultaLoteProcesso("Consulta lotes", 5, this.icon_principal, this.user_control.User_logado);
                }

                lbl_status_sincronizador.Text = "em execução";
            }catch(Exception ex)
            {
                MessageBox.Show("Erro ao iniciar processo de assinaturas: " + ex.Message);
                lbl_status_sincronizador.Text = "desligado";
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
            atualizarTabela();
            this.Show();
        }

        //=============/ METODOS AUTOMÁTICOS /===============//

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

        private void btn_cnf_certificado_Click(object sender, EventArgs e)
        {
            if (this.user_control.User_logado == null)
            {
                MessageBox.Show("usuário deve estar logado");
                return;
            }
            this.modal_config.ShowDialog();
            atualizarInterface();
            atualizaProcessos();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            this.modal_login.ShowDialog();
            this.atualizarInterface();
            atualizaProcessos();
        }

        private void btnAjuda_Click(object sender, EventArgs e)
        {
            this.modal_log.ShowDialog();
        }

        private void btn_limpar_notas_Click(object sender, EventArgs e)
        {
            if (this.user_control.User_logado == null)
            {
                MessageBox.Show("usuário deve estar logado");
                return;
            } else
            {
                user_control.limparNotasUser(this.user_control.User_logado.Id);
                atualizarTabela();
            }
        }
    }
}
