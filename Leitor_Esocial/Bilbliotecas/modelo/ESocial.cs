using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bilbliotecas.modelo
{
    public class ESocial
    {
        /// <summary>
        /// 1 - 
        /// </summary>
        public int id { get; private set; }
        public int Status { get; set; }
        public int Id_servidor { get; set; }
        public DateTime Data { get; set; }
        public string Xml_base64 { get; set; }

        public ESocial() { }

        public ESocial(int status, int id_servidor, DateTime data, string xml_base64)
        {
            this.Status = status;
            this.Id_servidor = id_servidor;
            this.Data = data;
            this.Xml_base64 = xml_base64; 
        }

        public ESocial(int id, int status, int id_servidor, DateTime data, string xml_base64)
        {
            this.id = id;
            this.Status = status;
            this.Id_servidor = id_servidor;
            this.Data = data;
            this.Xml_base64 = xml_base64;
        }
    }
}
