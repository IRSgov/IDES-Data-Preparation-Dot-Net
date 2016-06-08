using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using System.Linq;


namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        

        private void btnBrowseXml_Click(object sender, EventArgs e)
        {
            // load XML
            txtXmlFile.Text = dlgOpen.ShowDialogWithFilter("XML Files (*.xml)|*.xml");
        }

        private void btnBrowseCert_Click(object sender, EventArgs e)
        {
            // load certificate
            txtCert.Text = dlgOpen.ShowDialogWithFilter("Signing Certificates (*.pfx, *.p12)|*.pfx;*.p12;");
        }

        private void btnBrowseKeyCert_Click(object sender, EventArgs e)
        {
            // load AES key encryption certificate
            txtKeyCert.Text = dlgOpen.ShowDialogWithFilter("Certificate Files (*.cer, *.pfx, *.p12)|*.cer;*.pfx;*.p12;");
        }

        private void btnSignXML_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtXmlFile.Text))
            {
                // files validation
                MessageBox.Show("The XML file was not specified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCert.Text))
            {
                // files validation
                MessageBox.Show("The Signing Certificate was not specified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCertPass.Text))
            {
                // certificate password validation
                MessageBox.Show("Signing Certificate password was not specified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtKeyCert.Text))
            {
                // files validation
                MessageBox.Show("Encryption Certificate was not specified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // load XML file content
                byte[] xmlContent = File.ReadAllBytes(txtXmlFile.Text);
                string senderGIIN = Path.GetFileNameWithoutExtension(txtXmlFile.Text);
                string filePath = Path.GetDirectoryName(txtXmlFile.Text);

                // perform sign
                byte[] envelopingSignature = XmlManager.Sign(XmlSignatureType.Enveloping, xmlContent, txtCert.Text, txtCertPass.Text);

                string envelopingFileName = txtXmlFile.Text.Replace(".xml", "_Payload.xml");
                string zipFileName = envelopingFileName.Replace(".xml", ".zip");

                // save enveloping version to disk
                File.WriteAllBytes(envelopingFileName, envelopingSignature);

                // add enveloping signature to ZIP file
                ZipManager.CreateArchive(envelopingFileName, zipFileName);

                // generate AES key (32 bytes) & default initialization vector (empty)
                byte[] aesEncryptionKey = AesManager.GenerateRandomKey(AesManager.KeySize / 8);
                byte[] aesEncryptionVector = AesManager.GenerateRandomKey(16, radECB.Checked);

                // encrypt file & save to disk
                string encryptedFileName = zipFileName.Replace(".zip", "");
                string encryptedHCTAFileName = zipFileName.Replace(".zip", "");
                string payloadFileName = encryptedFileName;
                AesManager.EncryptFile(zipFileName, encryptedFileName, aesEncryptionKey, aesEncryptionVector, radECB.Checked);


                // encrypt key with public key of certificate & save to disk
                encryptedFileName = Path.GetDirectoryName(zipFileName) + "\\000000.00000.TA.840_Key";
                AesManager.EncryptAesKey(aesEncryptionKey, aesEncryptionVector, txtKeyCert.Text, txtKeyCertPassword.Text, encryptedFileName, radECB.Checked);
                //For Model1 Option2 Only, encrypt the AES Key with the HCTA Public Key
                if (chkM1O2.Checked) {
                    encryptedHCTAFileName = Path.GetDirectoryName(zipFileName) + "\\000000.00000.TA." + txtHCTACode.Text + "_Key";
                    AesManager.EncryptAesKey(aesEncryptionKey, aesEncryptionVector, txtHCTACert.Text, txtHCTACertPassword.Text, encryptedHCTAFileName, radECB.Checked);
                }

                // cleanup
                envelopingSignature = null;
                aesEncryptionKey = aesEncryptionVector = null;

                //Start creating XML metadata
                XmlWriter writer = null;
                string fileCreationDateTime = "";
                fileCreationDateTime = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ");

                DateTime uDat = new DateTime();
                uDat = DateTime.UtcNow;
                string senderFile = uDat.ToString("yyyyMMddTHHmmssfffZ") + "_" + senderGIIN;

                try
                {

                    // Create an XmlWriterSettings object with the correct options. 
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = ("\t");
                    settings.OmitXmlDeclaration = false;
                    settings.NewLineHandling = NewLineHandling.Replace;
                    settings.CloseOutput = true;

                    string metadataFileName = filePath + "\\" + senderGIIN + "_Metadata.xml";

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
                    writer.WriteString("XML");
                    writer.WriteEndElement();
                    writer.WriteStartElement("BinaryEncodingSchemeCd");
                    writer.WriteString("NONE");
                    writer.WriteEndElement();
                    writer.WriteStartElement("FileCreateTs");
                    writer.WriteString(fileCreationDateTime);
                    writer.WriteEndElement();
                    writer.WriteStartElement("TaxYear");
                    writer.WriteString(cmbTaxYear.SelectedItem.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("FileRevisionInd");
                    writer.WriteString("false");
                    writer.WriteEndElement();
                    //Close the XmlTextWriter.
                    writer.WriteEndDocument();
                    writer.Close();
                    writer.Flush();


                    //Add the metadata, payload, and key files to the final zip package
                    // add enveloping signature to ZIP file
                    ZipManager.CreateArchive(metadataFileName, filePath + "\\" + senderFile + ".zip");
                    ZipManager.UpdateArchive(encryptedFileName, filePath + "\\" + senderFile + ".zip");
                    ZipManager.UpdateArchive(payloadFileName, filePath + "\\" + senderFile + ".zip");
                    //Add the HCTA Key file for a M1O2 packet
                    if (chkM1O2.Checked)
                    {
                        ZipManager.UpdateArchive(encryptedHCTAFileName, filePath + "\\" + senderFile + ".zip");
                    }

                    // success
                    MessageBox.Show("XML Signing and Encryption process is complete!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                finally
                {
                    if (writer != null)
                        writer.Close();
                }



            }
            catch (Exception ex)
            {
                ex.DisplayException(Text);
            }
        }

        private void btnBrowseNotificationZip_Click(object sender, EventArgs e)
        {
            // load Notification Zip file
            txtNotificationZip.Text = dlgOpen.ShowDialogWithFilter("ZIP Files (*.zip)|*.zip");
        }

        private void btnBrowseRecCert_Click(object sender, EventArgs e)
        {
            // load Notification Receiver key
            txtReceiverCert.Text = dlgOpen.ShowDialogWithFilter("Certificate Files (*.cer, *.pfx, *.p12)|*.cer;*.pfx;*.p12");
        }

        private void btnDecryptZip_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNotificationZip.Text) || string.IsNullOrWhiteSpace(txtReceiverCert.Text))
            {
                // files validation
                MessageBox.Show("Either the ZIP file or certificate was not specified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string zipFolder = "";
            try
            {
                //Deflate the zip archive
                zipFolder = ZipManager.ExtractArchive(txtNotificationZip.Text, txtNotificationFolder.Text);

            }
            catch (Exception ex)
            {
                ex.DisplayException(Text);
                return;
            }
            // select encrypted key file
            string encryptedKeyFile = "";
            string encryptedPayloadFile = "";
            string metadataFile = "";
            string[] keyFiles = Directory.GetFiles(zipFolder, "*_Key", SearchOption.TopDirectoryOnly);
            string[] payloadFiles = Directory.GetFiles(zipFolder, "*_Payload", SearchOption.TopDirectoryOnly);
            string[] metadataFiles = Directory.GetFiles(zipFolder, "*_Metadata*", SearchOption.TopDirectoryOnly);
            
            if (keyFiles.Length == 0)
            {
                // key file validation
                MessageBox.Show("There was no file found containing the encrypted AES key!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (payloadFiles.Length == 0)
            {
                // key file validation
                MessageBox.Show("There was no file found containing the encrypted Payload!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            encryptedKeyFile = keyFiles[0];
            encryptedPayloadFile = payloadFiles[0];
            metadataFile = metadataFiles[0];

            //Check the metadata and see what we have
            string metadataContentType = XmlManager.CheckMetadataType(metadataFile);


            byte[] encryptedAesKey = null;
            byte[] decryptedAesKey = null;
            byte[] aesVector = null;

            try
            {
                // load encrypted AES key
                encryptedAesKey = File.ReadAllBytes(encryptedKeyFile);

                // decrypt AES key & generate default (empty) initialization vector
                decryptedAesKey = AesManager.DecryptAesKey(encryptedAesKey, txtReceiverCert.Text, txtRecKeyPassword.Text);
                aesVector = AesManager.GenerateRandomKey(16, true);
                if (radECB.Checked != true)
                {
                    aesVector = decryptedAesKey.Skip(32).Take(16).ToArray();
                    decryptedAesKey = decryptedAesKey.Take(32).ToArray();
                }

                // decrypt encrypted ZIP file using decrypted AES key
                string decryptedFileName = encryptedPayloadFile.Replace("_Payload", "_Payload_decrypted.zip");
                AesManager.DecryptFile(encryptedPayloadFile, decryptedFileName, decryptedAesKey, aesVector, radECB.Checked);


                //Deflate the decrypted zip archive
                ZipManager.ExtractArchive(decryptedFileName, decryptedFileName, false);
                string decryptedPayload = decryptedFileName.Replace("_Payload_decrypted.zip", "_Payload.xml");
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

                // success
                MessageBox.Show("Notification decryption process is complete!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ex.DisplayException(Text);
            }
            finally
            {
                if (encryptedAesKey != null)
                {
                    encryptedAesKey = null;
                }

                if (decryptedAesKey != null)
                {
                    decryptedAesKey = null;
                }

                if (aesVector != null)
                {
                    aesVector = null;
                }
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            // load AES key encryption certificate
            if (dlgOpenFolder.ShowDialog() == DialogResult.OK)
            {
                txtNotificationFolder.Text = dlgOpenFolder.SelectedPath;
            }
        }


        private void chkM1O2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkM1O2.Checked)
            {
                this.lblHCTAKey.Location = new Point(
                this.lblHCTAKey.Location.X,
                this.lblHCTAKey.Location.Y - 40
                );
                this.btnBrowseHCTACert.Location = new Point(
                this.btnBrowseHCTACert.Location.X,
                this.btnBrowseHCTACert.Location.Y - 40
                );
                this.txtHCTACert.Location = new Point(
                this.txtHCTACert.Location.X,
                this.txtHCTACert.Location.Y - 40
                );
                this.lblEncryptionHCTAPassword.Location = new Point(
                this.lblEncryptionHCTAPassword.Location.X,
                this.lblEncryptionHCTAPassword.Location.Y - 40
                );
                this.txtHCTACertPassword.Location = new Point(
                this.txtHCTACertPassword.Location.X,
                this.txtHCTACertPassword.Location.Y - 40
                );
                this.lblHCTACode.Location = new Point(
                this.lblHCTACode.Location.X,
                this.lblHCTACode.Location.Y - 40
                );
                this.txtHCTACode.Location = new Point(
                this.txtHCTACode.Location.X,
                this.txtHCTACode.Location.Y - 40
                );
                this.btnSignXML.Location = new Point(
                this.btnSignXML.Location.X,
                this.btnSignXML.Location.Y + 140
                );
                

                lblHCTAKey.Visible = true;
                txtHCTACert.Visible = true;
                btnBrowseHCTACert.Visible = true;
                lblEncryptionHCTAPassword.Visible = true;
                txtHCTACertPassword.Visible = true;
                lblHCTACode.Visible = true;
                txtHCTACode.Visible = true;
            }
            else
            {
                this.lblHCTAKey.Location = new Point(
                this.lblHCTAKey.Location.X,
                this.lblHCTAKey.Location.Y + 40
                );
                this.btnBrowseHCTACert.Location = new Point(
                this.btnBrowseHCTACert.Location.X,
                this.btnBrowseHCTACert.Location.Y + 40
                );
                this.txtHCTACert.Location = new Point(
                this.txtHCTACert.Location.X,
                this.txtHCTACert.Location.Y + 40
                );
                this.lblEncryptionHCTAPassword.Location = new Point(
                this.lblEncryptionHCTAPassword.Location.X,
                this.lblEncryptionHCTAPassword.Location.Y + 40
                );
                this.txtHCTACertPassword.Location = new Point(
                this.txtHCTACertPassword.Location.X,
                this.txtHCTACertPassword.Location.Y + 40
                );
                this.lblHCTACode.Location = new Point(
                this.lblHCTACode.Location.X,
                this.lblHCTACode.Location.Y + 40
                );
                this.txtHCTACode.Location = new Point(
                this.txtHCTACode.Location.X,
                this.txtHCTACode.Location.Y + 40
                );
                this.btnSignXML.Location = new Point(
                this.btnSignXML.Location.X,
                this.btnSignXML.Location.Y - 140
                );

                lblHCTAKey.Visible = false;
                txtHCTACert.Visible = false;
                btnBrowseHCTACert.Visible = false;
                lblEncryptionHCTAPassword.Visible = false;
                txtHCTACertPassword.Visible = false;
                lblHCTACode.Visible = false;
                txtHCTACode.Visible = false;
            }
        }


        private void btnBrowseHCTACert_Click(object sender, EventArgs e)
        {
            // load AES key encryption certificate
            txtHCTACert.Text = dlgOpen.ShowDialogWithFilter("Certificate Files (*.cer, *.pfx, *.p12)|*.cer;*.pfx;*.p12");
      
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblHCTAKey.Visible = false;
            txtHCTACert.Visible = false;
            btnBrowseHCTACert.Visible = false;
            lblEncryptionHCTAPassword.Visible = false;
            txtHCTACertPassword.Visible = false;
            lblHCTACode.Visible = false;
            txtHCTACode.Visible = false;

            //Populate tax year combo box
            cmbTaxYear.Items.Add(2014);
            cmbTaxYear.Items.Add(2015);
            cmbTaxYear.Items.Add(2016);
            cmbTaxYear.SelectedItem = DateTime.Now.Year - 1;

            //Default the CBC checkbox to on
            radCBC.Checked = true;
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       

       


       
    }
}
