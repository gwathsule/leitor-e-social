using Bilbliotecas.controlador;
using Bilbliotecas.modelo;
using Bilbliotecas.sincronizador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leitor_Esocial
{
    class ESocialSincronizador : Sincronizador
    {
        private Principal janela_principal;
        private Assinador assinador;

        public ESocialSincronizador(string path, Principal janela_principal, Assinador assinador) : base(path)
        {
            this.janela_principal = janela_principal;
            this.assinador = assinador;
        }

        public override void arquivoAlterado(string path_file)
        {

        }

        public override void novoArquivo(string path_file)
        {
            try
            {
                ESocialControl controlador = new ESocialControl(path_file);
                controlador.assinarXML(this.assinador);
                this.janela_principal.adicionarLinhaTabelaExternamente(path_file, "assinada");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.janela_principal.adicionarLinhaTabelaExternamente(path_file, "nao assinada");
            }
        }
    }
}
