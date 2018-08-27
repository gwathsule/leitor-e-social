using Bilbliotecas.app;
using Bilbliotecas.controlador;
using Bilbliotecas.modelo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using WebServices.Contando;
using WebServices.Esocial;

namespace Bilbliotecas.processos
{
    public class ESocialProcesso : Processo
    {
        Logs log;
        ConexaoContando contanto_wb;
        X509Certificate2 certificado;

        public ESocialProcesso(X509Certificate2 certificado, string nome_processo, int intervalo, NotifyIcon icone_notificacao) : 
            base(nome_processo, intervalo, icone_notificacao)
        {
            this.certificado = certificado;
            this.contanto_wb = new ConexaoContando();
            this.log = new Logs(nome_processo);

            this.Thread.Start();
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
                        string retorno_servidor = contanto_wb.consultarXmls();
                        //salva os documentos no banco
                        extrairXmlsRetornoServidor(retorno_servidor);
                        //pega no máximo 15 xmls do banco para assinar
                        List<ESocial> documentos = ESocialApp.getDocumentosNaoProcessados(15);
                        if (documentos.Count > 0)
                        {
                            log.log(documentos.Count + " encontrados. Iniciando processamento");
                            processarDocumentosESocial(documentos);
                            notificao_info(documentos.Count + " novos documentos assinados");
                        }
                        else
                        {
                            log.log("nenhum documento novo encontrado");
                        }
                    } catch(Exception ex)
                    {
                        this.log.log("Erro no processo de sicronização: " + ex.Message);
                        notificao_erro(ex.Message);
                    }

                    //inicia espera
                    this.log.log("Iniciando aguardo de " + this.Intervalo_minutos + " antes de iniciar um novo upload");
                    this.log.log("==========================================//=========================================");
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
            catch (Exception ex)
            {
                log.log("ERRO CRÍTICO: " + ex.Message);
                notificao_erro("ERRO CRÍTICO: " + ex.Message);
            }

        }

        /// <summary>
        /// Processa os documentos assinando-os e enviando a receita
        /// </summary>
        private void processarDocumentosESocial(List<ESocial> documentos)
        {
            foreach(ESocial documento in documentos)
            {
                XmlDocument resposta = new XmlDocument();
                //assina e envia documento
                XmlDocument xml_assinado = ESocialControl.assinarXML(certificado, documento.Xml_base64);
                string resposta_servidor = ConexaoEsocial.processar_eventos(xml_assinado, this.certificado);
                resposta.LoadXml(resposta_servidor);
                
                //tratamento BD

                this.log.log(resposta_servidor);
            }
        }



        /// <summary>
        /// extrai os xmls compactados presente no servidor
        /// </summary>
        private void extrairXmlsRetornoServidor(string retorno_servidor)
        {
            try
            {
                List<ESocial> documentos = new List<ESocial>();
                JObject json = JObject.Parse(retorno_servidor);

                int erro = (int)json["erro"];
                string retorno = (string)json["retorno"];
                JArray dados = (JArray)json["dados"];

                this.log.log("Retorno do WB Contando: ");
                this.log.log("Erro: " + erro);
                this.log.log("retorno: " + retorno);

                if (erro == 0)
                {
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
                        ESocialApp.novo(documento);
                    }
                }
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
            this.Icone_notificao.ShowBalloonTip(3, "Assinador ESocial", "Erro: " + mensagem, ToolTipIcon.Info);
        }

    }
}
