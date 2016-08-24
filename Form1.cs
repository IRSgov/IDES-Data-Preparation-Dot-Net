using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using System.Linq;
using WinSCP;

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
            if (chkSendFolder.Checked == true)
            {
                // Select folder location that holds all 8966 XML files to process
                if (dlgOpenFolder.ShowDialog() == DialogResult.OK)
                {
                    txtXmlFile.Text = dlgOpenFolder.SelectedPath;
                }

            }
            else
            {

                // load XML
                txtXmlFile.Text = dlgOpen.ShowDialogWithFilter("XML Files (*.xml, *.pdf)|*.xml;*.pdf");

            }
        }

        private void btnBrowseCert_Click(object sender, EventArgs e)
        {
            // load certificate
            txtCert.Text = dlgOpen.ShowDialogWithFilter("Signing Certificates (*.pfx, *.p12)|*.pfx;*.p12");
        }

        private void btnBrowseKeyCert_Click(object sender, EventArgs e)
        {
            // load AES key encryption certificate
            txtKeyCert.Text = dlgOpen.ShowDialogWithFilter("Certificate Files (*.cer, *.pfx, *.p12)|*.cer;*.pfx;*.p12");
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

            if (chkSchemaValidation.Checked == true && string.IsNullOrWhiteSpace(txtSchemaFolder.Text))
            {
                // files validation
                MessageBox.Show("Schema Validation selected but Schema Folder was not specified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //this will be used as a holder for processed files if a folder is being sent
            string processedFolder = "";

            try
            {

                //if we are sending a folder, we will load up the files into an array to process
                //otherwise, we will just load our file in the array
                string[] filesToProcess;
                if (chkSendFolder.Checked == false)
                {
                    filesToProcess = new string[1];
                    filesToProcess[0] = txtXmlFile.Text;
                }
                else
                {
                    // Load all XML files in the folder into an array
                    filesToProcess = Directory.GetFiles(txtXmlFile.Text, "*.xml", SearchOption.TopDirectoryOnly);
                    processedFolder = txtXmlFile.Text + "\\Processed";

                    //if the processedFolder doesn't exist, create it
                    if (!Directory.Exists(processedFolder))
                    {
                        Directory.CreateDirectory(processedFolder);
                    }
                }

                //This will loop through all files in the array
                //This will be one file or it could be a set of files in a folder
                string currentFileToProcess = "";
                foreach (string fileName in filesToProcess)
                {
                    currentFileToProcess = fileName;
                    //if the file we are processing has an underscore we will split it off for logging
                    //this should only happen for bulk sending from a folder
                    if (currentFileToProcess.Contains("_"))
                    {
                        string[] filePart = fileName.Split('_');
                        currentFileToProcess = txtXmlFile.Text + "\\" + filePart[1];
                        //Rename the file so we can process it
                        File.Move(fileName, currentFileToProcess);
                    }

                    // perform the schema validation if we have the checkbox checked for it
                    if (chkSchemaValidation.Checked)
                    {
                        string validationError = XmlManager.CheckSchema(currentFileToProcess, txtSchemaFolder.Text);
                        if (validationError != "")
                        {
                            // Show schema validation error(s)
                            MessageBox.Show("Schema Validation Error:\r\n" + validationError, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // load XML file content
                    byte[] xmlContent = File.ReadAllBytes(currentFileToProcess);
                    string senderGIIN = Path.GetFileNameWithoutExtension(currentFileToProcess);
                    string filePath = Path.GetDirectoryName(currentFileToProcess);
                    string fileExtension = Path.GetExtension(currentFileToProcess.ToUpper()).Replace(".", "");
                    bool isXML = true;

                    if (fileExtension != "XML")
                    {
                        isXML = false;
                    }

                    // perform signature
                    byte[] envelopingSignature;
                    string envelopingFileName = "";

                    //if the file is XML we will use the enveloping digital signature for the XML
                    if (isXML == true)
                    {
                        envelopingSignature = XmlManager.Sign(XmlSignatureType.Enveloping, xmlContent, txtCert.Text, txtCertPass.Text, filePath);
                        envelopingFileName = currentFileToProcess.Replace(".xml", "_Payload.xml");
                    }
                    //if the file is NOT XML, we will convert the file data to base64 and put in XML and sign it
                    else
                    {
                        envelopingSignature = XmlManager.Sign(XmlSignatureType.NonXML, xmlContent, txtCert.Text, txtCertPass.Text, filePath);
                        envelopingFileName = currentFileToProcess.Replace(".pdf", "_Payload.xml");
                    }

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
                    string payloadFileName = encryptedFileName + "";
                    AesManager.EncryptFile(zipFileName, encryptedFileName, aesEncryptionKey, aesEncryptionVector, radECB.Checked);

                    // encrypt key with public key of certificate & save to disk
                    encryptedFileName = Path.GetDirectoryName(zipFileName) + "\\000000.00000.TA.840_Key"; ;
                    AesManager.EncryptAesKey(aesEncryptionKey, aesEncryptionVector, txtKeyCert.Text, txtKeyCertPassword.Text, encryptedFileName, radECB.Checked);
                    //For Model1 Option2 Only, encrypt the AES Key with the HCTA Public Key
                    if (chkM1O2.Checked)
                    {
                        encryptedHCTAFileName = Path.GetDirectoryName(zipFileName) + "\\000000.00000.TA." + txtHCTACode.Text + "_Key";
                        AesManager.EncryptAesKey(aesEncryptionKey, aesEncryptionVector, txtHCTACert.Text, txtHCTACertPassword.Text, encryptedHCTAFileName, radECB.Checked);
                    }

                    // cleanup
                    envelopingSignature = null;
                    aesEncryptionKey = aesEncryptionVector = null;

                    

                    try
                    {
                        DateTime uDat = new DateTime();
                        uDat = DateTime.UtcNow;
                        string senderFile = uDat.ToString("yyyyMMddTHHmmssfffZ") + "_" + senderGIIN;
                        string metadataFileName = filePath + "\\" + senderGIIN + "_Metadata.xml";
                        XmlManager.CreateMetadataFile(metadataFileName, fileExtension, isXML, cmbTaxYear.SelectedItem.ToString(), senderGIIN,  senderFile);

                        //Check the signature to make sure it is valid, this requires the KeyInfo to be present
                        //This is controlled using the checkbox on the form
                        //This should be commented out or not selected if not using the KeyInfo in the XmlManager class
                        if (chkSignatureValidation.Checked)
                        {
                            bool result = XmlManager.CheckSignature(envelopingFileName);
                            if (result == false)
                            {
                                MessageBox.Show("Signature is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

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


                        
                        if (chkAutoSendSFTP.Checked == true)
                        {
                            SessionOptions currentSFTPSession = SFTPManager.CreateSFTPSession(cmbSFTPServers.SelectedItem.ToString(), username.Text, password.Text);
                            string sftpUpName = filePath + "\\" + senderFile + ".zip";
                            string transferResult = SFTPManager.UploadFile(currentSFTPSession, sftpUpName);
                            //This can be commented out if there is no desire to see an upload confirmation
                            MessageBox.Show(transferResult, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            //write to log
                            string path = @"simplelog.csv";
                            // This text is added only once to the file.
                            string lineToLog = fileName + "," + currentFileToProcess + "," + senderFile + ",IDESTRANSID,NOTIFICATIONID,NULL";
                            if (!File.Exists(path))
                            {
                                // Create a file to write to.
                                using (StreamWriter sw = File.CreateText(path))
                                {
                                    sw.WriteLine(lineToLog);
                                }
                            }
                            else
                            { 
                                // This text is always added, making the file longer over time
                                // if it is not deleted.
                                using (StreamWriter sw = File.AppendText(path))
                                {
                                    sw.WriteLine(lineToLog);
                                }
                            }
                        }
                        if (chkSendFolder.Checked == true)
                        { 
                        //Move the file to a processed folder so we can move on to the next
                        //This is only used when sending an entire folder
                        File.Move(currentFileToProcess, processedFolder + "\\" + Path.GetFileName(fileName));
                        }


                    }
                    catch (Exception ex)
                    {
                        ex.DisplayException(Text);
                        return;
                    }
                    finally
                    {
                       
                    }

                }
                // success
                MessageBox.Show("XML Signing and Encryption process is complete!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

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

            //Decrypt the Payload
            string decryptedPayload = "";
            try
            {
                decryptedPayload = AesManager.DecryptNotification(zipFolder, txtReceiverCert.Text, txtRecKeyPassword.Text, radECB.Checked);
            }
            catch (Exception ex)
            {
                ex.DisplayException("Decryption Failed:" + Text);
                return;
            }

            // success
            MessageBox.Show("Decryption process is complete!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);



        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            // load AES key encryption certificate
            if (dlgOpenFolder.ShowDialog() == DialogResult.OK)
            {
                txtNotificationFolder.Text = dlgOpenFolder.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            
            //Populate tax year combo box, this will contain the current year back to the first reporting year of 2014
            for (int i = DateTime.Now.Year; i >= 2014; i--)
            {
                cmbTaxYear.Items.Add(i);
            }
            cmbTaxYear.SelectedItem = DateTime.Now.Year - 1;

            //Default the CBC checkbox to on
            radCBC.Checked = true;


            //Add the SFTP Servers
            cmbSFTPServers.Items.Add("PRODUCTION: WWW.IDESGATEWAY.COM");
            cmbSFTPServers.Items.Add("TEST: WWWPSE.IDESGATEWAY.COM");
            cmbSFTPServers.SelectedItem = "PRODUCTION: WWW.IDESGATEWAY.COM";
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

      

        private void cmdCheckSignature_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSignedPayloadFile.Text))
            {
                // files validation
                MessageBox.Show("The Signed Payload File was not specified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // register SHA-256 and open certificate with exportable private key
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), RSAPKCS1SHA256SignatureDescription.SignatureMethod);

            //Check the signature to make sure it is valid, this requires the KeyInfo to be present
            //This should be commented out if not using the KeyInfo in the XmlManager class 
           
            try{
                bool result = XmlManager.CheckSignature(txtSignedPayloadFile.Text); 
                if (result == false)
                {
                    MessageBox.Show("Signature is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Signature is valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                ex.DisplayException(Text);
                return;
            }

        }

        private void btnBrowsePayload_Click(object sender, EventArgs e)
        {
            {
                // load Signed Payload file, must be unencrypted and have KeyInfo element
                txtSignedPayloadFile.Text = dlgOpen.ShowDialogWithFilter("Signed Payload File (*.xml)|*.xml;");
            }
        }

        //this can be used to autopopulate these boxes during test periods
        private void cmdPopulateSettings_Click(object sender, EventArgs e)
        {
            txtXmlFile.Text = "";
            txtCert.Text = "";
            txtCertPass.Text = "";
            txtKeyCert.Text = "";
        }



        private void btnSchemaFile_Click(object sender, EventArgs e)
        {
            {
                // load Signed Payload file, must be unencrypted and have KeyInfo element
                txtSchemaFile.Text = dlgOpen.ShowDialogWithFilter("8966 XML File (*.xml)|*.xml;");
            }
        }

        private void btnSchemaFolder_Click(object sender, EventArgs e)
        {
            // Select folder location that holds all 8966 schema files
            if (dlgOpenFolder.ShowDialog() == DialogResult.OK)
            {
                txtSchemaFolder.Text = dlgOpenFolder.SelectedPath;
            }
        }

        private void btnCheckSchema_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSchemaFile.Text))
            {
                // files validation
                MessageBox.Show("The XML File To Validate was not specified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSchemaFolder.Text))
            {
                // files validation
                MessageBox.Show("The Schema Folder location was not specified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string validationError = XmlManager.CheckSchema(txtSchemaFile.Text, txtSchemaFolder.Text);
                if (validationError != "")
                {
                    // files validation
                    MessageBox.Show("Schema Validation Error:\r\n" + validationError, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    // files validation
                    MessageBox.Show("Validation Successful", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
             }
            catch (Exception ex)
            {
                ex.DisplayException("Schema Validation Failure: " + Text);
                return;
            }
        }


        private void chkSendFolder_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSendFolder.Checked)
            {
                lblLoadXML.Text = "XML Folder";
            }
            else
            {
                lblLoadXML.Text = "XML File";
            }
        }

        private void btnCheckNotification_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDecryptKey.Text))
            {
                // files validation
                MessageBox.Show("The Receiver's key for decryption must be set!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Char delimiter = ',';
            Dictionary<string, string> transmissions = new Dictionary<string, string>();

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("simplelog.csv"))
                {
                    while (sr.Peek() >= 0)
                    {
                        String line = sr.ReadLine();
                        string[] strLineArray = line.Split(delimiter);
                    
                        //if there is no notification id, we will store the file name
                        if (strLineArray[4] == "NOTIFICATIONID")
                        {
                            transmissions.Add(strLineArray[2], line);
                        }
                        else
                        // otherwise, we store the notification id, and we can ignore this file if it is in the inbox
                        {
                            transmissions.Add(strLineArray[4], line);
                        }
                        Console.WriteLine(line);
                    }
                }


            }
            catch (Exception ex)
            {
                ex.DisplayException(Text);
                return;
            }

            //Connect to the Inbox and see if there are any files
            try
            {
                // Setup session options
                SessionOptions currentSFTPSession = SFTPManager.CreateSFTPSession(cmbSFTPServers.SelectedItem.ToString(), username.Text, password.Text);
                SFTPManager.DownloadInbox(currentSFTPSession, transmissions, txtDownFolder.Text);
                            
            }
            catch (Exception ex)
            {
                ex.DisplayException(Text);
                return;
            }

            //loop through the files we have downloaded and process them
            //these files will be matched to our log file to see if we have any matches

            string[] filesToProcess;
            string xmlProcessedFolder = "";
            string xmlProcessingFolder = "";
            string xmlProcessedUnmatchedFolder = "";
            string destinationFolder = "";

            // Load all XML files in the folder into the array
            filesToProcess = Directory.GetFiles(txtDownFolder.Text, "*.zip", SearchOption.TopDirectoryOnly);
            xmlProcessedFolder = txtDownFolder.Text + "\\Processed";
            xmlProcessingFolder = txtDownFolder.Text + "\\Processing";
            xmlProcessedUnmatchedFolder = txtDownFolder.Text + "\\Processed\\Unmatched";

            //if the processedFolder doesn't exist, create it
            if (!Directory.Exists(xmlProcessedFolder))
            {
                Directory.CreateDirectory(xmlProcessedFolder);
            }
            //if the processedUnmatchedFolder doesn't exist, create it
            if (!Directory.Exists(xmlProcessedUnmatchedFolder))
            {
                Directory.CreateDirectory(xmlProcessedUnmatchedFolder);
            }
            //if the processingFolder doesn't exist, create it
            if (!Directory.Exists(xmlProcessingFolder))
            {
                Directory.CreateDirectory(xmlProcessingFolder);
            }
           
            //loop through the downloaded files
            int matchCounter = 0;
            int unmatchCounter = 0;
            foreach (string fileName in filesToProcess)
            {
                try
                {
                    //clean out the processing folder, the current file will be unzipped into it
                    DirectoryInfo dir = new DirectoryInfo(xmlProcessingFolder);
                    foreach (FileInfo fi in dir.GetFiles())
                    {
                        fi.Delete();
                    }
                    //Deflate the zip archive
                    ZipManager.ExtractArchive(fileName, xmlProcessingFolder, false);
                }
                catch (Exception ex)
                {
                    ex.DisplayException(Text);
                    return;
                }

                string decryptedPayload = "";
                try
                {
                    decryptedPayload = AesManager.DecryptNotification(xmlProcessingFolder, txtDecryptKey.Text, txtDecryptPassword.Text, radECB.Checked);
                }
                catch (Exception ex)
                {
                    ex.DisplayException("Decryption Failed:" + Text);
                    return;
                }
                          
                //take a look at the decrypted payload and see if this notification matches anything we are looking for
                string[] notificationID = new string[3];
                string notificationFileName = Path.GetFileNameWithoutExtension(fileName);
                bool isFileMatched = false;
                
                //This will return three values
                //0 Ides Transmission ID
                //1 Sender FIle Id
                //2 Return Code
                notificationID = XmlManager.CheckNotification(decryptedPayload);
                
                //Check our log file dictionary and see if we can find a match
                if (transmissions.ContainsKey(notificationID[1]) == true)
                {
                    isFileMatched = true;
                    
                    //we will add a new record to reflect the updated information and remove the old record   
                    //make sure the filename is in our transmissions and return the current log data for it
                    string currentNotificationData = "";
                    transmissions.TryGetValue(notificationID[1],out currentNotificationData);

                    //update the current notification Data before we add the new record
                    //we will take fields from the decrypted notification and insert it back in
                    currentNotificationData = currentNotificationData.Replace("NOTIFICATIONID", Path.GetFileName(fileName));
                    currentNotificationData = currentNotificationData.Replace("IDESTRANSID", notificationID[0]);
                    currentNotificationData = currentNotificationData.Replace("NULL", notificationID[2]);
                    
                    //add new record with updated information
                    transmissions.Add(Path.GetFileName(fileName), string.Join(",", currentNotificationData));
                    
                    //we can remove the old record now
                    transmissions.Remove(notificationID[1]);

                    //we will write the current transmissions back into the log file so we keep it updated with the latest information
                    //write to log
                    string filePath = @"simplelog.csv";
                    using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        using (TextWriter tw = new StreamWriter(fs))

                            foreach (KeyValuePair<string, string> kvp in transmissions)
                            {
                                tw.WriteLine(string.Format("{0}", kvp.Value));
                            }
                    }
                    //processing of the file is complete
                    
                }
                //the decrypted files will be moved to the Processed folder for safekeeping
                //if no match is found, it moves to the Unmatched subfolder
                DirectoryInfo dirProcessing = new DirectoryInfo(xmlProcessingFolder);
                if (isFileMatched == true)
                {
                    destinationFolder = xmlProcessedFolder;
                    if (!Directory.Exists(xmlProcessedFolder + "\\" + notificationFileName))
                    {
                        Directory.CreateDirectory(xmlProcessedFolder + "\\" + notificationFileName);
                    }
                    matchCounter = matchCounter + 1;   
                }
                else{
                    destinationFolder = xmlProcessedUnmatchedFolder;
                    //if this non-matching file has already been pulled, remove the old and replace with the new
                    if (Directory.Exists(xmlProcessedUnmatchedFolder + "\\" + notificationFileName))
                    {
                        //clean out the folder, it will get the current decrypted contents
                        DirectoryInfo dir = new DirectoryInfo(xmlProcessedUnmatchedFolder + "\\" + notificationFileName);
                        foreach (FileInfo fi in dir.GetFiles())
                        {
                            fi.Delete();
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(xmlProcessedUnmatchedFolder + "\\" + notificationFileName);
                    }              
                    unmatchCounter = unmatchCounter + 1;   
                }
                
                //move from the processing folder
                foreach (FileInfo fi in dirProcessing.GetFiles())
                {
                    File.Move(fi.FullName, Path.Combine(destinationFolder, notificationFileName, fi.Name));
                }
                //Rename the file so we can process it
                File.Move(fileName, Path.Combine(destinationFolder, notificationFileName, Path.GetFileName(fileName)));
            }
            MessageBox.Show(matchCounter + " matching files found\n" + unmatchCounter + " non-matching files found");
        }

        private void btnBrowseDecKey_Click(object sender, EventArgs e)
        {
            // load Notification Receiver key
            txtDecryptKey.Text = dlgOpen.ShowDialogWithFilter("Certificate Files (*.cer, *.pfx, *.p12)|*.cer;*.pfx;*.p12");   
        }

        private void btnBrowseDownloadFolder_Click(object sender, EventArgs e)
        {
            // Select folder location that holds all 8966 XML files to process
            if (dlgOpenFolder.ShowDialog() == DialogResult.OK)
            {
               txtDownFolder.Text = dlgOpenFolder.SelectedPath;
            }
        }

       
    }
}
