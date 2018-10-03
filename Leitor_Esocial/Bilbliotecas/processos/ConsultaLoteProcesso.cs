using Bilbliotecas.modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServices.Contando;

namespace Bilbliotecas.processos
{
    class ConsultaLoteProcesso : Processo
    {
        private Logs log;
        private ConexaoContando contanto_wb;
        public User user { get; set; }

        public ConsultaLoteProcesso(string nome_processo, int intervalo, NotifyIcon icone_notificacao) : base(nome_processo, intervalo, icone_notificacao)
        {
            this.user = user;
            this.contanto_wb = new ConexaoContando();
            this.log = new Logs(nome_processo);

            this.Thread.Start();
            this.notificao_info("processo de assinatura automática iniciada");
        }

        public override void run()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        log.log("Iniciando verificação no webservice");
                        string retorno_servidor = contanto_wb.consultarXmlResposta(user.Id_servidor, user.Hash, user.Educont);
                        //salva os documentos no banco
                        List<ESocial> documentos_nao_processados = extrairXmlsRetornoServidor(retorno_servidor);

                        /*
                        fazer struct
                        */


                        if (documentos_nao_processados.Count > 0)
                        {
                            log.log(documentos_nao_processados.Count + " não processados encontrados. Iniciando processamento");
                            int doc_processados = processarDocumentosESocial(documentos_nao_processados);
                            if (doc_processados > 0)
                                notificao_info(doc_processados + " novos documentos processados");
                        }
                        else
                        {
                            log.log("nenhum documento novo encontrado");
                        }

                        //pega no máximo 10 xmls processados no banco para envio ao servidor
                        List<ESocial> documentos_processados = ESocialApp.getDocumentosProcessados(10, user.Id_servidor);
                        if (documentos_processados.Count > 0)
                        {
                            log.log(documentos_processados.Count + " processados encontrados. Iniciando envio à nuvem");
                            subirDocumentosESocial(documentos_processados);
                            notificao_info(documentos_processados.Count + " novos documentos enviados ao servidor");
                        }
                        else
                        {
                            log.log("nenhum novo documento processado encontrado");
                        }

                    }
                    catch (Exception ex)
                    {
                        this.log.log("Erro no processo de sicronização: " + ex.Message);
                        notificao_erro(ex.Message);
                    }

                    //inicia espera
                    this.log.log("Iniciando aguardo de " + this.Intervalo_minutos + " antes de iniciar um novo upload" +
                                "\n====================//======================");
                    try
                    {
                        Thread.Sleep(this.Intervalo_minutos);
                    }
                    catch (ThreadAbortException)
                    {
                        this.log.log("Processo de " + this.log.Processo + " foi finalizada pelo usuário");
                        notificao_info("Processo de " + this.log.Processo + " foi finalizada pelo usuário");
                        return;
                    }
                }

            }
            catch (ThreadAbortException)
            {
                this.log.log("Processo abortado");
            }
            catch (Exception ex)
            {
                log.log("ERRO CRÍTICO: " + ex.Message);
                notificao_erro("ERRO CRÍTICO: " + ex.Message);
            }
        }

        private List<ESocial> extrairXmlsRetornoServidor(string retorno_servidor)
        {
            throw new NotImplementedException();
        }

        private void notificao_erro(string mensagem)
        {
            this.Icone_notificao.ShowBalloonTip(3, "Assinador ESocial", "Erro: " + mensagem, ToolTipIcon.Error);
        }

        private void notificao_info(string mensagem)
        {
            this.Icone_notificao.ShowBalloonTip(3, "Assinador ESocial", mensagem, ToolTipIcon.Info);
        }
    }
}
