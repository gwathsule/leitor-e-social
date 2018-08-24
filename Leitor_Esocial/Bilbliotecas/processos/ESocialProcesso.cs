using Bilbliotecas.modelo;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using WebServices.Contando;

namespace Bilbliotecas.processos
{
    public class ESocialProcesso : Processo
    {
        Logs log;
        ConexaoContando contanto_wb;

        public ESocialProcesso(string nome_processo, int intervalo, NotifyIcon icone_notificacao) : 
            base(nome_processo, intervalo, icone_notificacao)
        {
            this.contanto_wb = new ConexaoContando();
            this.Thread.Start();
            this.log = new Logs(nome_processo);
        }

        public override void run()
        {
            try
            {
                log.log("Iniciando verificação no webservice");
                string retorno_servidor = contanto_wb.consultarXmls();
            }
            catch (Exception ex)
            {
                log.log("Erro: " + ex.Message);
            }

        }

        /// <summary>
        /// extrai os xmls compactados presente no servidor
        /// </summary>
        private List<ESocial> extrairXmlsRetornoServidor(string retorno_servidor)
        {
            return null;
        }
    }
}
