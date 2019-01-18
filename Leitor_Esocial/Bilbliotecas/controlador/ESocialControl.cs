using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml_str);

                List<XmlDocument> eventos = assinarEsocial(certificado, doc);
                XmlNodeList lista_eventos = doc.GetElementsByTagName("evento");

                for (int i = 0; i < eventos.Count; i++)
                {
                    XmlNode node = lista_eventos.Item(i);
                    XmlDocument evento = eventos[i];

                    node.RemoveChild(node.FirstChild);
                    node.InnerXml = evento.InnerXml;
                }

                return doc;
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Utiliza o certificado digital do cliente para assinar as tags "infEvento" do xml passado como parametro
        /// </summary>
        private static List<XmlDocument> assinarEsocial(X509Certificate2 certificado, XmlDocument xml_nao_assinado)
        {
            List<XmlDocument> eventos = new List<XmlDocument>();
            xml_nao_assinado = retirarAssinaturaAntiga(xml_nao_assinado);
            XmlNodeList lista_eventos = xml_nao_assinado.GetElementsByTagName("evento");

            //percorre as tags eventos
            if (certificado.HasPrivateKey && certificado.NotAfter > DateTime.Now && certificado.NotBefore < DateTime.Now)
            {
                foreach (XmlElement evento in lista_eventos)
                {
                    XmlDocument evento_separado = new XmlDocument();
                    evento_separado.LoadXml(evento.OuterXml);

                    XmlDocument esocial = new XmlDocument();
                    esocial.LoadXml(evento_separado.FirstChild.InnerXml);

                    XmlSignatureExtensions.signDocument(esocial, "", certificado);

                    eventos.Add(esocial);
                }
            }
            else
            {
                throw new Exception("Certificado digital ausente ou vencido");
            }

            return eventos;
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
