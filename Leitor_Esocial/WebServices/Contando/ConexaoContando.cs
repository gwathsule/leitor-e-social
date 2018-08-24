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
        private const string web_service = "https://esocial.contando.com.br/index.php/Api/";
        private const string id = "101";
        private const string hash = "e9437399bfb68ed926d4767604c8f4e3";
        private const string action = "consultarXml";

        public ConexaoContando()
        {
                
        }

        /// <summary>
        /// procura no web service contando, novos jsons para assinatura
        /// </summary>
        /// <returns></returns>
        public string consultarXmls()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                WebClient wc = new WebClient();

                wc.QueryString.Add("id", id);
                wc.QueryString.Add("hash", hash);
                wc.QueryString.Add("action", action);

                var data = wc.UploadValues(web_service, "POST", wc.QueryString);

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
