using System;
using System.Security.Cryptography.X509Certificates;
using Bilbliotecas.modelo;
using DataBase.dao;
using System.Data;

namespace Bilbliotecas.app
{
    public static class UserApp
    {
        public static void salvarUser(User user)
        {
            try
            {
                UserDAO.salvarUser(user.Id_servidor, user.Nome, user.Hash, user.Email, 
                    user.Senha, user.Documento, user.Status_certificado, Convert.ToInt32(user.Educont));
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void salvarInfoCertificado(X509Certificate2 certificado, int status, string caminho, string senha)
        {
            try
            {
                if (status == 1)
                    UserDAO.salvarInfoCertificado(status, caminho, "", senha);
                else if (status == 2)
                    UserDAO.salvarInfoCertificado(status, "", certificado.SerialNumber, "");
                else if (status == 3)
                    UserDAO.salvarInfoCertificado(status, "", certificado.SerialNumber, senha);
                else
                    throw new Exception("tipo de certificado nao especificado");
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// retorna o usuário salvo no banco
        /// </summary>
        public static User getUser()
        {
            try
            {
                DataSet dados = UserDAO.getUser();

                if(dados.Tables[0].Rows.Count <= 0)
                {
                    return null;
                }

                User user = new User();

                user.Id = Int32.Parse(dados.Tables[0].Rows[0]["id"].ToString());
                user.Id_servidor = Int32.Parse(dados.Tables[0].Rows[0]["id_servidor"].ToString()); 
                user.Nome = dados.Tables[0].Rows[0].Field<string>("nome");
                user.Hash = dados.Tables[0].Rows[0].Field<string>("hash");
                user.Email = dados.Tables[0].Rows[0].Field<string>("email");
                user.Senha = dados.Tables[0].Rows[0].Field<string>("senha");
                user.Documento = dados.Tables[0].Rows[0].Field<string>("documento");
                user.Status_certificado = Int32.Parse(dados.Tables[0].Rows[0]["status_certificado"].ToString());
                user.Serial_certificado = dados.Tables[0].Rows[0].Field<string>("serial_certificado");
                user.Caminho_certificado = dados.Tables[0].Rows[0].Field<string>("caminho_certificado");
                user.Senha_certificado = dados.Tables[0].Rows[0].Field<string>("senha_certificado");
                string educont = dados.Tables[0].Rows[0]["educont"].ToString();
                if (educont.Equals(""))
                {
                    user.Educont = false;
                }
                else
                {
                    user.Educont = Int32.Parse(dados.Tables[0].Rows[0]["educont"].ToString()) != 0;
                }
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
