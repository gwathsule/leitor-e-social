using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bilbliotecas.modelo
{
    public class Assinador
    {
        X509Certificate2 certificado_cache;

        //inicializa o assinador e só armazena o certificado em cache, quando ele for utilizado
        public Assinador()
        {
        }

        //inicializa o assinador, já armazenando o certificado em cache
        public Assinador(string senha)
        {
        }

        public XmlDocument assinarXml(XmlDocument xml)
        {
            return null;
        }
    }
}
