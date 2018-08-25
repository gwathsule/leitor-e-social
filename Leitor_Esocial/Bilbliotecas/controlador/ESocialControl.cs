using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
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

                XmlDocument xml_assinado = assinarEsocial(certificado, xml_str);

                return xml_assinado;
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Utiliza o certificado digital do cliente para assinar as tags "infEvento" do xml passado como parametro
        /// </summary>
        private static XmlDocument assinarEsocial(X509Certificate2 certificado, string envelope_evento)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(envelope_evento);

            XmlNodeList list_eSocial = xml.GetElementsByTagName("eSocial");
            int i = 0;

            if (certificado.HasPrivateKey && certificado.NotAfter > DateTime.Now && certificado.NotBefore < DateTime.Now)
            {
                foreach (XmlElement eSocial in list_eSocial)
                {
                    if(i > 0)
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
    }
}
