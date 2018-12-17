using Bilbliotecas.controlador;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Leitor_Esocial
{
    public partial class ModalLogin : Form
    {
        UsersControl user_control;
        ModalListaEmpresas modal_lista;

        public ModalLogin(UsersControl user_control)
        {
            InitializeComponent();
            this.user_control = user_control;
            this.modal_lista = new ModalListaEmpresas();
        }

        private void btn_logar_Click(object sender, EventArgs e)
        {
            try
            {
                JArray retorno = user_control.logar(txt_email.Text, txt_senha.Text, txt_documento.Text, chk_educont.Checked);
                modal_lista.atualizaListaEmpresas(retorno);
                modal_lista.ShowDialog();
                IDictionary<string, string> empresa_info = modal_lista.getInfoEmpresaSelecionada();
                empresa_info.Add("email", txt_email.Text);
                empresa_info.Add("senha", txt_senha.Text);
                empresa_info.Add("documento", txt_documento.Text);
                user_control.cadastrarUserLogado(empresa_info, chk_educont.Checked);
                MessageBox.Show("empresa '" + empresa_info["nome"] + "' logada com sucesso.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao realizar login: " + ex.Message);
            }
        }
    }
}
