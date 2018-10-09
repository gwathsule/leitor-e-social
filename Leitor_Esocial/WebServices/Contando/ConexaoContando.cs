using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebServices.Contando
{
    public class ConexaoContando
    {
        private const string web_service_xmls = "https://esocial.contando.com.br/index.php/Api/";
        private const string web_service_login = "https://esocial.contando.com.br/index.php/Api/consultarEmpresas";
        private const string action_consultar_xml = "consultarXml";
        private const string action_consultar_xml_resposta = "consultarXmlResposta";
        private const string action_enviar_xml_assinado = "enviarXmlAssinado";
        private const string action_enviar_xml_resultado = "enviarXmlResultado";

        public ConexaoContando()
        {
                
        }

        /// <summary>
        /// procura no web service contando, novos jsons para assinatura
        /// </summary>
        /// <returns></returns>
        public string consultarXmls(int id, string hash, bool educont)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                WebClient wc = new WebClient();

                wc.QueryString.Add("id", id.ToString());
                wc.QueryString.Add("hash", hash);
                wc.QueryString.Add("action", action_consultar_xml);
                if (educont)
                    wc.QueryString.Add("educont", "1");

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

        public string consultarXmlResposta(int id_servidor, string hash, bool educont)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                WebClient wc = new WebClient();

                wc.QueryString.Add("id", id_servidor.ToString());
                wc.QueryString.Add("hash", hash);
                wc.QueryString.Add("action", action_consultar_xml_resposta);
                if (educont)
                    wc.QueryString.Add("educont", "1");

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

        public string enviarXmlAssinado(int id_empresa, string hash, int id_documento_servidor, string xml_assinado_base_64,
            string reposta_esocial_base64, int ambiente_documento, bool educont)
        {
            try
            {
                string postData = "";

                Dictionary<string, string> postParameters = new Dictionary<string, string>();

                postParameters.Add("action", action_enviar_xml_assinado);
                postParameters.Add("id", id_empresa.ToString());
                postParameters.Add("hash", hash);
                postParameters.Add("idRegistro", id_documento_servidor.ToString());
                postParameters.Add("xml", xml_assinado_base_64);
                postParameters.Add("resposta", reposta_esocial_base64);
                postParameters.Add("ambiente", ambiente_documento.ToString());
                if (educont)
                    postParameters.Add("educont", "1");

                foreach (string key in postParameters.Keys)
                {
                    postData += HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(postParameters[key]) + "&";
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(web_service_xmls);
                myHttpWebRequest.Method = "POST";

                byte[] data = Encoding.ASCII.GetBytes(postData);

                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                Stream responseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                string pageContent = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();

                myHttpWebResponse.Close();

                return pageContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string enviarXmlResultado(int id_empresa, string hash, int id_registro_servidor, string xml_resultado_base_64,
            int ambiente_documento, bool educont)
        {
            try
            {
                string postData = "";

                Dictionary<string, string> postParameters = new Dictionary<string, string>();

                postParameters.Add("action", action_enviar_xml_resultado);
                postParameters.Add("id", id_empresa.ToString());
                postParameters.Add("hash", hash);
                postParameters.Add("idRegistro", id_registro_servidor.ToString());
                postParameters.Add("resultado", xml_resultado_base_64);
                postParameters.Add("ambiente", ambiente_documento.ToString());
                if (educont)
                    postParameters.Add("educont", "1");

                foreach (string key in postParameters.Keys)
                {
                    postData += HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(postParameters[key]) + "&";
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(web_service_xmls);
                myHttpWebRequest.Method = "POST";

                byte[] data = Encoding.ASCII.GetBytes(postData);

                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                Stream responseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                string pageContent = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();

                myHttpWebResponse.Close();

                return pageContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// retorna informações do cliente, se esse existir no banco de dados (login)
        /// </summary>
        public string consultarEmpresas(string email, string senha, string documento, bool educont)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                WebClient wc = new WebClient();

                wc.QueryString.Add("email", email);
                wc.QueryString.Add("senha", senha);
                wc.QueryString.Add("documento", documento);

                if (educont)
                {
                    wc.QueryString.Add("educont", "1");
                }

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
