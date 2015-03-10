using System;
using System.Security.Cryptography;

namespace WindowsFormsApplication1
{
    public sealed class RSAPKCS1SHA256SignatureDescription : SignatureDescription
    {
        public const string SignatureMethod = "http://" + "www.w3.org/2001/04/xmldsig-more#rsa-sha256";
        public const string ReferenceDigestMethod = "http://" + "www.w3.org/2001/04/xmlenc#sha256";

        public RSAPKCS1SHA256SignatureDescription()
        {
            KeyAlgorithm = typeof(RSACryptoServiceProvider).FullName;
            DigestAlgorithm = typeof(SHA256Managed).FullName;
            FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).FullName;
            DeformatterAlgorithm = typeof(RSAPKCS1SignatureDeformatter).FullName;
        }

        public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
        {
            if (key == null)
            {
                throw new Exception("Invalid key specified for RSAPKCS1SHA256SignatureDescription!");
            }

            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(key);
            deformatter.SetHashAlgorithm("SHA256");

            return deformatter;
        }

        public override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
        {
            if (key == null)
            {
                throw new Exception("Invalid key specified for RSAPKCS1SHA256SignatureDescription!");
            }

            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(key);
            formatter.SetHashAlgorithm("SHA256");

            return formatter;
        }
    }
}

