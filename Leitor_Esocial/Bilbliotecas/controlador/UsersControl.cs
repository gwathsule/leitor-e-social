using Bilbliotecas.app;
using Bilbliotecas.modelo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServices.Contando;
using System.Security.Cryptography.X509Certificates;
using OnContabilLibrary.Models.Sistema;

namespace Bilbliotecas.controlador
{
    public class UsersControl
    {
        public User User_logado { get; set; }
        private ConexaoContando wb_contando;
        public UsersControl()
        {
            this.wb_contando = new ConexaoContando();
        }

        public UsersControl(bool inicializar_com_user)
        {
            this.wb_contando = new ConexaoContando();
            inicializarUserLogado();
        }

        /// <summary>
        /// se existir algum user no BD, ele inicia o mesmo
        /// </summary>
        private void inicializarUserLogado()
        {
            try
            {
                this.User_logado = UserApp.getUser();
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public JArray logar(string email, string senha, string documento, bool educont)
        {
            try
            {
                string retorno_servidor = wb_contando.consultarEmpresas(email, senha, documento, educont);
                JObject json = JObject.Parse(retorno_servidor);
                int erro = (int)json["erro"];
                string retorno = (string)json["retorno"];
                if (erro == 0)
                {
                    return (JArray)json["dados"];
                }
                else
                {
                    throw new Exception(retorno);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void cadastrarUserLogado(IDictionary<string, string> empresa_info)
        {
            try
            {
                User user = new User(Int32.Parse(empresa_info["id"]), empresa_info["nome"], empresa_info["hash"], empresa_info["email"],
                                     empresa_info["senha"], empresa_info["documento"], 0);
                UserApp.salvarUser(user);
                this.User_logado = user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// salva as informações do certificado no user logado atual: 
        /// 1 - A1 arquivo | 
        /// 2 - A1 instalado no windows |
        /// 3 - A3
        /// </summary>
        public void salvarCertificadoUserLogado(X509Certificate2 certificado, int status, string caminho, string senha)
        {
            try
            {
                UserApp.salvarInfoCertificado(certificado, status, caminho, senha);
                this.User_logado.Certificado = certificado;
                this.User_logado.Status_certificado = status;
                this.User_logado.Caminho_certificado = caminho;
                this.User_logado.Senha_certificado = senha;
                this.User_logado.Serial_certificado = certificado.SerialNumber;

            } catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
