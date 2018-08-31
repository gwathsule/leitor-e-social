using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bilbliotecas.modelo
{
    public class User
    {
        public int Id { get; set; }
        public int Id_servidor { get; set; }
        public string Nome{ get; set; }
        public string Hash { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Documento { get; set; }
        public bool Educont { get; set; }

        /// <summary>
        /// 0 - não confiurado |  
        /// 1 - A1 arquivo | 
        /// 2 - A1 pelo windows | 
        /// 3 - A3 pelo windows 
        /// </summary>
        public int Status_certificado { get; set; }
        public string Serial_certificado { get; set; }
        public string Caminho_certificado { get; set; }
        public string Senha_certificado { get; set; }
        public X509Certificate2 Certificado { get; set; }

        //contrutores com as propriedades obrigatórias (BD)
        public User(int id_servidor, string nome, string hash, string email, string senha, string documento, int status_certificado, bool educont)
        {
            this.Id_servidor = id_servidor;
            this.Nome = nome;
            this.Hash = hash;
            this.Email = email;
            this.Senha = senha;
            this.Documento = documento;
            this.Status_certificado = status_certificado;
            this.Educont = educont;
        }


        public User(int id, int id_servidor, string nome, string hash, string email, string senha, 
            string documento, int status_certificado, bool educont)
        {
            this.Id = id;
            this.Id_servidor = id_servidor;
            this.Nome = nome;
            this.Hash = hash;
            this.Email = email;
            this.Senha = senha;
            this.Documento = documento;
            this.Status_certificado = status_certificado;
            this.Educont = educont;
        }

        public User(int id, int id_servidor, string nome, string hash, string email, string senha, string documento, 
            int status_certificado, string serial_certificado, string caminho_certificado, string senha_certificado, bool educont)
        {
            this.Id = id;
            this.Id_servidor = id_servidor;
            this.Nome = nome;
            this.Hash = hash;
            this.Email = email;
            this.Senha = senha;
            this.Documento = documento;
            this.Status_certificado = status_certificado;
            this.Serial_certificado = serial_certificado;
            this.Caminho_certificado = caminho_certificado;
            this.Senha_certificado = senha_certificado;
            this.Educont = educont;
        }

        public User(){}
    }
}
