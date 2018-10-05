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
                        //apagar
                        User user = new User();
                        //
                        log.log("Iniciando verificação reposta de eventos na nuvem");
                        string retorno_servidor = contanto_wb.consultarXmlResposta(user.Id_servidor, user.Hash, user.Educont);
                        

                        JObject json_recebido = JObject.Parse(retorno_servidor);
                        int erro = (int)json_recebido["erro"];
                        string retorno = (string)json_recebido["retorno"];

                        log.log("resultado da API: erro = " + erro + " | retorno = " + retorno);

                        JArray dados = (JArray)json_recebido["dados"];

                        if (dados.Count > 0)
                        {
                            JObject json_envio = new JObject();
                            JArray array_envio = new JArray();

                            foreach(JObject documento in dados)
                            {
                                int idRegistro = (int)documento["id"];
                                string protocolo_envio = extrairProtocoloEnvio((string)documento["xml"]);
                                if(protocolo_envio.Equals("erro") == false)
                                {
                                    string resultado = ConexaoEsocial.consultarLoteEventos(protocolo_envio, user.Certificado);
                                    contanto_wb.//envia o resultado e o id de registro
                                }
                            }
                        }
                        else
                        {
                            log.log("nenhum documento novo encontrado");
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
                string protocoloEnvio = xml.GetElementsByTagName("protocoloEnvio").Item(0).InnerText;
                return protocoloEnvio;
            }
            catch(Exception ex)
            {
                this.log.log("erro ao carregar xml do servidor: " + ex.Message);
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

        private string base64decode(string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
    }
}
