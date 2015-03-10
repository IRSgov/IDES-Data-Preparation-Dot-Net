using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace WindowsFormsApplication1
{
    class AesManager
    {
        private const int bufferSize = 1024 * 8;
        private const int keySize = 256;

        /// <summary>
        /// AES key size
        /// </summary>
        public static int KeySize
        {
            get
            {
                return keySize;
            }
        }

        /// <summary>
        /// Generates a random key of the specified size
        /// </summary>
        /// <param name="length">The length of the key to generate</param>
        /// <param name="emptyBytes">If true all bytes will contain zeroes</param>
        /// <returns>A random key represented as byte array</returns>
        public static byte[] GenerateRandomKey(int length, bool emptyBytes = false)
        {
            byte[] key = new byte[length];

            if (emptyBytes)
            {
                return key;
            }

            using (RNGCryptoServiceProvider random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(key);
            }

            return key;
        }

        /// <summary>
        /// Encrypts a file with AES
        /// </summary>
        /// <param name="filePath">Full path of the file to be encrypted</param>
        /// <param name="outputFilePath">Full path of the encrypted file</param>
        /// <param name="key">AES encryption key</param>
        /// <param name="iv">AES initialization vector</param>
        public static void EncryptFile(string filePath, string outputFilePath, byte[] key, byte[] iv)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.ECB;
                aes.Key = key;
                aes.IV = iv;


                using (ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (FileStream plain = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (FileStream encrypted = File.Open(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            using (CryptoStream cs = new CryptoStream(encrypted, cryptoTransform, CryptoStreamMode.Write))
                            {
                                plain.CopyTo(cs, bufferSize);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts a file encrypted with AES
        /// </summary>
        /// <param name="filePath">Full path of the file to be decrypted</param>
        /// <param name="outputFilePath">Full path of the decrypted file</param>
        /// <param name="key">AES decryption key</param>
        /// <param name="iv">AES initialization vector</param>
        public static void DecryptFile(string filePath, string outputFilePath, byte[] key, byte[] iv)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.ECB;
                aes.Key = key;
                aes.IV = iv;

                using (ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (FileStream encrypted = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (FileStream plain = File.Open(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            using (CryptoStream cs = new CryptoStream(plain, cryptoTransform, CryptoStreamMode.Write))
                            {
                                encrypted.CopyTo(cs, bufferSize);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts a key using the public key from the specified certificate
        /// </summary>
        /// <param name="payload">The data to encrypt</param>
        /// <param name="encryptionCert">The certificate used for encryption</param>
        /// <param name="encryptionCertPassword">The password for the encryption certificate</param>
        /// <param name="outputFilePath">Full path of the encrypted file</param>
        public static void EncryptAesKey(byte[] payload, string encryptionCert, string encryptionCertPassword, string outputFilePath)
        {
            X509Certificate2 cert = new X509Certificate2(encryptionCert, encryptionCertPassword);

            using (RSACryptoServiceProvider rsa = cert.PublicKey.Key as RSACryptoServiceProvider)
            {
                byte[] encryptedKey = rsa.Encrypt(payload, false);
                File.WriteAllBytes(outputFilePath, encryptedKey);
            }
        }

        /// <summary>
        /// Decrypts a key using the private key from the specified certificate
        /// </summary>
        /// <param name="payload">The data to decrypt</param>
        /// <param name="encryptionCert">The certificate used for decryption</param>
        /// <param name="encryptionCertPassword">The password for the decryption certificate</param>
        /// <returns>Decrypted key represented as byte array</returns>
        public static byte[] DecryptAesKey(byte[] payload, string encryptionCert, string encryptionCertPassword)
        {
            X509Certificate2 cert = new X509Certificate2(encryptionCert, encryptionCertPassword);

            if (!cert.HasPrivateKey)
            {
                // invalid certificate
                throw new Exception("Specified certificate not suitable for decryption!");
            }

            using (RSACryptoServiceProvider rsa = cert.PrivateKey as RSACryptoServiceProvider)
            {

                return rsa.Decrypt(payload, false);
            }
        }
    }
}
