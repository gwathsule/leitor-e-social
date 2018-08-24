using Bilbliotecas.controlador;
using Bilbliotecas.sincronizador;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Xml;

namespace Leitor_Esocial
{
    class ESocialSincronizador : Sincronizador
    {
        private Principal janela_principal;
        private X509Certificate2 certificado;

        public ESocialSincronizador(string path, Principal janela_principal, X509Certificate2 certificado) : base(path)
        {
            this.janela_principal = janela_principal;
            this.certificado = certificado;
        }

        public override void arquivoAlterado(string path_file)
        {

        }

        public override void novoArquivo(string path_file)
        {
            try
            {
                ESocialControl controlador = new ESocialControl(path_file);
                string[] partes = path_file.Split('\\');
                if (Directory.Exists(@"C:\saida_esocial") == false)
                {
                    Directory.CreateDirectory(@"C:\saida_esocial");
                }


                XmlDocument xml_assinado = controlador.assinarXML(this.certificado);
                xml_assinado.Save(@"C:\saida_esocial\" + partes[partes.Length - 1]);
                this.janela_principal.adicionarLinhaTabelaExternamente(@"C:\saida_esocial\" + partes[partes.Length - 1], "assinada");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.janela_principal.adicionarLinhaTabelaExternamente(path_file, "nao assinada");
            }
        }
    }
}
