using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace OnContabilLibrary.Models.Sistema
{
    public static class CertificadoDigital
    {
        private static readonly Dictionary<string, X509Certificate2> CacheCertificado = new Dictionary<string, X509Certificate2>();

        #region Métodos privados

        /// <summary>
        /// Cria e devolve um objeto <see cref="X509Store"/>
        /// </summary>
        /// <param name="openFlags"></param>
        /// <returns></returns>
        private static X509Store ObterX509Store(OpenFlags openFlags)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(openFlags);
            return store;
        }

        #region Métodos para obter um certificado X509Certificate2

        /// <summary>
        /// Obtém um certificado a partir do arquivo e da senha passados nos parâmetros
        /// </summary>
        /// <param name="arquivo">Arquivo do certificado digital</param>
        /// <param name="senha">Senha do certificado digital</param>
        /// <returns></returns>
        public static X509Certificate2 ObterDeArquivo(string arquivo, string senha)
        {
            if (!File.Exists(arquivo))
            {
                throw new Exception(String.Format("Certificado digital {0} não encontrado!", arquivo));
            }

            var certificado = new X509Certificate2(arquivo, senha, X509KeyStorageFlags.MachineKeySet);
            return certificado;
        }


        /// <summary>
        /// Obtém um certificado a partir do arquivo e da senha passados nos parâmetros
        /// </summary>
        /// <param name="arquivo">Arquivo do certificado digital</param>
        /// <param name="senha">Senha do certificado digital</param>
        /// <returns></returns>
        private static X509Certificate2 ObterDoArrayBytes(byte[] arrayBytes, string senha)
        {
            try
            {
                var certificado = new X509Certificate2(arrayBytes, senha, X509KeyStorageFlags.MachineKeySet);
                return certificado;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel converter o stream para o certificado.", ex);
            }
        }

        /// <summary>
        /// Obtém um objeto <see cref="X509Certificate2"/> pelo serial passado no parÂmetro
        /// </summary>
        /// <returns></returns>
        private static X509Certificate2 ObterDoRepositorio(string serial, OpenFlags opcoesDeAbertura)
        {
            if (string.IsNullOrEmpty(serial))
                throw new ArgumentException("O número de série do certificado digital não foi informado!");
            X509Certificate2 certificado = null;
            var store = ObterX509Store(opcoesDeAbertura);
            try
            {
                foreach (var item in store.Certificates)
                {
                    if (item.SerialNumber != null && item.SerialNumber.ToUpper().Equals(serial.ToUpper(), StringComparison.InvariantCultureIgnoreCase))
                        certificado = item;
                }

                if (certificado == null)
                    throw new Exception(string.Format("Certificado digital nº {0} não encontrado!", serial.ToUpper()));
            }
            finally
            {
                store.Close();
            }

            return certificado;
        }

        /// <summary>
        /// Obtém um objeto <see cref="X509Certificate2"/> pelo serial passado no parâmetro e com opção de definir o PIN
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        private static X509Certificate2 ObterDoRepositorioPassandoPin(string serial, string senha = null)
        {
            var certificado = ObterDoRepositorio(serial, OpenFlags.ReadOnly);
            if (string.IsNullOrEmpty(senha)) return certificado;
            certificado.DefinirPinParaChavePrivada(senha);
            return certificado;
        }

        #endregion

        /// <summary>
        /// Define o PIN para chave privada de um objeto <see cref="X509Certificate2"/> passado no parâmetro
        /// </summary>
        private static void DefinirPinParaChavePrivada(this X509Certificate2 certificado, string pin)
        {
            if (certificado == null) throw new ArgumentNullException("certificado");
            var key = (RSACryptoServiceProvider)certificado.PrivateKey;

            var providerHandle = IntPtr.Zero;
            var pinBuffer = Encoding.ASCII.GetBytes(pin);

            MetodosNativos.Executar(() => MetodosNativos.CryptAcquireContext(ref providerHandle,
                key.CspKeyContainerInfo.KeyContainerName,
                key.CspKeyContainerInfo.ProviderName,
                key.CspKeyContainerInfo.ProviderType,
                MetodosNativos.CryptContextFlags.Silent));
            MetodosNativos.Executar(() => MetodosNativos.CryptSetProvParam(providerHandle,
                MetodosNativos.CryptParameter.KeyExchangePin,
                pinBuffer, 0));
            MetodosNativos.Executar(() => MetodosNativos.CertSetCertificateContextProperty(
                certificado.Handle,
                MetodosNativos.CertificateProperty.CryptoProviderHandle,
                0, providerHandle));
        }

        #endregion

        /// <summary>
        /// Exibe a lista de certificados instalados no PC e devolve o certificado selecionado
        /// </summary>
        /// <returns></returns>
        public static X509Certificate2 ListareObterDoRepositorio()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certs = X509Certificate2UI.SelectFromCollection(
            store.Certificates , "Selecionar certificado", "Selecione um certificado para utilizar na assinatura do ESocial:"
            , X509SelectionFlag.MultiSelection);

                
            store.Close();
            if(certs[0] == null)
            {
                throw new Exception("Nenhum certificado selecionado.");
            }
            return certs[0];
        }

        public static X509Certificate2 getA1CertificadoWindows(string serialCertificado)
        {
            try
            {
                X509Certificate2 certificado = null;
                X509Store store = new X509Store("My", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                foreach (X509Certificate2 item in store.Certificates)
                {
                    if (item.SerialNumber.EndsWith(serialCertificado, StringComparison.InvariantCultureIgnoreCase))
                    {
                        certificado = item;
                        certificado.VerificaValidade();
                        break;
                    }
                }
                if (certificado == null)
                    throw new Exception("Certificado não encontrado na lista do windows");
                return certificado;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter certificado do windows: " + ex.Message, ex);
            }
        }

        private static bool VerifyPassword(byte[] fileContent, string password)
        {
            try
            {
                // ReSharper disable once UnusedVariable
                var certificate = new X509Certificate2(fileContent, password);
            }
            catch (CryptographicException ex)
            {
                if ((ex.HResult & 0xFFFF) == 0x56)
                {
                    return false;
                };

                throw;
            }

            return true;
        }

        public static X509Certificate2 getA3Certificado(string serialCertificado, string senha)
        {
            try
            {
                X509Certificate2 certificado = null;
                X509Store store = new X509Store("My", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                foreach (X509Certificate2 item in store.Certificates)
                {
                    if (item.SerialNumber.EndsWith(serialCertificado, StringComparison.InvariantCultureIgnoreCase))
                    {
                        certificado = item;
                        certificado.DefinirPinParaChavePrivada(senha);
                        break;
                    }
                }
                if (certificado == null)
                    throw new Exception("Certificado não encontrado na lista do windows");
                return certificado;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter certificado do windows: " + ex.Message, ex);
            }
        }


        public static void ClearCache()
        {
            CacheCertificado.Clear();
        }
    }

    internal static class MetodosNativos
    {
        internal enum CryptContextFlags
        {
            None = 0,
            Silent = 0x40
        }

        internal enum CertificateProperty
        {
            None = 0,
            CryptoProviderHandle = 0x1
        }

        internal enum CryptParameter
        {
            None = 0,
            KeyExchangePin = 0x20
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptAcquireContext(
            ref IntPtr hProv,
            string containerName,
            string providerName,
            int providerType,
            CryptContextFlags flags
            );

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CryptSetProvParam(
            IntPtr hProv,
            CryptParameter dwParam,
            [In] byte[] pbData,
            uint dwFlags);

        [DllImport("CRYPT32.DLL", SetLastError = true)]
        internal static extern bool CertSetCertificateContextProperty(
            IntPtr pCertContext,
            CertificateProperty propertyId,
            uint dwFlags,
            IntPtr pvData
            );

        public static void Executar(Func<bool> action)
        {
            if (!action())
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }

    public static class ExtensaoCertificadoDigital
    {
        /// <summary>
        /// Extenção para certificado digital
        /// <para>Verificar validade do certificado digital, se vencido dispara ArgumentException</para>
        /// </summary>
        /// <param name="x509Certificate2"></param>
        public static void VerificaValidade(this X509Certificate2 x509Certificate2)
        {
            DateTime dataExpiracao = Convert.ToDateTime(x509Certificate2.GetExpirationDateString());

            if (dataExpiracao <= DateTime.Now)
            {
                throw new ArgumentException("Certificado digital vencido na data => " + dataExpiracao);
            }
        }


        /// <summary>
        /// Extenção para certificado digital
        /// <para>Se usado ele retorna true se for um hardware, se for PenDriver ou SmartCard</para>
        /// </summary>
        /// <param name="x509Certificate2"></param>
        /// <returns>bool</returns>
        public static bool IsA3(this X509Certificate2 x509Certificate2)
        {
            if (x509Certificate2 == null)
                return false;

            bool result = false;

            try
            {
                RSACryptoServiceProvider service = x509Certificate2.PrivateKey as RSACryptoServiceProvider;

                if (service != null)
                {
                    if (service.CspKeyContainerInfo.Removable &&
                        service.CspKeyContainerInfo.HardwareDevice)
                        result = true;
                }
            }
            catch
            {
                //assume que é false
                result = false;
            }

            return result;
        }
    }
}
