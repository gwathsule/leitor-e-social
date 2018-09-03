using Bilbliotecas.app;
using Bilbliotecas.controlador;
using Bilbliotecas.modelo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using WebServices.Contando;
using WebServices.Esocial;

namespace Bilbliotecas.processos
{
    public class ESocialProcesso : Processo
    {
        private Logs log;
        private ConexaoContando contanto_wb;
        public User user { get; set; }

        public ESocialProcesso(string nome_processo, int intervalo, NotifyIcon icone_notificacao, User user) : 
            base(nome_processo, intervalo, icone_notificacao)
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
                        string retorno_servidor = contanto_wb.consultarXmls(user.Id_servidor, user.Hash, user.Educont);
                        //salva os documentos no banco
                        List<ESocial> documentos_nao_processados = extrairXmlsRetornoServidor(retorno_servidor);
                        
                        if (documentos_nao_processados.Count > 0)
                        {
                            log.log(documentos_nao_processados.Count + " não processados encontrados. Iniciando processamento");
                            int doc_processados = processarDocumentosESocial(documentos_nao_processados);
                            if(doc_processados > 0 )
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

        private void subirDocumentosESocial(List<ESocial> documentos_processados)
        {
            int count = 1;
            int max = documentos_processados.Count;

            foreach (ESocial documento in documentos_processados)
            {
                this.log.log("Subindo " + count + " de " + max + "...");

                //envia informações do documento à nuvem
                string retorno_servidor = contanto_wb.enviarXmlAssinado(this.user.Id_servidor, this.user.Hash, 
                    documento.Id_servidor, documento.Xml_base64, documento.Resposta_xml_base64, documento.Ambiente, this.user.Educont);

                JObject json = JObject.Parse(retorno_servidor);

                int erro = (int)json["erro"];
                string retorno = (string)json["retorno"];

                this.log.log("Retorno do WB Contando: " + erro + " - " + retorno);

                if (erro == 0)
                {
                    ESocialApp.excluirLocal(documento.id);
                }
                count++;
            }
        }

        /// <summary>
        /// Processa os documentos assinando-os e enviando a receita
        /// </summary>
        private int processarDocumentosESocial(List<ESocial> documentos)
        {
            int count = 1;
            int max = documentos.Count;
            int doc_processado = 0;
            foreach(ESocial documento in documentos)
            {
                this.log.log("Processando " + count + " de " + max +"...");
                XmlDocument resposta = new XmlDocument();
                //assina e envia documento
                XmlDocument xml_assinado = ESocialControl.assinarXML(this.user.Certificado, documento.Xml_base64);
                string resposta_servidor = ConexaoEsocial.processar_eventos(xml_assinado, this.user.Certificado, documento.Ambiente);
                resposta.LoadXml(resposta_servidor);

                string cdResposta = resposta.GetElementsByTagName("cdResposta").Item(0).InnerText;
                string descResposta = resposta.GetElementsByTagName("descResposta").Item(0).InnerText;
                this.log.log("Resposta do servidor: " + cdResposta + " - " + descResposta);
                ESocialApp.novoDocumentoProcessado(documento, xml_assinado, resposta, this.user.Id);
                doc_processado++;
                count++;
            }

            return doc_processado;
        }



        /// <summary>
        /// extrai os xmls compactados presente no servidor
        /// </summary>
        private List<ESocial> extrairXmlsRetornoServidor(string retorno_servidor)
        {
            try
            {
                List<ESocial> documentos = new List<ESocial>();

                if (retorno_servidor.Equals(""))
                {
                    return documentos;
                }
                JObject json = JObject.Parse(retorno_servidor);

                int erro = (int)json["erro"];
                string retorno = (string)json["retorno"];

                this.log.log("Retorno do WB Contando: ");
                this.log.log("Erro: " + erro);
                this.log.log("retorno: " + retorno);

                if (erro == 0)
                {
                    JArray dados = (JArray)json["dados"];
                    this.log.log("Iniciando copia de novos xmls para o banco de dados");
                    foreach (JObject dado in dados)
                    {
                        int id_servidor = (int)dado["id"];
                        int id_empresa = (int)dado["id_empresa"];
                        int ambiente = (int)dado["ambiente"];
                        string data_str = (string)dado["data"];
                        string hora_str = (string)dado["hora"];
                        int assinado = (int)dado["assinado"];
                        string xml_base64 = (string)dado["xml"];

                        DateTime data = DateTime.ParseExact(data_str + " " + hora_str, "yyyy-MM-dd HH:mm:ss",
                                               System.Globalization.CultureInfo.InvariantCulture);

                        ESocial documento = new ESocial(assinado, id_servidor, ambiente, id_empresa, data, xml_base64);
                        documentos.Add(documento);
                    }
                }

                return documentos;
            } catch(Exception ex)
            {
                throw new Exception("Erro extrair os xmls do retorno do servidor: " + ex.Message);
            }
        }



        private void notificao_erro(string mensagem)
        {
            this.Icone_notificao.ShowBalloonTip(3,"Assinador ESocial", "Erro: " + mensagem, ToolTipIcon.Error);
        }

        private void notificao_info(string mensagem)
        {
            this.Icone_notificao.ShowBalloonTip(3, "Assinador ESocial", mensagem, ToolTipIcon.Info);
        }

    }
}
