using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

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
        /// <param name="useECBMode">boolean to determine if ECB or CBC mode is used</param>
        public static void EncryptFile(string filePath, string outputFilePath, byte[] key, byte[] iv, bool useECBMode)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                if(useECBMode)
                    aes.Mode = CipherMode.ECB;
                else
                    aes.Mode = CipherMode.CBC;

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
        public static void DecryptFile(string filePath, string outputFilePath, byte[] key, byte[] iv, bool useECBMode)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                if (useECBMode)
                    aes.Mode = CipherMode.ECB;
                else
                    aes.Mode = CipherMode.CBC;
               
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
        /// <param name="payloadIV">The IV to encrypt - ONLY FOR CBC</param>
        /// <param name="encryptionCert">The certificate used for encryption</param>
        /// <param name="encryptionCertPassword">The password for the encryption certificate</param>
        /// <param name="outputFilePath">Full path of the encrypted file</param>
        /// <param name="useECBMode">boolean to determine if ECB or CBC mode is used</param>
        public static void EncryptAesKey(byte[] payload, byte[] payloadIV, string encryptionCert, string encryptionCertPassword, string outputFilePath, bool useECBMode)
        {
            X509Certificate2 cert = new X509Certificate2(encryptionCert, encryptionCertPassword);

            //If we are not using ECB mode, we need to add the IV to the key before we encrypt
            if(useECBMode != true)
            {
                payload = payload.Concat(payloadIV).ToArray();
            }    

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

        /// <summary>
        /// This will decrypt the payload from a downloaded and decompressed notification 
        /// </summary>
        /// <param name="xmlProcessingFolder">The path to folder that contains the decompressed notification files</param>
        /// <param name="decryptionKey">The key file used to decrypt the AES key</param>
        /// <param name="decryptionPass">The password to the key file above</param>
        /// <param name="isECB">Determines the cipher mode, CBC or ECB</param>
        /// <returns>the file path to the decrypted payload file</returns>
        public static string DecryptNotification(string xmlProcessingFolder, string decryptionKey, string decryptionPass, bool isECB)
        {
            // select encrypted key file
            string encryptedKeyFile = "";
            string encryptedPayloadFile = "";
            string metadataFile = "";
            string[] keyFiles = Directory.GetFiles(xmlProcessingFolder, "*_Key", SearchOption.TopDirectoryOnly);
            string[] payloadFiles = Directory.GetFiles(xmlProcessingFolder, "*_Payload", SearchOption.TopDirectoryOnly);
            string[] metadataFiles = Directory.GetFiles(xmlProcessingFolder, "*_Metadata*", SearchOption.TopDirectoryOnly);

            if (keyFiles.Length == 0)
            {
                // key file validation
                throw new Exception("There was no file found containing the encrypted AES key!");
            }
            if (payloadFiles.Length == 0)
            {
                // key file validation
                throw new Exception("There was no file found containing the encrypted Payload!");
            }
            if (metadataFiles.Length == 0)
            {
                // key file validation
                throw new Exception("There was no file found containing the Metadata!");
            }

            encryptedKeyFile = keyFiles[0];
            encryptedPayloadFile = payloadFiles[0];
            metadataFile = metadataFiles[0];

            //Check the metadata and see what we have            
            string metadataContentType = XmlManager.CheckMetadataType(metadataFile);

            byte[] encryptedAesKey = null;
            byte[] decryptedAesKey = null;
            byte[] aesVector = null;
            string decryptedPayload = "";

            // load encrypted AES key
            encryptedAesKey = File.ReadAllBytes(encryptedKeyFile);

            // decrypt AES key & generate default (empty) initialization vector
            decryptedAesKey = AesManager.DecryptAesKey(encryptedAesKey, decryptionKey, decryptionPass);
            aesVector = AesManager.GenerateRandomKey(16, true);
            if (isECB != true)
            {
                aesVector = decryptedAesKey.Skip(32).Take(16).ToArray();
                decryptedAesKey = decryptedAesKey.Take(32).ToArray();
            }

            // decrypt encrypted ZIP file using decrypted AES key
            string decryptedFileName = encryptedPayloadFile.Replace("_Payload", "_Payload_decrypted.zip");
            AesManager.DecryptFile(encryptedPayloadFile, decryptedFileName, decryptedAesKey, aesVector, isECB);

            //Deflate the decrypted zip archive
            ZipManager.ExtractArchive(decryptedFileName, xmlProcessingFolder, true);
            decryptedPayload = decryptedFileName.Replace("_Payload_decrypted.zip", "_Payload.xml");

            //If the metadata is something other than XML, read the wrapper and rebuild the non-XML file
            if (metadataContentType != "XML")
            {
                //Some non-XML files may not have _Payload in the file name, if not remove it   
                if (!File.Exists(decryptedPayload))
                {
                    decryptedPayload = decryptedPayload.Replace("_Payload.xml", ".xml");
                }

                //This will give us the base64 encoded data from the XML file   
                string encodedData = XmlManager.ExtractXMLImageData(decryptedPayload);

                //We will convert the base64 data back to bytes
                byte[] binaryData;
                string decodedPayload = decryptedPayload.Replace(".xml", "." + metadataContentType);
                binaryData = System.Convert.FromBase64String(encodedData);

                //We can write the bytes back to rebuild the file
                FileStream decodedFile;
                decodedFile = new FileStream(decodedPayload, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                decodedFile.Write(binaryData, 0, binaryData.Length);
                decodedFile.Close();

            }

            return decryptedPayload;
           
        }
    }
}
