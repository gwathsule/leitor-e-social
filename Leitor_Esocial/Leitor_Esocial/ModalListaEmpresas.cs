using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leitor_Esocial
{
    public partial class ModalListaEmpresas : Form
    {
        struct empresa
        {
            public string nome;
            public string id;
            public string hash;

            public override string ToString()
            {
                return nome;
            }
        }

        public JObject Empresa_selecionada { get; set; }
        private empresa empresa_selecionada;

        public ModalListaEmpresas()
        {
            InitializeComponent();
        }

        public void atualizaListaEmpresas(JArray empresas)
        {
            this.list_empresas.Items.Clear();
            foreach(JObject empresa in empresas)
            {
                empresa nova = new empresa();
                nova.nome = (string)empresa["nome"];
                nova.id = (string)empresa["id"];
                nova.hash = (string)empresa["hash"];

                list_empresas.Items.Add(nova);
            }
        }

        public IDictionary<string, string> getInfoEmpresaSelecionada()
        {
            Dictionary<string, string> empresa_info = new Dictionary<string, string>();
            empresa_info.Add("nome", this.empresa_selecionada.nome);
            empresa_info.Add("id", this.empresa_selecionada.id);
            empresa_info.Add("hash", this.empresa_selecionada.hash);
            return empresa_info;
        }

        private void btn_selecionar_Click(object sender, EventArgs e)
        {
            this.empresa_selecionada = (empresa)list_empresas.SelectedItem;
            this.Close();
        }
    }
}
