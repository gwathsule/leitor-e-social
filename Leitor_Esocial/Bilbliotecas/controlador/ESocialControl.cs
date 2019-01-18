using System;
using System.Collections.Generic;
using System.Deployment.Internal.CodeSigning;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Bilbliotecas.controlador
{
    public static class ESocialControl
    {
        public static XmlDocument assinarXML(X509Certificate2 certificado, string xml_base64)
        {
            try
            {
                byte[] data = Convert.FromBase64String(xml_base64);
                string xml_str = Encoding.UTF8.GetString(data);

                xml_str = xml_str.Replace("<v1:", "<");
                xml_str = xml_str.Replace("</v1:", "</");
                xml_str = xml_str.Replace("<V1:", "<");
                xml_str = xml_str.Replace("</V1:", "</");

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xml_str);

                XmlDocument xmlAssinado = assinarEvento(certificado, xml);

                return xmlAssinado;

            } catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Utiliza o certificado digital do cliente para assinar as tags "eSocial" das tags "Evento"
        /// </summary>
        private static XmlDocument assinarEvento(X509Certificate2 certificadoX509, XmlDocument xml)
        {
            // Variáveis utilizadas na assinatura;
            SignedXml signedXml = null;
            Reference reference = null;
            KeyInfo keyInfo = null;
            XmlElement sig = null;
            string signatureMethod = @"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
            string digestMethod = @"http://www.w3.org/2001/04/xmlenc#sha256";
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), signatureMethod);

            try
            {
                XmlNodeList list_eventos = xml.GetElementsByTagName("evento");

                if (certificadoX509.HasPrivateKey && certificadoX509.NotAfter > DateTime.Now && certificadoX509.NotBefore < DateTime.Now)
                {
                    foreach (XmlElement evento in list_eventos)
                    {
                        signedXml = new SignedXml(evento);
                        signedXml.SignedInfo.SignatureMethod = signatureMethod;

                        RSACryptoServiceProvider privateKey = (RSACryptoServiceProvider)certificadoX509.PrivateKey;

                        if (!privateKey.CspKeyContainerInfo.HardwareDevice)
                        {
                            CspKeyContainerInfo enhCsp = new RSACryptoServiceProvider().CspKeyContainerInfo;
                            CspParameters cspparams = new CspParameters(enhCsp.ProviderType, enhCsp.ProviderName, privateKey.CspKeyContainerInfo.KeyContainerName);
                            if (privateKey.CspKeyContainerInfo.MachineKeyStore)
                            {
                                cspparams.Flags |= CspProviderFlags.UseMachineKeyStore;
                            }
                            privateKey = new RSACryptoServiceProvider(cspparams);
                        }

                        // Adicionando a chave privada para assinar o documento
                        signedXml.SigningKey = privateKey;

                        // Referenciando o identificador da tag que será assinada
                        reference = new Reference(String.Empty);
                        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(false));
                        reference.AddTransform(new XmlDsigC14NTransform(false));
                        reference.DigestMethod = digestMethod;

                        // Adicionando a referencia de qual tag será assinada
                        signedXml.AddReference(reference);

                        // Adicionando informações do certificado na assinatura
                        keyInfo = new KeyInfo();
                        keyInfo.AddClause(new KeyInfoX509Data(certificadoX509));
                        signedXml.KeyInfo = keyInfo;

                        // Calculando a assinatura
                        signedXml.ComputeSignature();

                        // Adicionando a tag de assinatura ao documento xml
                        sig = signedXml.GetXml();
                        evento.GetElementsByTagName("eSocial").Item(0).AppendChild(sig);
                        return xml;
                    }
                }
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static XmlDocument montaEnvelopeComEventosAssiandos(List<XmlDocument> eventos_assinados, string str_xml_antigo)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(str_xml_antigo);
            xml.GetElementsByTagName("eventos")[0].RemoveAll();

            string xmls_assinados = "";

            foreach (XmlDocument evento in eventos_assinados)
                xmls_assinados += evento.OuterXml;

            xml.GetElementsByTagName("eventos")[0].InnerXml = xmls_assinados;

            return xml;
        }

        private static List<XmlDocument> getEventosFromXml(string xml_str)
        {
            List <XmlDocument> lista_eventos = new List<XmlDocument>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xml_str);

            XmlNodeList list_eSocial = xml.GetElementsByTagName("evento");

            foreach (XmlElement evento in list_eSocial)
            {
                XmlDocument evento_doc = new XmlDocument();
                evento_doc.LoadXml(evento.OuterXml);
                lista_eventos.Add(evento_doc);
            }

            return lista_eventos;
        }
    }
}
