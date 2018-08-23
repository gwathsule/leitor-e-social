using Bilbliotecas.modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bilbliotecas.controlador
{
    public class ESocialControl
    {
        private string path_xml;

        public ESocialControl(string path_xml)
        {
            this.path_xml = path_xml;
        }

        public bool assinarXML(Assinador assinador)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(this.path_xml);
                assinador.assinarXml(xml);
                return true;
            } catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
