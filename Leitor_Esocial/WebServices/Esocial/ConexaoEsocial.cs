using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebServices.Esocial
{
    public static class ConexaoEsocial
    {
        private const string web_service_teste =
            "https://webservices.producaorestrita.esocial.gov.br/servicos/empregador/enviarloteeventos/WsEnviarLoteEventos.svc";

        private const string soap_action =
            "http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_1/ServicoEnviarLoteEventos/EnviarLoteEventos";

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
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(data);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                var urlServicoEnvio = web_service_teste;
                var address = new EndpointAddress(urlServicoEnvio);
                var binding = new BasicHttpsBinding();  //Disponível desde .NET Framework 4.5
                                                        // ou:
                                                        //var binding = new BasicHttpBinding(BasicHttpsSecurityMode.Transport);
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

                var wsClient = new WsEnviarRestrira.ServicoEnviarLoteEventos();
                wsClient.ClientCertificates.Add(certificado);

                var retornoEnvioXElement = wsClient.EnviarLoteEventos(xml.DocumentElement);
                return retornoEnvioXElement.InnerXml;
            }
            catch (WebException ex)
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
                return respostaServidor;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static string envolope_eventos(XmlDocument eventos_assinados)
        {
            string envelope_env = "";

            envelope_env += "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:v1=\"http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v2_4_02\">";
            envelope_env += "<soapenv:Header/>";
            envelope_env += "<soapenv:Body>";
            envelope_env += File.ReadAllText(@"C:\Users\Rafael\Documents\esocial\varios_eventos_assinados.xml");
            envelope_env += "</soapenv:Body>";
            envelope_env += "</soapenv:Envelope>";


            envelope_env = envelope_env.TrimEnd();
            envelope_env = envelope_env.TrimStart();

            envelope_env = envelope_env.Trim();

            return envelope_env;
        }
    }
}
