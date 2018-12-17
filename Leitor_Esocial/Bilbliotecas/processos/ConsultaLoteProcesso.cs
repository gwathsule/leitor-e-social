using Bilbliotecas.modelo;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using WebServices.Contando;
using WebServices.Esocial;

namespace Bilbliotecas.processos
{
    public class ConsultaLoteProcesso : Processo
    {
        private Logs log;
        private ConexaoContando contanto_wb;
        public User user { get; set; }

        public ConsultaLoteProcesso(string nome_processo, int intervalo, NotifyIcon icone_notificacao, User user) : base(nome_processo, intervalo, icone_notificacao)
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
                        //----------\  teste \---------------
                        /*User user = new User();
                        string path_cert = @"D:\projetos\certificados\SUPERMERCADO PESSIN LTDA ME18750973000103 [Certificado01].pfx";
                        user.Certificado = new System.Security.Cryptography.X509Certificates.X509Certificate2(path_cert, "Certificado01");
                        user.Id_servidor = 101;
                        user.Hash = "e9437399bfb68ed926d4767604c8f4e3";
                        user.Educont = false;*/
                        //-----------------------------

                        log.log("Iniciando verificação reposta de eventos na nuvem");
                        string retorno_servidor = contanto_wb.consultarXmlResposta(user.Id_servidor, user.Hash, user.Educont);

                        if (retorno_servidor.Equals("") == false)
                        {

                            JObject json_recebido = JObject.Parse(retorno_servidor);
                            int erro = (int)json_recebido["erro"];
                            string retorno = (string)json_recebido["retorno"];

                            log.log("resultado da API: erro = " + erro + " | retorno = " + retorno);

                            JArray dados = (JArray)json_recebido["dados"];

                            if (dados.Count > 0)
                            {
                                log.log(dados.Count + " documentos encontrados...");
                                JObject json_envio = new JObject();
                                JArray array_envio = new JArray();
                                int cont = 1;
                                foreach (JObject documento in dados)
                                {
                                    log.log("processando " + cont++ + " de " + dados.Count);
                                    int idRegistro = (int)documento["id"];
                                    int ambiente = (int)documento["ambiente"];
                                    //string protocolo_envio = extrairProtocoloEnvio((string)documento["xml"]);
                                    string protocolo_envio = "1.2.201812.0000000000047503996";
                                    if (protocolo_envio.Equals("erro") == false)
                                    {
                                        log.log("Protocolo encontrado: '" + protocolo_envio + "', iniciando procura no web service Esocial...");
                                        string resultado = ConexaoEsocial.consutarLoteEventos(ambiente, protocolo_envio, user.Certificado);
                                        XmlDocument resultado_xml = new XmlDocument();
                                        resultado_xml.LoadXml(resultado);
                                        string cd_resposta = resultado_xml.GetElementsByTagName("cdResposta").Item(0).InnerText;
                                        string desc_resposta = resultado_xml.GetElementsByTagName("descResposta").Item(0).InnerText;

                                        if (cd_resposta.Equals("201")) {
                                            log.log("sucesso código " + cd_resposta + ": " + desc_resposta);
                                            string resultado_base64 = base64Encode(resultado);
                                            string resultado_contando = contanto_wb.enviarXmlResultado(user.Id_servidor, user.Hash, idRegistro, resultado_base64, ambiente, user.Educont);
                                            JObject json_contando = JObject.Parse(resultado_contando);
                                            int resultado_erro = (int)json_contando["erro"];
                                            if (resultado_erro == 0)
                                            {
                                                log.log((string)json_contando["retorno"]);
                                            }
                                            else
                                            {
                                                log.log("Erro; " + (string)json_contando["retorno"]);
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                string codigo_ocorrencia = resultado_xml.GetElementsByTagName("codigo").Item(0).InnerText;
                                                string descricao_ocorrencia = resultado_xml.GetElementsByTagName("descricao").Item(0).InnerText;
                                                log.log("Erro código " + codigo_ocorrencia + ": " + descricao_ocorrencia);
                                            }
                                            catch (Exception)
                                            {
                                                log.log("Erro código " + cd_resposta + ": " + desc_resposta);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        log.log("tag protocolo não encontrado em documento");
                                    }
                                }
                            }
                            else
                            {
                                log.log("nenhum documento novo encontrado");
                            }
                        }
                        else
                        {
                            log.log("retorno vazio do servidor");
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

        private string extrairProtocoloEnvio(string xml_bas64)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(base64decode(xml_bas64));
                //--------| teste |---------
                //xml.Load(@"D:\projetos\leitorESocial\esocial_documentos\ConsultarLoteEventosEnvio_sucesso.xml");
                //--------------------------
                string protocoloEnvio = xml.GetElementsByTagName("protocoloEnvio").Item(0).InnerText;
                return protocoloEnvio;
            }
            catch(Exception)
            {
                return "erro";
            }
        }

        private void notificao_erro(string mensagem)
        {
            this.Icone_notificao.ShowBalloonTip(3, "Assinador ESocial", "Erro: " + mensagem, ToolTipIcon.Error);
        }

        private void notificao_info(string mensagem)
        {
            this.Icone_notificao.ShowBalloonTip(3, "Assinador ESocial", mensagem, ToolTipIcon.Info);
        }

        public static string base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private string base64decode(string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
    }
}
