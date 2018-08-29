using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebServices.Contando
{
    public class ConexaoContando
    {
        private const string web_service_xmls = "https://esocial.contando.com.br/index.php/Api/";
        private const string web_service_login = "https://esocial.contando.com.br/index.php/Api/consultarEmpresas";
        private const string action_consultar_xml = "consultarXml";
        private const string action_enviar_xml_assinado = "enviarXmlAssinado";

        public ConexaoContando()
        {
                
        }

        /// <summary>
        /// procura no web service contando, novos jsons para assinatura
        /// </summary>
        /// <returns></returns>
        public string consultarXmls(int id, string hash)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                WebClient wc = new WebClient();

                wc.QueryString.Add("id", id.ToString());
                wc.QueryString.Add("hash", hash);
                wc.QueryString.Add("action", action_consultar_xml);

                var data = wc.UploadValues(web_service_xmls, "POST", wc.QueryString);

                // data here is optional, in case we recieve any string data back from the POST request.
                string responseString = UnicodeEncoding.UTF8.GetString(data);
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// envia um documento processado
        /// </summary>
        /// <returns></returns>
        public string enviarXmlAssinado(int id_empresa, string hash, int id_documento_servidor, string xml_assinado_base_64, 
            string reposta_esocial_base64, int ambiente_documento)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                WebClient wc = new WebClient();

                wc.QueryString.Add("id", id_empresa.ToString());
                wc.QueryString.Add("hash", hash);
                wc.QueryString.Add("action", action_enviar_xml_assinado);
                wc.QueryString.Add("idRegistro", id_documento_servidor.ToString());
                wc.QueryString.Add("xml", xml_assinado_base_64);
                wc.QueryString.Add("resposta", reposta_esocial_base64);
                wc.QueryString.Add("ambiente", ambiente_documento.ToString());

                var data = wc.UploadValues(web_service_xmls, "POST", wc.QueryString);

                // data here is optional, in case we recieve any string data back from the POST request.
                string responseString = UnicodeEncoding.UTF8.GetString(data);
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// retorna informações do cliente, se esse existir no banco de dados (login)
        /// </summary>
        public string consultarEmpresas(string email, string senha, string documento)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                WebClient wc = new WebClient();

                wc.QueryString.Add("email", email);
                wc.QueryString.Add("senha", senha);
                wc.QueryString.Add("documento", documento);

                var data = wc.UploadValues(web_service_login, "POST", wc.QueryString);

                // data here is optional, in case we recieve any string data back from the POST request.
                string responseString = UnicodeEncoding.UTF8.GetString(data);
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
