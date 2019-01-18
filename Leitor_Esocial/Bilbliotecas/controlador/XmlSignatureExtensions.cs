using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

/// <summary>
/// Classe retirada do link: https://arnhem.luminis.eu/signing-xml-before-net-4-6-2-and-after/
/// Onde explica como assinar um documento em algoritmo SHA256 em versões anteriores ao .NET 4.6.2
/// </summary>
namespace Bilbliotecas.controlador
{
    public static class XmlSignatureExtensions
    {
        public static void signDocument(this XmlDocument doc, string id, X509Certificate2 cert)
        {
            SignedXml signedXml = new SignedXml(doc);
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationUrl;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(new CspParameters() { Flags = CspProviderFlags.CreateEphemeralKey }))
            {
                rsa.FromXmlString(cert.PrivateKey.ToXmlString(true));
                signedXml.SigningKey = rsa;

                Reference reference = null;
                if (id.Equals(""))
                    reference = new Reference(String.Empty);
                else
                    reference = new Reference("#" + id);

                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                reference.AddTransform(new XmlDsigC14NTransform());
                reference.DigestMethod = SignedXml.XmlDsigSHA256Url;

                signedXml.AddReference(reference);

                signedXml.KeyInfo = new KeyInfo();
                signedXml.KeyInfo.AddClause(new KeyInfoX509Data(cert, X509IncludeOption.EndCertOnly));
                signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;

                signedXml.ComputeSignature();

                doc.DocumentElement.InsertAfter(doc.ImportNode(signedXml.GetXml(), true), doc.DocumentElement.LastChild);
            }
        }
    }
}
