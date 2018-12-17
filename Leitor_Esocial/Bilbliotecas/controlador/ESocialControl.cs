using System;
using System.Collections.Generic;
using System.Deployment.Internal.CodeSigning;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

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

                List<XmlDocument> eventos = getEventosFromXml(xml_str);
                List<XmlDocument> eventos_assinados = new List<XmlDocument>();

                foreach (XmlDocument evento in eventos)
                {
                    eventos_assinados.Add(assinarEsocial(certificado, evento));
                }

                XmlDocument xmlAssinado = montaEnvelopeComEventosAssiandos(eventos_assinados, xml_str);

                return xmlAssinado;

            } catch (Exception ex)
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

            xml = retirarAssinaturaAntiga(xml);

            XmlNodeList list_eSocial = xml.GetElementsByTagName("evento");

            foreach (XmlElement evento in list_eSocial)
            {
                XmlDocument evento_doc = new XmlDocument();
                evento_doc.LoadXml(evento.OuterXml);
                lista_eventos.Add(evento_doc);
            }

            return lista_eventos;
        }

        private static void SignXmlDoc(XmlDocument xmlDoc, X509Certificate2 certificate)
        {
            SignedXml signedXml = new SignedXml(xmlDoc);
      
            signedXml.SigningKey = certificate.GetRSAPrivateKey();
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url; //"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"

            Reference reference = new Reference(string.Empty);

            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform());
            reference.DigestMethod = SignedXml.XmlDsigSHA256Url; //""http://www.w3.org/2001/04/xmlenc#sha256"
            signedXml.AddReference(reference);

            signedXml.KeyInfo = new KeyInfo();
            signedXml.KeyInfo.AddClause(new KeyInfoX509Data(certificate));
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
            //coloca o xml assinado após a tag "evtInfoEmpregador" na tag "eSocial"
            //XmlNode eSocial = xmlDoc.GetElementsByTagName("eSocial").Item(0);
            //eSocial.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
            if (xmlDoc.FirstChild is XmlDeclaration)
                xmlDoc.RemoveChild(xmlDoc.FirstChild);
        }

        /// <summary>
        /// Utiliza o certificado digital do cliente para assinar as tags "infEvento" do xml passado como parametro
        /// </summary>
        private static XmlDocument assinarEsocial(X509Certificate2 certificado, XmlDocument evento)
        {
            XmlDocument xml = new XmlDocument();
            xml = retirarAssinaturaAntiga(evento);
            XmlDocument eSocial = new XmlDocument();
            eSocial.LoadXml(xml.GetElementsByTagName("eSocial").Item(0).OuterXml);
            SignXmlDoc(eSocial, certificado);
            xml.GetElementsByTagName("evento")[0].InnerXml = "";
            xml.GetElementsByTagName("evento")[0].InnerXml = eSocial.OuterXml;
            return xml;

            /*
             
             XmlDocument xml = new XmlDocument();
            xml.LoadXml(str_xml_antigo);
            xml.GetElementsByTagName("eventos")[0].RemoveAll();

            string xmls_assinados = "";

            foreach (XmlDocument evento in eventos_assinados)
                xmls_assinados += evento.OuterXml;

            xml.GetElementsByTagName("eventos")[0].InnerXml = xmls_assinados;
             
             */
        }

        /// <summary>
        /// Utiliza o certificado digital do cliente para assinar as tags "infEvento" do xml passado como parametro
        /// </summary>
        private static XmlDocument assinarEsocialAntigo(X509Certificate2 certificado, string envelope_evento)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(envelope_evento);

            XmlNodeList list_eSocial = xml.GetElementsByTagName("eSocial");
            int i = 0;

            if (certificado.HasPrivateKey && certificado.NotAfter > DateTime.Now && certificado.NotBefore < DateTime.Now)
            {
                foreach (XmlElement eSocial in list_eSocial)
                {
                    if (i > 0)
                    {
                        string id = eSocial.FirstChild.Attributes["Id"].Value;

                        SignedXml signedXml = new SignedXml(eSocial);
                        signedXml.SigningKey = certificado.PrivateKey;

                        Reference reference = new Reference(string.Empty);
                        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                        reference.AddTransform(new XmlDsigC14NTransform());
                        signedXml.AddReference(reference);
                        signedXml.ComputeSignature();

                        //inclui cláusula com os dados do certificado
                        KeyInfo keyInfo = new KeyInfo();
                        keyInfo.AddClause(new KeyInfoX509Data(certificado));
                        signedXml.KeyInfo = keyInfo;

                        signedXml.ComputeSignature();

                        XmlElement xmlKeyInfo = signedXml.KeyInfo.GetXml();
                        XmlElement xmlSignature = xml.CreateElement("Signature", "http://www.w3.org/2000/09/xmldsig#");
                        XmlElement xmlSignedInfo = signedXml.SignedInfo.GetXml();

                        xmlSignature.AppendChild(xml.ImportNode(xmlSignedInfo, true));

                        XmlElement xmlSignatureValue = xml.CreateElement("SignatureValue", xmlSignature.NamespaceURI);
                        string signBase64 = Convert.ToBase64String(signedXml.Signature.SignatureValue);
                        XmlText text = xml.CreateTextNode(signBase64);
                        xmlSignatureValue.AppendChild(text);
                        xmlSignature.AppendChild(xmlSignatureValue);

                        xmlSignature.AppendChild(xml.ImportNode(xmlKeyInfo, true));

                        eSocial.AppendChild(xmlSignature);
                    }
                    i++;
                }

                return xml;
            }
            else
            {
                throw new Exception("Certificado digital ausente ou vencido");
            }
        }

        //retira a assinatura antiga do xml caso exista
        public static XmlDocument retirarAssinaturaAntiga(XmlDocument xml)
        {
            if(xml.GetElementsByTagName("Signature").Item(0) != null)
            {
                XmlNode node = xml.GetElementsByTagName("Signature")[0];
                XmlNode doc = xml.GetElementsByTagName("eSocial")[1].RemoveChild(node);
                return xml;
            }
            return xml;
        }
    }
}
