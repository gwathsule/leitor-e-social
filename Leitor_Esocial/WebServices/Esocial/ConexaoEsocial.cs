using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebServices.Esocial
{
    public static class ConexaoEsocial
    {
        private const string web_service_teste = "https://webservices.producaorestrita.esocial.gov.br/servicos/empregador/consultarloteeventos/WsConsultarLoteEventos.svc";
        
        //metodos privados
        private static string getRetornoServidor(string repostaServidor)
        {
            try
            {
                byte[] encodedString = Encoding.UTF8.GetBytes(repostaServidor);
                MemoryStream ms = new MemoryStream(encodedString);
                ms.Flush();
                ms.Position = 0;

                XmlDocument doc = new XmlDocument();
                doc.Load(ms);
                XmlNodeList elemList = doc.GetElementsByTagName("nfeRecepcaoEventoNFResult");
                return elemList.Item(0).InnerXml;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static string enviarRequisicao(string data, X509Certificate2 certificado)
        {
            try
            {
                ///
                //string data = "the xml document to submit";
                string url = web_service_teste;
                string response = "";

                // build request objects to pass the data/xml to the server
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "text/xml;charset=utf-8";
                request.ContentLength = buffer.Length;
                request.ClientCertificates.Add(certificado);
                Stream post = request.GetRequestStream();

                // post data and close connection
                post.Write(buffer, 0, buffer.Length);
                post.Close();

                // build response object
                HttpWebResponse resposta = request.GetResponse() as HttpWebResponse;
                Stream responsedata = resposta.GetResponseStream();
                StreamReader responsereader = new StreamReader(responsedata);
                response = responsereader.ReadToEnd();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //metodos públicos

        public static string processar_eventos(XmlDocument eventos_assinados, X509Certificate2 certificado)
        {
            try
            {
                string envelope = envolope_eventos(eventos_assinados);
                string respostaServidor = enviarRequisicao(envelope, certificado);
                return getRetornoServidor(respostaServidor);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static string envolope_eventos(XmlDocument eventos_assinados)
        {
            string envelope_env = "";

            envelope_env += "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:v1=\"http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0\">";
            envelope_env += "<soapenv:Header/>";
            envelope_env += "<soapenv:Body>";
            envelope_env += eventos_assinados.InnerXml;
            envelope_env += "</soapenv:Body>";
            envelope_env += "</soapenv:Envelope>";

            return envelope_env;
        }
    }
}
