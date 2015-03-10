using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// The type of signature to perform
    /// </summary>
    public enum XmlSignatureType
    {

        Enveloping
    }

    class XmlManager
    {
        #region Private methods
        /// <summary>
        /// Creates a KeyInfo object based on information from specified certificate
        /// </summary>
        /// <param name="certificate">The certificate used to create the KeyInfo from</param>
        /// <returns>KeyInfo object</returns>
        private static KeyInfo CreateKeyInfoFromCertificate(X509Certificate2 certificate)
        {
            // create KeyInfoX509Data object & include certificate subject
            KeyInfoX509Data kiData = new KeyInfoX509Data(certificate);
            kiData.AddSubjectName(certificate.Subject);

            // create KeyInfo object with specified KeyInfoX509Data
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(kiData);

            return keyInfo;
        }

        /// <summary>
        /// Gets the RSA private key from the specified signing certificate
        /// </summary>
        /// <param name="certificate">The certificate used to extract the key from</param>
        /// <returns>RSACryptoServiceProvider object</returns>
        private static RSACryptoServiceProvider GetSigningRsaKeyFromCertificate(X509Certificate2 certificate)
        {
            RSACryptoServiceProvider key = new RSACryptoServiceProvider(new CspParameters(24));
            key.FromXmlString(certificate.PrivateKey.ToXmlString(true));

            return key;
        }

        /// <summary>
        /// Loads the specified certificate using specific storage flags for SHA-256
        /// </summary>
        /// <param name="certFile">The certificate file to open</param>
        /// <param name="certPassword">The certificate password</param>
        /// <param name="signWithSha256">Sign the document using SHA-256</param>
        /// <returns>X509Certificate2 object</returns>
        private static X509Certificate2 LoadSigningCertificate(string certFile, string certPassword, bool signWithSha256)
        {
            X509Certificate2 certificate = null;

            if (signWithSha256)
            {
                // register SHA-256 and open certificate with exportable private key
                CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), RSAPKCS1SHA256SignatureDescription.SignatureMethod);
                certificate = new X509Certificate2(certFile, certPassword, X509KeyStorageFlags.Exportable);
            }
            else
            {
                // open certificate with password (to be able to access the private key)
                certificate = new X509Certificate2(certFile, certPassword);
            }

            return certificate;
        }

        /// <summary>
        /// Generates a SignedXml object
        /// </summary>
        /// <param name="doc">The XML data to sign represented as XmlDocument object</param>
        /// <param name="certificate">The certificate used for signing represented as X509Certificate2 object</param>
        /// <param name="signWithSha256">Sign the document using SHA-256</param>
        /// <returns></returns>
        private static SignedXml GenerateSignedXml(XmlDocument doc, X509Certificate2 certificate, bool signWithSha256)
        {
            // create new SignedXml from XmlDocument
            SignedXml signed = new SignedXml(doc);

            if (signWithSha256)
            {
                // set signing key and signature method for SHA-256
                signed.SigningKey = GetSigningRsaKeyFromCertificate(certificate);
                signed.SignedInfo.SignatureMethod = RSAPKCS1SHA256SignatureDescription.SignatureMethod;
            }
            else
            {
                // set signing key and signature method for SHA-1
                signed.SigningKey = certificate.PrivateKey;
            }

            return signed;
        }

        /// <summary>
        /// Signs a XML file (enveloped signature) using a digital certificate
        /// </summary>
        /// <param name="xml">The XML data to sign represented as byte array</param>
        /// <param name="certFile">The certificate file to use for signing</param>
        /// <param name="certPassword">The certificate password</param>
        /// <param name="signWithSha256">Sign the document using SHA-256</param>
        /// <returns>The signed data represented as byte array</returns>
        private static byte[] SignEnvelopedXml(byte[] xml, string certFile, string certPassword, bool signWithSha256)
        {
            if (xml == null || xml.Length == 0)
            {
                // invalid XML array
                throw new Exception("Nothing to sign!");
            }

            // load certificate
            X509Certificate2 certificate = LoadSigningCertificate(certFile, certPassword, signWithSha256);

            if (!certificate.HasPrivateKey)
            {
                // invalid certificate
                throw new Exception("Specified certificate not suitable for signing!");
            }

            using (MemoryStream stream = new MemoryStream(xml))
            {
                // go to the beginning of the stream
                stream.Flush();
                stream.Position = 0;

                // create new XmlDocument from stream
                XmlDocument doc = new XmlDocument() { PreserveWhitespace = true };
                doc.Load(stream);

                // create new SignedXml from XmlDocument
                SignedXml signed = GenerateSignedXml(doc, certificate, signWithSha256);

                // create reference & add enveloped transform
                Reference reference = new Reference(string.Empty);
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());

                if (signWithSha256)
                {
                    // SHA-256 digest
                    reference.DigestMethod = RSAPKCS1SHA256SignatureDescription.ReferenceDigestMethod;
                }

                // add reference to document
                signed.AddReference(reference);

                // include KeyInfo object & compute signature
                signed.KeyInfo = CreateKeyInfoFromCertificate(certificate);
                signed.ComputeSignature();

                // get signature & append node
                XmlElement xmlDigitalSignature = signed.GetXml();
                doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

                // returned content as byte array
                return Encoding.UTF8.GetBytes(doc.OuterXml);
            }
        }

        /// <summary>
        /// Signs a XML file (enveloping signature) using a digital certificate
        /// </summary>
        /// <param name="xml">The XML data to sign represented as byte array</param>
        /// <param name="certFile">The certificate file to use for signing</param>
        /// <param name="certPassword">The certificate password</param>
        /// <param name="signWithSha256">Sign the document using SHA-256</param>
        /// <returns>The signed data represented as byte array</returns>
        private static byte[] SignEnvelopingXml(byte[] xml, string certFile, string certPassword, bool signWithSha256)
        {
            if (xml == null || xml.Length == 0)
            {
                // invalid XML array
                throw new Exception("Nothing to sign!");
            }

            // load certificate
            X509Certificate2 certificate = LoadSigningCertificate(certFile, certPassword, signWithSha256);

            if (!certificate.HasPrivateKey)
            {
                // invalid certificate
                throw new Exception("Specified certificate not suitable for signing!");
            }

            using (MemoryStream stream = new MemoryStream(xml))
            {
                // go to the beginning of the stream
                stream.Flush();
                stream.Position = 0;

                // create new XmlDocument from stream
                XmlDocument doc = new XmlDocument() { PreserveWhitespace = true };
                doc.Load(stream);

                // craete transform (for canonicalization method & reference)
                XmlDsigExcC14NTransform transform = new XmlDsigExcC14NTransform();

                // create new SignedXml from XmlDocument
                SignedXml signed = GenerateSignedXml(doc, certificate, signWithSha256);
                signed.SignedInfo.CanonicalizationMethod = transform.Algorithm;

                // get nodes (use XPath to include FATCA declaration)
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/*");

                // define data object
                DataObject dataObject = new DataObject() { Data = nodes, Id = "FATCA" };

                // add the data we are signing as a sub-element (object) of the signature element
                signed.AddObject(dataObject);

                // create reference
                Reference reference = new Reference(string.Format("#{0}", dataObject.Id));
                reference.AddTransform(transform);

                if (signWithSha256)
                {
                    // SHA-256 digest
                    reference.DigestMethod = RSAPKCS1SHA256SignatureDescription.ReferenceDigestMethod;
                }

                // add reference to document
                signed.AddReference(reference);

                // include KeyInfo object & compute signature
                signed.KeyInfo = CreateKeyInfoFromCertificate(certificate);
                signed.ComputeSignature();

                // get signature
                XmlElement xmlDigitalSignature = signed.GetXml();

                // XML declaration
                string xmlDeclaration = string.Empty;

                if (doc.FirstChild is XmlDeclaration)
                {
                    // include declaration
                    xmlDeclaration = doc.FirstChild.OuterXml;
                }

                // return signature as byte array
                return Encoding.UTF8.GetBytes(string.Concat(xmlDeclaration, xmlDigitalSignature.OuterXml));
            }
        }
        #endregion Private methods

        /// <summary>
        /// Signs a XML file using a digital certificate
        /// </summary>
        /// <param name="signatureType">The type of signature to perform</param>
        /// <param name="xml">The XML data to sign represented as byte array</param>
        /// <param name="certFile">The certificate file to use for signing</param>
        /// <param name="certPassword">The certificate password</param>
        /// <param name="signWithSha256">Sign the document using SHA-256</param>
        /// <returns>The signed data represented as byte array</returns>
        public static byte[] Sign(XmlSignatureType signatureType, byte[] xml, string certFile, string certPassword, bool signWithSha256 = true)
        {
            switch (signatureType)
            {

                case XmlSignatureType.Enveloping:
                    return SignEnvelopingXml(xml, certFile, certPassword, signWithSha256);

                default:
                    throw new Exception("Please use a valid XML signature type!");
            }
        }

        /// <summary>
        /// Verifies if the signature of the file is valid
        /// </summary>
        /// <param name="signedXml">The path to the signed file</param>
        /// <returns>True or false</returns>
        public static bool CheckSignature(string signedXml)
        {
            byte[] xml = File.ReadAllBytes(signedXml);

            using (MemoryStream stream = new MemoryStream(xml))
            {
                // go to the beginning of the stream
                stream.Flush();
                stream.Position = 0;

                // create new XmlDocument from stream
                XmlDocument doc = new XmlDocument() { PreserveWhitespace = true };
                doc.Load(stream);

                // get signature node
                XmlNodeList nodeList = doc.GetElementsByTagName("Signature");

                if (nodeList.Count != 1)
                {
                    // invalid file
                    throw new Exception("Signature is missing or multiple signatures found!");
                }

                // create SignedXml and load it with data
                SignedXml signed = new SignedXml(doc);
                signed.LoadXml(nodeList[0] as XmlElement);

                // check
                return signed.CheckSignature();
            }
        }
    }
}