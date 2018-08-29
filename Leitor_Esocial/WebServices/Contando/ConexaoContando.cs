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

        public string enviarXmlAssinado(int id_empresa, string hash, int id_documento_servidor, string xml_assinado_base_64,
            string reposta_esocial_base64, int ambiente_documento)
        {
            try
            {
                string strNewValue;
                string strResponse;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(web_service_xmls);

                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                strNewValue = "action={0}&id={1}&hash={2}&idRegistro={3}&xml={4}&resposta={5}&ambiente={6}";

                byte[] byteArray = Encoding.UTF8.GetBytes(string.Format(strNewValue, action_enviar_xml_assinado, id_empresa, 
                    hash, id_documento_servidor, xml_assinado_base_64, reposta_esocial_base64, ambiente_documento));
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                try
                {
                    HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                    Stream dataStream_resp = response.GetResponseStream();
                    StreamReader SR = new StreamReader(dataStream_resp, Encoding.UTF8);
                    strResponse = SR.ReadToEnd();
                    response.Close();
                    dataStream_resp.Close();
                    SR.Close();
                    req.Abort();
                    return strResponse;
                }
                catch (Exception e){ req.Abort(); throw e; }
            }catch (Exception ex)
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
                    wc.QueryString.Add("educont", "1");

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
