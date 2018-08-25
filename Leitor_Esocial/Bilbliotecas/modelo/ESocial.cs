using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bilbliotecas.modelo
{
    public class ESocial
    {
        /// <summary>
        /// 0 - não assinada | 
        /// 1 - assinada e processada
        /// </summary>
        public int id { get; private set; }
        public int Status { get; set; }
        public int Id_servidor { get; set; }
        public int Id_empresa { get; set; }
        public int Ambiente { get; set; }
        public DateTime Data { get; set; }
        public string Xml_base64 { get; set; }

        public ESocial() { }

        public ESocial(int status, int id_servidor, int ambiente, int id_empresa, DateTime data, string xml_base64)
        {
            this.Status = status;
            this.Id_servidor = id_servidor;
            this.Id_empresa = id_empresa;
            this.Ambiente = ambiente;
            this.Data = data;
            this.Xml_base64 = xml_base64; 
        }

        public ESocial(int id, int status, int id_servidor, int ambiente, int id_empresa, DateTime data, string xml_base64)
        {
            this.id = id;
            this.Status = status;
            this.Id_servidor = id_servidor;
            this.Id_empresa = id_empresa;
            this.Ambiente = ambiente;
            this.Data = data;
            this.Xml_base64 = xml_base64;
        }
    }
}
