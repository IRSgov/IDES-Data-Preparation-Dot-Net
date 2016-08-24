using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace WindowsFormsApplication1
{

    public delegate string schemaValidationEventHandler(object sender, ValidationEventArgs e);
    /// <summary>
    /// The type of signature to perform
    /// </summary>
    public enum XmlSignatureType
    {

        Enveloping,
        NonXML
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
        /// <param name="encodingPath">Used for non-xml, the root path of the document being signed, used for temporary encoding file</param>
        /// <param name="signatureType">This will be XML or Non-XML</param>
        /// <param name="xml">The XML data to sign represented as byte array</param>
        /// <param name="certFile">The certificate file to use for signing</param>
        /// <param name="certPassword">The certificate password</param>
        /// <param name="signWithSha256">Sign the document using SHA-256</param>
        /// <returns>The signed data represented as byte array</returns>
        private static byte[] SignEnvelopingXml(XmlSignatureType signatureType, byte[] xml, string certFile, string certPassword, bool signWithSha256, string encodingPath = "")
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

            //If this is NOT XML, then we have to convert it and stick it in XML first
            if (signatureType == XmlSignatureType.NonXML)
            {
                //base64 encode it
                string strEncodedImage;
                strEncodedImage =   System.Convert.ToBase64String(xml, 0,xml.Length);
   
                //create a small xml file and put the encoded Image data inside 
                // Create an XmlWriterSettings object with the correct options. 
                XmlWriter writer = null;
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = false;
                settings.NewLineHandling = NewLineHandling.Replace;
                settings.CloseOutput = true;

                string metadataFileName = encodingPath + "\\TempEncoded.xml";

                // Create the XmlWriter object and write some content.
                writer = XmlWriter.Create(metadataFileName, settings);
                writer.WriteStartElement("Wrapper", "");
                writer.WriteString(strEncodedImage);
                writer.WriteEndElement();
               
                //Close the XmlTextWriter.
                writer.WriteEndDocument();
                writer.Close();
                writer.Flush();
                xml = File.ReadAllBytes(encodingPath + "\\TempEncoded.xml");
            }
            

            using (MemoryStream stream = new MemoryStream(xml))
            {
                // go to the beginning of the stream
                stream.Flush();
                stream.Position = 0;

                // create new XmlDocument from stream
                XmlDocument doc = new XmlDocument() { PreserveWhitespace = true };
                doc.Load(stream);

                // create transform (for canonicalization method & reference)
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
        public static byte[] Sign(XmlSignatureType signatureType, byte[] xml, string certFile, string certPassword, string encodingPath, bool signWithSha256 = true)
        {
            switch (signatureType)
            {

                case XmlSignatureType.Enveloping:
                    return SignEnvelopingXml(signatureType, xml, certFile, certPassword, signWithSha256);
                case XmlSignatureType.NonXML:
                    return SignEnvelopingXml(signatureType, xml, certFile, certPassword, signWithSha256, encodingPath);
                default:
                    throw new Exception("Please use a valid XML signature type!");
            }
        }

        /// <summary>
        /// Verifies if the signature of the file is valid
        /// </summary>
        /// <param name="signedXml">The path to the signed file</param>
        /// <returns>True if valid or false if not valid</returns>
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
                XmlNodeList nodeList = doc.GetElementsByTagName("Signature", "*");
                if (nodeList.Count != 1)
                {
                    // invalid file
                    throw new Exception("Signature is missing or multiple signatures found!");
                }

                // get signature node
                XmlNodeList keyInfoNode = doc.GetElementsByTagName("KeyInfo", "*");
                if (keyInfoNode.Count != 1)
                {
                    // invalid file
                    throw new Exception("KeyInfo element is required to validate signature!");
                }

                // create SignedXml and load it with data
                SignedXml signed = new SignedXml(doc);
                signed.LoadXml(nodeList[0] as XmlElement);
                XmlElement root = doc.DocumentElement;

                // check the reference in the signature 
                CheckSignatureReference(signed, root);
                // check the signature
                return signed.CheckSignature();
            }
        }

        /// <summary>
        /// Verifies that there is only one reference in the signature and that the reference in the signature matches the reference in the XML
        /// </summary>
        /// <param name="signedXml">The path to the signed file</param>
        /// <param name="xmlElement">signature and object data of the signed file</param>
        private static void CheckSignatureReference(SignedXml signedXml, XmlElement xmlElement) 
         { 
            //if no reference at all is found, there is a problem 
            if(signedXml.SignedInfo.References.Count == 0) 
             { 
                 throw new Exception("No reference was found in XML signature"); 
             } 
 
             //if there is more than one reference, there is a problem   
             if(signedXml.SignedInfo.References.Count != 1) 
             { 
                 throw new Exception("Multiple references for XML signatures are not allowed"); 
             } 
 
            var reference = (Reference)signedXml.SignedInfo.References[0]; 
            var id = reference.Uri.Substring(1);             
            var idElement = signedXml.GetIdElement(xmlElement.OwnerDocument, id);
            string signedReference = "";
            
            //the reference in the XML will be compared to the reference in the signature, if no match, there is a problem
            if (idElement == null)
             {
                 XmlNodeList objectNode = xmlElement.GetElementsByTagName("Object", "*");
                 //If we dont fine the id above, we will pull the Object element from the file
                 if (objectNode.Count == 1)
                 {
                    //Create a new attribute.
                    XmlNode testroot = objectNode[0];
                    signedReference = testroot.Attributes[0].Value.ToString();
                 }
                 
                 //Check the reference from the XML and see if it matches the signature, if it still doesn't there is a problem
                 if (id != signedReference)
                 { 
                     throw new Exception("The signed reference does not match the XML reference");
                 }
             }
 
         } 




        /// <summary>
        /// This will extract the base64 image data from the notification
        /// </summary>
        /// <param name="signedXml">The path to the xml file with the encoded data</param>
        /// <returns>the encoded image data</returns>
        public static string ExtractXMLImageData(string signedXml)
        {

            string hold = "test";
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
                XmlNodeList nodeList = doc.GetElementsByTagName("Wrapper", "*");

                if (nodeList.Count != 1)
                {
                    // invalid file
                    throw new Exception("No wrapper element for the image data was found!");
                }

                //This will be the base64 encoded Payload that will need to be converted back to an image file
                hold = nodeList[0].InnerText;

                // check
                return hold;
            }
        }

        /// <summary>
        /// This will check the metadata file to see if there is a FileFormatCd and what it is
        /// </summary>
        /// <param name="metadataFile">The path to the metadata file</param>
        /// <returns>the code from the metadata file</returns>
        public static string CheckMetadataType(string metadataFile)
        {
            //Our default value will be XML, and we only need to do something different if it isn't
            string codeFromMetadata = "XML";
            byte[] xml = File.ReadAllBytes(metadataFile);

            using (MemoryStream stream = new MemoryStream(xml))
            {
                // go to the beginning of the stream
                stream.Flush();
                stream.Position = 0;

                // create new XmlDocument from stream
                XmlDocument doc = new XmlDocument() { PreserveWhitespace = true };
                doc.Load(stream);

                // get File Format Code node
                XmlNodeList nodeList = doc.GetElementsByTagName("FileFormatCd", "*");
                
                //If the node is found, we will pull the value so we know if this is something other than XML
                if (nodeList.Count == 1)
                {
                    codeFromMetadata = nodeList[0].InnerText;
                }

                // check
                return codeFromMetadata;
            }
        }

        /// <summary>
        /// This will look at a decrypted notification and pull the transmissionID, senderFileId, and the notification error code
        /// </summary>
        /// <param name="notificationFile">The path to the notification file</param>
        /// <returns>notificationValues array with 3 values</returns>
        public static string[] CheckNotification(string notificationFile)
        {
            //Our default value will be XML, and we only need to do something different if it isn't
            string[] notificationValues = new string[3];
            byte[] xml = File.ReadAllBytes(notificationFile);

            using (MemoryStream stream = new MemoryStream(xml))
            {
                // go to the beginning of the stream
                stream.Flush();
                stream.Position = 0;

                // create new XmlDocument from stream
                XmlDocument doc = new XmlDocument() { PreserveWhitespace = true };
                doc.Load(stream);

                // get IDES transmission ID
                XmlNodeList nodeList = doc.GetElementsByTagName("IDESTransmissionId");
                //If the node is found, we will pull the value so we know if this is something other than XML
                if (nodeList.Count == 1)
                {
                    notificationValues[0] = nodeList[0].InnerText;
                }

                // get File Format Code node
                nodeList = doc.GetElementsByTagName("SenderFileId");
                //If the node is found, we will pull the value so we know if this is something other than XML
                if (nodeList.Count == 1)
                {
                    notificationValues[1] = nodeList[0].InnerText;
                }

                // get File Format Code node
                nodeList = doc.GetElementsByTagName("FATCANotificationCd");
                //If the node is found, we will pull the value so we know if this is something other than XML
                if (nodeList.Count == 1)
                {
                    notificationValues[2] = nodeList[0].InnerText;
                }

                // check
                return notificationValues;
            }
        }

        /// <summary>
        /// This will check the input XML file against the provided schema folder. It will dynamically look for the individual 
        /// schema files, so it will work for the current schema version and the upcoming version 2.
        /// </summary>
        /// <param name="inputFile">The path to the xml file being validated</param>
        /// <param name="schemaFolder">The path to the folder containing the .xsd files that the XML will be validated against</param>
        /// <returns>a string that will contain the results of the validation</returns>
        public static string CheckSchema(string inputFile, string schemaFolder = "")
        {

            string folderCheck = schemaFolder.Substring(schemaFolder.Length - 1, 1);
            if (folderCheck != "\\")
            {
                schemaFolder = schemaFolder + "\\";
            }

            // select the schema files from the input folder
            string fatcaFile = "";
            string stfFile = "";
            string isoFile = "";
            string oecdFile = "";
            string[] fatcaFiles = Directory.GetFiles(schemaFolder, "*fatcaXML*.xsd", SearchOption.TopDirectoryOnly);
            string[] stfFiles = Directory.GetFiles(schemaFolder, "*stffatcatypes*.xsd", SearchOption.TopDirectoryOnly);
            string[] isoFiles = Directory.GetFiles(schemaFolder, "*isofatcatypes*.xsd", SearchOption.TopDirectoryOnly);
            string[] oecdFiles = Directory.GetFiles(schemaFolder, "*oecdtypes*.xsd", SearchOption.TopDirectoryOnly);

            if (fatcaFiles.Length == 0)
            {
                // fatca xsd validation
                throw new Exception("There was no file found containing the fatca xsd file!");
            }
            if (stfFiles.Length == 0)
            {
                // stf xsd validation
                throw new Exception("There was no file found containing the stffatcatypes xsd file!");
            }
            if (isoFiles.Length == 0)
            {
                // iso xsd validation
                throw new Exception("There was no file found containing the isofatcatypes xsd file!");
            }
            if (oecdFiles.Length == 0)
            {
                // oecd file validation
                throw new Exception("There was no file found containing the oecdtypes xsd file!");
            }

            fatcaFile = fatcaFiles[0];
            stfFile = stfFiles[0];
            isoFile = isoFiles[0];
            oecdFile = oecdFiles[0];
            XmlSchemaSet schemas = new XmlSchemaSet();
            if (fatcaFile.Contains("v2"))
            {

                schemas.Add("urn:oecd:ties:fatca:v2", fatcaFile);
                schemas.Add("urn:oecd:ties:stffatcatypes:v2", stfFile);
                schemas.Add("urn:oecd:ties:isofatcatypes:v1", isoFile);
                schemas.Add("urn:oecd:ties:stf:v4", oecdFile);
            
            }
            else
            {
                schemas.Add("urn:oecd:ties:fatca:v1", fatcaFile);
                schemas.Add("urn:oecd:ties:stffatcatypes:v1", stfFile);
                schemas.Add("urn:oecd:ties:isofatcatypes:v1", isoFile);
                schemas.Add("urn:oecd:ties:stf:v4", oecdFile);
            }
            
            
            XDocument doc = XDocument.Load(inputFile);
            string msg = "";
            doc.Validate(schemas, (o, e) =>
            {
                msg += e.Message + Environment.NewLine;
            });
            Console.WriteLine(msg == "" ? "Document is valid" : "Document invalid: " + msg);

            return msg;
        }

        /// <summary>
        /// This will create a metadata file that will be included in the data packet
        /// </summary>
        /// <param name="metadataFileName">The path to where the metadata file will be created</param>
        /// <param name="fileExtension">The extension of the file being encrypted (XML, PDF, TXT, etc...)</param>
        /// <param name="isXML">A boolean indicating whether this is an XML transfer or non-XML</param>
        /// <param name="taxYear">The tax year from the drop down box</param>
        /// <param name="senderGIIN">The GIIN of the sender as indicated on the payload</param>
        /// <param name="senderFile">The file name of the final data packet in correct UTC format</param>
        /// <returns></returns>
        public static void CreateMetadataFile(string metadataFileName, string fileExtension, bool isXML, string taxYear, string senderGIIN, string senderFile) 
        {
            //Start creating XML metadata
            XmlWriter writer = null;
            string fileCreationDateTime = "";
            fileCreationDateTime = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ");

            // Create an XmlWriterSettings object with the correct options. 
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.OmitXmlDeclaration = false;
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.CloseOutput = true;

            // Create the XmlWriter object and write some content.
            writer = XmlWriter.Create(metadataFileName, settings);
            writer.WriteStartElement("FATCAIDESSenderFileMetadata", "urn:fatca:idessenderfilemetadata");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteStartElement("FATCAEntitySenderId");
            writer.WriteString(senderGIIN);
            writer.WriteEndElement();
            writer.WriteStartElement("FATCAEntityReceiverId");
            writer.WriteString("000000.00000.TA.840");
            writer.WriteEndElement();
            writer.WriteStartElement("FATCAEntCommunicationTypeCd");
            writer.WriteString("RPT");
            writer.WriteEndElement();
            writer.WriteStartElement("SenderFileId");
            writer.WriteString(senderFile);
            writer.WriteEndElement();
            writer.WriteStartElement("FileFormatCd");
            writer.WriteString(fileExtension);
            writer.WriteEndElement();
            writer.WriteStartElement("BinaryEncodingSchemeCd");
            if (isXML == true)
            {
                writer.WriteString("NONE");
            }
            else
            {
                writer.WriteString("BASE64");
            }
            writer.WriteEndElement();
            writer.WriteStartElement("FileCreateTs");
            writer.WriteString(fileCreationDateTime);
            writer.WriteEndElement();
            writer.WriteStartElement("TaxYear");
            writer.WriteString(taxYear);
            writer.WriteEndElement();
            writer.WriteStartElement("FileRevisionInd");
            writer.WriteString("false");
            writer.WriteEndElement();
            
            //Close the XmlTextWriter.
            writer.WriteEndDocument();
            writer.Close();
            writer.Flush();
        }
    }
}