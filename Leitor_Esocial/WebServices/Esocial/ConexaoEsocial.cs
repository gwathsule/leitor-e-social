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

        private const string web_service_producao =
            "https://webservices.envio.esocial.gov.br/servicos/empregador/enviarloteeventos/WsEnviarLoteEventos.svc";

        private const string wb_consultar_lote_eventos =
            "https://webservices.consulta.esocial.gov.br/servicos/empregador/consultarloteeventos/WsConsultarLoteEventos.svc";

        private const string wb_consultar_lote_eventos_restrito =
            "https://webservices.producaorestrita.esocial.gov.br/servicos/empregador/consultarloteeventos/WsConsultarLoteEventos.svc";

        private static string enviarRequisicaoAmbienteProducao(string data, X509Certificate2 certificado)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(data);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                var address = new EndpointAddress(web_service_producao);
                var binding = new BasicHttpsBinding();  
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

                var wsClient = new WsEnviarProducao.ServicoEnviarLoteEventos();
                wsClient.ClientCertificates.Add(certificado);

                var retornoEnvioXElement = wsClient.EnviarLoteEventos(xml.DocumentElement);
                return retornoEnvioXElement.InnerXml;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        private static string enviarRequisicaoAmbienteRestrito(string data, X509Certificate2 certificado)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(data);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                var address = new EndpointAddress(web_service_teste);
                var binding = new BasicHttpsBinding();
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

        public static string consutarLoteEventos(int ambiente, string protocolo_envio, X509Certificate2 certificado)
        {

            if(ambiente == 2 || ambiente == 3)
            {
                return consultarLoteEventosAmbienteRestrito(protocolo_envio, certificado);
            }
            else if (ambiente == 1)
            {
                return consultarLoteEventosAmbienteProducao(protocolo_envio, certificado);
            }

            return "ambiente desconhecido";
        }

        private static string consultarLoteEventosAmbienteProducao(string protocolo_envio, X509Certificate2 certificado)
        {
            string envelope = envelope_consultar_eventos(protocolo_envio);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(envelope);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            var address = new EndpointAddress(wb_consultar_lote_eventos);
            var binding = new BasicHttpsBinding();
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            var wsClient = new WsConsultarLoteEventos.ServicoConsultarLoteEventos();
            wsClient.ClientCertificates.Add(certificado);

            var retornoEnvioXElement = wsClient.ConsultarLoteEventos(xml.DocumentElement);
            return retornoEnvioXElement.InnerXml;
        }



        private static string consultarLoteEventosAmbienteRestrito(string protocolo_envio, X509Certificate2 certificado)
        {
            string envelope = envelope_consultar_eventos(protocolo_envio);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(envelope);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            var address = new EndpointAddress(wb_consultar_lote_eventos_restrito);
            var binding = new BasicHttpsBinding();
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            var wsClient = new WsConsultarLoteEventosRestrito.ServicoConsultarLoteEventos();
            wsClient.ClientCertificates.Add(certificado);

            var retornoEnvioXElement = wsClient.ConsultarLoteEventos(xml.DocumentElement);
            return retornoEnvioXElement.InnerXml;
        }

        public static string processar_eventos(XmlDocument eventos_assinados, X509Certificate2 certificado, int ambiente_documento)
        {
            try
            {
                string envelope = envolope_eventos(eventos_assinados);
                string respostaServidor = "";
                if (ambiente_documento == 1)
                    respostaServidor = enviarRequisicaoAmbienteProducao(envelope, certificado);
                else if (ambiente_documento == 2 || ambiente_documento == 3)
                    respostaServidor = enviarRequisicaoAmbienteRestrito(envelope, certificado);
                else
                    throw new Exception("ambiente do esocial desconhecido ao processar nota");
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
            envelope_env += eventos_assinados.InnerXml;
            envelope_env += "</soapenv:Body>";
            envelope_env += "</soapenv:Envelope>";


            envelope_env = envelope_env.TrimEnd();
            envelope_env = envelope_env.TrimStart();

            envelope_env = envelope_env.Trim();

            return envelope_env;
        }

        private static string envelope_consultar_eventos(string protocolo_envio)
        {
            string envelope_env = "";

            envelope_env += "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:v1=\"http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retornoProcessamento/v1_1_0\">";
            envelope_env += "<soapenv:Header/>";
            envelope_env += "<soapenv:Body>";
            envelope_env += "<v1:ConsultarLoteEventos>";
            envelope_env += "<v1:consulta>";
            envelope_env += "<eSocial xmlns=\"http://www.esocial.gov.br/schema/lote/eventos/envio/consulta/retornoProcessamento/v1_0_0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";
            envelope_env += "<consultaLoteEventos>";
            envelope_env += "<protocoloEnvio>" + protocolo_envio + "</protocoloEnvio>";
            envelope_env += "</consultaLoteEventos>";
            envelope_env += "</eSocial>";
            envelope_env += "</v1:consulta>";
            envelope_env += "</v1:ConsultarLoteEventos>";
            envelope_env += "</soapenv:Body>";
            envelope_env += "</soapenv:Envelope>";

            return envelope_env;
        }

    }
}
