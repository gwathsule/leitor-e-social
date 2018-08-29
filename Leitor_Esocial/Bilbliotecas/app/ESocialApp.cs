using Bilbliotecas.modelo;
using DataBase.dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bilbliotecas.app
{
    public static class ESocialApp
    {
        public static void novo(ESocial esocial, int user_id)
        {
            try
            {
                ESocialDAO.novo(esocial.Id_servidor, esocial.Id_empresa, esocial.Status, esocial.Ambiente, esocial.Data, esocial.Xml_base64, user_id);
            } catch (Exception ex)
            {
                throw new Exception("Erro ao salvar ESocial no banco: " + ex.Message);
            }
        }

        public static List<ESocial> getDocumentosNaoProcessados(int limite, int id_empresa_servidor)
        {
            try
            {
                List<ESocial> documentos = new List<ESocial>();
                DataSet dados = ESocialDAO.getDocumentosNaoProcessados(limite, id_empresa_servidor);

                foreach (DataRow dado in dados.Tables[0].Rows)
                {
                    int id = Int32.Parse(dado["id"].ToString());
                    int status = Int32.Parse(dado["status"].ToString());
                    int id_servidor = Int32.Parse(dado["id_servidor"].ToString());
                    int id_empresa = Int32.Parse(dado["id_empresa"].ToString());
                    int ambiente = Int32.Parse(dado["ambiente"].ToString());
                    DateTime data = dado.Field<DateTime>("data");
                    string xml_base64 = dado.Field<string>("xml_base64");
                    
                    ESocial documento = new ESocial(id, status, id_servidor, ambiente, id_empresa, data, xml_base64);
                    documentos.Add(documento);
                }

                return documentos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar documentos no banco: " + ex.Message);
            }
        }

        public static void marcarComoProcessado(ESocial documento, XmlDocument xml_assinado, XmlDocument resposta)
        {
            try
            {
                string xml_assinado_base64 = base64Encode(xml_assinado.InnerXml);
                string resposta_base64 = base64Encode(resposta.InnerXml);
                ESocialDAO.marcarComoProcessado(documento.id, xml_assinado_base64, resposta_base64);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static List<ESocial> getDocumentosProcessados(int limite, int id_empresa_servidor)
        {
            try
            {
                List<ESocial> documentos = new List<ESocial>();
                DataSet dados = ESocialDAO.getDocumentosProcessados(limite, id_empresa_servidor);

                foreach (DataRow dado in dados.Tables[0].Rows)
                {
                    int id = Int32.Parse(dado["id"].ToString());
                    int status = Int32.Parse(dado["status"].ToString());
                    int id_servidor = Int32.Parse(dado["id_servidor"].ToString());
                    int id_empresa = Int32.Parse(dado["id_empresa"].ToString());
                    int ambiente = Int32.Parse(dado["ambiente"].ToString());
                    DateTime data = dado.Field<DateTime>("data");
                    string xml_base64 = dado.Field<string>("xml_base64");
                    string resposta_base64 = dado.Field<string>("resposta_xml_base64");

                    ESocial documento = new ESocial(id, status, id_servidor, ambiente, id_empresa, data, xml_base64, resposta_base64);
                    documentos.Add(documento);
                }

                return documentos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar documentos no banco: " + ex.Message);
            }
        }

        public static string base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        internal static void marcarComoArmazenadoEmNuvem(int documento_id)
        {
            try
            {
                ESocialDAO.marcarComoProcessado(documento_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
