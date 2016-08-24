namespace WindowsFormsApplication1
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.dlgOpenFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cmbTaxYear = new System.Windows.Forms.ComboBox();
            this.lblTaxYear = new System.Windows.Forms.Label();
            this.chkAutoSendSFTP = new System.Windows.Forms.CheckBox();
            this.chkSendFolder = new System.Windows.Forms.CheckBox();
            this.chkSignatureValidation = new System.Windows.Forms.CheckBox();
            this.chkSchemaValidation = new System.Windows.Forms.CheckBox();
            this.radCBC = new System.Windows.Forms.RadioButton();
            this.radECB = new System.Windows.Forms.RadioButton();
            this.chkM1O2 = new System.Windows.Forms.CheckBox();
            this.cmdPopulateSettings = new System.Windows.Forms.Button();
            this.btnBrowseHCTACert = new System.Windows.Forms.Button();
            this.txtHCTACode = new System.Windows.Forms.TextBox();
            this.lblHCTACode = new System.Windows.Forms.Label();
            this.txtHCTACertPassword = new System.Windows.Forms.TextBox();
            this.lblEncryptionHCTAPassword = new System.Windows.Forms.Label();
            this.txtHCTACert = new System.Windows.Forms.TextBox();
            this.lblHCTAKey = new System.Windows.Forms.Label();
            this.btnSignXML = new System.Windows.Forms.Button();
            this.txtKeyCertPassword = new System.Windows.Forms.TextBox();
            this.lblKeyEncryptionCertPassword = new System.Windows.Forms.Label();
            this.btnBrowseKeyCert = new System.Windows.Forms.Button();
            this.txtKeyCert = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCertPass = new System.Windows.Forms.TextBox();
            this.lblCertPass = new System.Windows.Forms.Label();
            this.btnBrowseCert = new System.Windows.Forms.Button();
            this.txtCert = new System.Windows.Forms.TextBox();
            this.lblCert = new System.Windows.Forms.Label();
            this.btnBrowseXml = new System.Windows.Forms.Button();
            this.txtXmlFile = new System.Windows.Forms.TextBox();
            this.lblLoadXML = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtNotificationFolder = new System.Windows.Forms.TextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.btnDecryptZip = new System.Windows.Forms.Button();
            this.txtRecKeyPassword = new System.Windows.Forms.TextBox();
            this.lblRecPass = new System.Windows.Forms.Label();
            this.btnBrowseRecCert = new System.Windows.Forms.Button();
            this.txtReceiverCert = new System.Windows.Forms.TextBox();
            this.lblReceiverCert = new System.Windows.Forms.Label();
            this.btnBrowseNotificationZip = new System.Windows.Forms.Button();
            this.txtNotificationZip = new System.Windows.Forms.TextBox();
            this.lblZipFile = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cmdCheckSignature = new System.Windows.Forms.Button();
            this.lblSchemaFolder = new System.Windows.Forms.Label();
            this.btnSchemaFolder = new System.Windows.Forms.Button();
            this.txtSchemaFolder = new System.Windows.Forms.TextBox();
            this.btnSchemaFile = new System.Windows.Forms.Button();
            this.txtSchemaFile = new System.Windows.Forms.TextBox();
            this.lblSchemaFile = new System.Windows.Forms.Label();
            this.btnCheckSchema = new System.Windows.Forms.Button();
            this.btnBrowsePayload = new System.Windows.Forms.Button();
            this.txtSignedPayloadFile = new System.Windows.Forms.TextBox();
            this.lblSignedPayloadFile = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnBrowseDownloadFolder = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDownFolder = new System.Windows.Forms.TextBox();
            this.cmbSFTPServers = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDecryptPassword = new System.Windows.Forms.TextBox();
            this.lblDecryptPassword = new System.Windows.Forms.Label();
            this.btnBrowseDecKey = new System.Windows.Forms.Button();
            this.txtDecryptKey = new System.Windows.Forms.TextBox();
            this.lblDecryptKey = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.btnCheckNotification = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlgOpen
            // 
            this.dlgOpen.Title = "Open File";
            // 
            // dlgSave
            // 
            this.dlgSave.Filter = "XML Files (*.xml)|*.xml";
            this.dlgSave.Title = "Save File";
            // 
            // dlgOpenFolder
            // 
            this.dlgOpenFolder.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(902, 438);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 30;
            this.button1.Text = "EXIT";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(860, 464);
            this.tabControl1.TabIndex = 63;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage1.Controls.Add(this.cmbTaxYear);
            this.tabPage1.Controls.Add(this.lblTaxYear);
            this.tabPage1.Controls.Add(this.chkAutoSendSFTP);
            this.tabPage1.Controls.Add(this.chkSendFolder);
            this.tabPage1.Controls.Add(this.chkSignatureValidation);
            this.tabPage1.Controls.Add(this.chkSchemaValidation);
            this.tabPage1.Controls.Add(this.radCBC);
            this.tabPage1.Controls.Add(this.radECB);
            this.tabPage1.Controls.Add(this.chkM1O2);
            this.tabPage1.Controls.Add(this.cmdPopulateSettings);
            this.tabPage1.Controls.Add(this.btnBrowseHCTACert);
            this.tabPage1.Controls.Add(this.txtHCTACode);
            this.tabPage1.Controls.Add(this.lblHCTACode);
            this.tabPage1.Controls.Add(this.txtHCTACertPassword);
            this.tabPage1.Controls.Add(this.lblEncryptionHCTAPassword);
            this.tabPage1.Controls.Add(this.txtHCTACert);
            this.tabPage1.Controls.Add(this.lblHCTAKey);
            this.tabPage1.Controls.Add(this.btnSignXML);
            this.tabPage1.Controls.Add(this.txtKeyCertPassword);
            this.tabPage1.Controls.Add(this.lblKeyEncryptionCertPassword);
            this.tabPage1.Controls.Add(this.btnBrowseKeyCert);
            this.tabPage1.Controls.Add(this.txtKeyCert);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtCertPass);
            this.tabPage1.Controls.Add(this.lblCertPass);
            this.tabPage1.Controls.Add(this.btnBrowseCert);
            this.tabPage1.Controls.Add(this.txtCert);
            this.tabPage1.Controls.Add(this.lblCert);
            this.tabPage1.Controls.Add(this.btnBrowseXml);
            this.tabPage1.Controls.Add(this.txtXmlFile);
            this.tabPage1.Controls.Add(this.lblLoadXML);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(852, 438);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Create Data Packet";
            // 
            // cmbTaxYear
            // 
            this.cmbTaxYear.FormattingEnabled = true;
            this.cmbTaxYear.Location = new System.Drawing.Point(529, 36);
            this.cmbTaxYear.Name = "cmbTaxYear";
            this.cmbTaxYear.Size = new System.Drawing.Size(78, 21);
            this.cmbTaxYear.TabIndex = 104;
            // 
            // lblTaxYear
            // 
            this.lblTaxYear.AutoSize = true;
            this.lblTaxYear.Location = new System.Drawing.Point(526, 18);
            this.lblTaxYear.Name = "lblTaxYear";
            this.lblTaxYear.Size = new System.Drawing.Size(50, 13);
            this.lblTaxYear.TabIndex = 103;
            this.lblTaxYear.Text = "Tax Year";
            // 
            // chkAutoSendSFTP
            // 
            this.chkAutoSendSFTP.AutoSize = true;
            this.chkAutoSendSFTP.Location = new System.Drawing.Point(706, 146);
            this.chkAutoSendSFTP.Name = "chkAutoSendSFTP";
            this.chkAutoSendSFTP.Size = new System.Drawing.Size(97, 17);
            this.chkAutoSendSFTP.TabIndex = 102;
            this.chkAutoSendSFTP.Text = "Auto SFTP File";
            this.chkAutoSendSFTP.UseVisualStyleBackColor = true;
            // 
            // chkSendFolder
            // 
            this.chkSendFolder.AutoSize = true;
            this.chkSendFolder.Location = new System.Drawing.Point(706, 169);
            this.chkSendFolder.Name = "chkSendFolder";
            this.chkSendFolder.Size = new System.Drawing.Size(113, 17);
            this.chkSendFolder.TabIndex = 101;
            this.chkSendFolder.Text = "Send Entire Folder";
            this.chkSendFolder.UseVisualStyleBackColor = true;
            this.chkSendFolder.CheckedChanged += new System.EventHandler(this.chkSendFolder_CheckedChanged);
            // 
            // chkSignatureValidation
            // 
            this.chkSignatureValidation.AutoSize = true;
            this.chkSignatureValidation.Location = new System.Drawing.Point(706, 120);
            this.chkSignatureValidation.Name = "chkSignatureValidation";
            this.chkSignatureValidation.Size = new System.Drawing.Size(120, 17);
            this.chkSignatureValidation.TabIndex = 100;
            this.chkSignatureValidation.Text = "Signature Validation";
            this.chkSignatureValidation.UseVisualStyleBackColor = true;
            // 
            // chkSchemaValidation
            // 
            this.chkSchemaValidation.AutoSize = true;
            this.chkSchemaValidation.Location = new System.Drawing.Point(706, 95);
            this.chkSchemaValidation.Name = "chkSchemaValidation";
            this.chkSchemaValidation.Size = new System.Drawing.Size(114, 17);
            this.chkSchemaValidation.TabIndex = 99;
            this.chkSchemaValidation.Text = "Schema Validation";
            this.chkSchemaValidation.UseVisualStyleBackColor = true;
            // 
            // radCBC
            // 
            this.radCBC.AutoSize = true;
            this.radCBC.Location = new System.Drawing.Point(706, 45);
            this.radCBC.Name = "radCBC";
            this.radCBC.Size = new System.Drawing.Size(76, 17);
            this.radCBC.TabIndex = 98;
            this.radCBC.Text = "CBC Mode";
            this.radCBC.UseVisualStyleBackColor = true;
            // 
            // radECB
            // 
            this.radECB.AutoSize = true;
            this.radECB.Checked = true;
            this.radECB.Location = new System.Drawing.Point(706, 62);
            this.radECB.Name = "radECB";
            this.radECB.Size = new System.Drawing.Size(76, 17);
            this.radECB.TabIndex = 97;
            this.radECB.TabStop = true;
            this.radECB.Text = "ECB Mode";
            this.radECB.UseVisualStyleBackColor = true;
            // 
            // chkM1O2
            // 
            this.chkM1O2.AutoSize = true;
            this.chkM1O2.Location = new System.Drawing.Point(706, 20);
            this.chkM1O2.Name = "chkM1O2";
            this.chkM1O2.Size = new System.Drawing.Size(93, 17);
            this.chkM1O2.TabIndex = 96;
            this.chkM1O2.Text = "Model 1 Opt 2";
            this.chkM1O2.UseVisualStyleBackColor = true;
            this.chkM1O2.CheckedChanged += new System.EventHandler(this.chkM1O2_CheckedChanged);
            // 
            // cmdPopulateSettings
            // 
            this.cmdPopulateSettings.Location = new System.Drawing.Point(207, 12);
            this.cmdPopulateSettings.Name = "cmdPopulateSettings";
            this.cmdPopulateSettings.Size = new System.Drawing.Size(75, 23);
            this.cmdPopulateSettings.TabIndex = 68;
            this.cmdPopulateSettings.Text = "Populate";
            this.cmdPopulateSettings.UseVisualStyleBackColor = true;
            this.cmdPopulateSettings.Visible = false;
            // 
            // btnBrowseHCTACert
            // 
            this.btnBrowseHCTACert.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseHCTACert.Location = new System.Drawing.Point(294, 312);
            this.btnBrowseHCTACert.Name = "btnBrowseHCTACert";
            this.btnBrowseHCTACert.Size = new System.Drawing.Size(28, 23);
            this.btnBrowseHCTACert.TabIndex = 67;
            this.btnBrowseHCTACert.Text = "...";
            this.btnBrowseHCTACert.UseVisualStyleBackColor = true;
            this.btnBrowseHCTACert.Click += new System.EventHandler(this.btnBrowseHCTACert_Click);
            // 
            // txtHCTACode
            // 
            this.txtHCTACode.Location = new System.Drawing.Point(23, 396);
            this.txtHCTACode.Name = "txtHCTACode";
            this.txtHCTACode.Size = new System.Drawing.Size(78, 20);
            this.txtHCTACode.TabIndex = 66;
            // 
            // lblHCTACode
            // 
            this.lblHCTACode.AutoSize = true;
            this.lblHCTACode.Location = new System.Drawing.Point(23, 380);
            this.lblHCTACode.Name = "lblHCTACode";
            this.lblHCTACode.Size = new System.Drawing.Size(64, 13);
            this.lblHCTACode.TabIndex = 65;
            this.lblHCTACode.Text = "HCTA Code";
            // 
            // txtHCTACertPassword
            // 
            this.txtHCTACertPassword.Location = new System.Drawing.Point(23, 353);
            this.txtHCTACertPassword.Name = "txtHCTACertPassword";
            this.txtHCTACertPassword.Size = new System.Drawing.Size(259, 20);
            this.txtHCTACertPassword.TabIndex = 64;
            this.txtHCTACertPassword.Visible = false;
            // 
            // lblEncryptionHCTAPassword
            // 
            this.lblEncryptionHCTAPassword.AutoSize = true;
            this.lblEncryptionHCTAPassword.Location = new System.Drawing.Point(23, 337);
            this.lblEncryptionHCTAPassword.Name = "lblEncryptionHCTAPassword";
            this.lblEncryptionHCTAPassword.Size = new System.Drawing.Size(261, 13);
            this.lblEncryptionHCTAPassword.TabIndex = 63;
            this.lblEncryptionHCTAPassword.Text = "Encryption Key Certificate HCTA password (if needed)";
            this.lblEncryptionHCTAPassword.Visible = false;
            // 
            // txtHCTACert
            // 
            this.txtHCTACert.Location = new System.Drawing.Point(23, 312);
            this.txtHCTACert.Name = "txtHCTACert";
            this.txtHCTACert.Size = new System.Drawing.Size(259, 20);
            this.txtHCTACert.TabIndex = 62;
            this.txtHCTACert.Visible = false;
            // 
            // lblHCTAKey
            // 
            this.lblHCTAKey.AutoSize = true;
            this.lblHCTAKey.Location = new System.Drawing.Point(23, 296);
            this.lblHCTAKey.Name = "lblHCTAKey";
            this.lblHCTAKey.Size = new System.Drawing.Size(219, 13);
            this.lblHCTAKey.TabIndex = 61;
            this.lblHCTAKey.Text = "Encryption Key Certificate (HCTA Public Key)";
            this.lblHCTAKey.Visible = false;
            // 
            // btnSignXML
            // 
            this.btnSignXML.Location = new System.Drawing.Point(23, 256);
            this.btnSignXML.Name = "btnSignXML";
            this.btnSignXML.Size = new System.Drawing.Size(173, 23);
            this.btnSignXML.TabIndex = 60;
            this.btnSignXML.Text = "Sign and Encrypt XML";
            this.btnSignXML.UseVisualStyleBackColor = true;
            this.btnSignXML.Click += new System.EventHandler(this.btnSignXML_Click);
            // 
            // txtKeyCertPassword
            // 
            this.txtKeyCertPassword.Location = new System.Drawing.Point(23, 225);
            this.txtKeyCertPassword.Name = "txtKeyCertPassword";
            this.txtKeyCertPassword.Size = new System.Drawing.Size(259, 20);
            this.txtKeyCertPassword.TabIndex = 59;
            // 
            // lblKeyEncryptionCertPassword
            // 
            this.lblKeyEncryptionCertPassword.AutoSize = true;
            this.lblKeyEncryptionCertPassword.Location = new System.Drawing.Point(23, 209);
            this.lblKeyEncryptionCertPassword.Name = "lblKeyEncryptionCertPassword";
            this.lblKeyEncryptionCertPassword.Size = new System.Drawing.Size(229, 13);
            this.lblKeyEncryptionCertPassword.TabIndex = 58;
            this.lblKeyEncryptionCertPassword.Text = "Encryption Key Certificate password (if needed)";
            // 
            // btnBrowseKeyCert
            // 
            this.btnBrowseKeyCert.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseKeyCert.Location = new System.Drawing.Point(294, 177);
            this.btnBrowseKeyCert.Name = "btnBrowseKeyCert";
            this.btnBrowseKeyCert.Size = new System.Drawing.Size(28, 23);
            this.btnBrowseKeyCert.TabIndex = 57;
            this.btnBrowseKeyCert.Text = "...";
            this.btnBrowseKeyCert.UseVisualStyleBackColor = true;
            this.btnBrowseKeyCert.Click += new System.EventHandler(this.btnBrowseKeyCert_Click);
            // 
            // txtKeyCert
            // 
            this.txtKeyCert.Location = new System.Drawing.Point(23, 179);
            this.txtKeyCert.Name = "txtKeyCert";
            this.txtKeyCert.Size = new System.Drawing.Size(259, 20);
            this.txtKeyCert.TabIndex = 56;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(240, 13);
            this.label3.TabIndex = 55;
            this.label3.Text = "Encryption Key Certificate (Receiver\'s Public Key)";
            // 
            // txtCertPass
            // 
            this.txtCertPass.Location = new System.Drawing.Point(23, 135);
            this.txtCertPass.Name = "txtCertPass";
            this.txtCertPass.PasswordChar = '*';
            this.txtCertPass.Size = new System.Drawing.Size(259, 20);
            this.txtCertPass.TabIndex = 54;
            // 
            // lblCertPass
            // 
            this.lblCertPass.AutoSize = true;
            this.lblCertPass.Location = new System.Drawing.Point(23, 119);
            this.lblCertPass.Name = "lblCertPass";
            this.lblCertPass.Size = new System.Drawing.Size(141, 13);
            this.lblCertPass.TabIndex = 53;
            this.lblCertPass.Text = "Signing Certificate Password";
            // 
            // btnBrowseCert
            // 
            this.btnBrowseCert.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseCert.Location = new System.Drawing.Point(294, 88);
            this.btnBrowseCert.Name = "btnBrowseCert";
            this.btnBrowseCert.Size = new System.Drawing.Size(28, 23);
            this.btnBrowseCert.TabIndex = 52;
            this.btnBrowseCert.Text = "...";
            this.btnBrowseCert.UseVisualStyleBackColor = true;
            this.btnBrowseCert.Click += new System.EventHandler(this.btnBrowseCert_Click);
            // 
            // txtCert
            // 
            this.txtCert.Location = new System.Drawing.Point(23, 92);
            this.txtCert.Name = "txtCert";
            this.txtCert.Size = new System.Drawing.Size(259, 20);
            this.txtCert.TabIndex = 51;
            // 
            // lblCert
            // 
            this.lblCert.AutoSize = true;
            this.lblCert.Location = new System.Drawing.Point(23, 76);
            this.lblCert.Name = "lblCert";
            this.lblCert.Size = new System.Drawing.Size(199, 13);
            this.lblCert.TabIndex = 50;
            this.lblCert.Text = "Signing Certificate (Sender\'s Private Key)";
            // 
            // btnBrowseXml
            // 
            this.btnBrowseXml.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseXml.Location = new System.Drawing.Point(294, 38);
            this.btnBrowseXml.Name = "btnBrowseXml";
            this.btnBrowseXml.Size = new System.Drawing.Size(28, 23);
            this.btnBrowseXml.TabIndex = 49;
            this.btnBrowseXml.Text = "...";
            this.btnBrowseXml.UseVisualStyleBackColor = true;
            this.btnBrowseXml.Click += new System.EventHandler(this.btnBrowseXml_Click);
            // 
            // txtXmlFile
            // 
            this.txtXmlFile.Location = new System.Drawing.Point(23, 41);
            this.txtXmlFile.Name = "txtXmlFile";
            this.txtXmlFile.Size = new System.Drawing.Size(259, 20);
            this.txtXmlFile.TabIndex = 48;
            // 
            // lblLoadXML
            // 
            this.lblLoadXML.AutoSize = true;
            this.lblLoadXML.Location = new System.Drawing.Point(23, 25);
            this.lblLoadXML.Name = "lblLoadXML";
            this.lblLoadXML.Size = new System.Drawing.Size(48, 13);
            this.lblLoadXML.TabIndex = 47;
            this.lblLoadXML.Text = "XML File";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage2.Controls.Add(this.btnBrowseOutput);
            this.tabPage2.Controls.Add(this.txtNotificationFolder);
            this.tabPage2.Controls.Add(this.lblOutput);
            this.tabPage2.Controls.Add(this.btnDecryptZip);
            this.tabPage2.Controls.Add(this.txtRecKeyPassword);
            this.tabPage2.Controls.Add(this.lblRecPass);
            this.tabPage2.Controls.Add(this.btnBrowseRecCert);
            this.tabPage2.Controls.Add(this.txtReceiverCert);
            this.tabPage2.Controls.Add(this.lblReceiverCert);
            this.tabPage2.Controls.Add(this.btnBrowseNotificationZip);
            this.tabPage2.Controls.Add(this.txtNotificationZip);
            this.tabPage2.Controls.Add(this.lblZipFile);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(852, 438);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Decrypt Notification";
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseOutput.Location = new System.Drawing.Point(246, 172);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseOutput.TabIndex = 74;
            this.btnBrowseOutput.Text = "...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // txtNotificationFolder
            // 
            this.txtNotificationFolder.Location = new System.Drawing.Point(22, 172);
            this.txtNotificationFolder.Name = "txtNotificationFolder";
            this.txtNotificationFolder.Size = new System.Drawing.Size(212, 20);
            this.txtNotificationFolder.TabIndex = 73;
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(22, 156);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(127, 13);
            this.lblOutput.TabIndex = 72;
            this.lblOutput.Text = "Notification Output Folder";
            // 
            // btnDecryptZip
            // 
            this.btnDecryptZip.Location = new System.Drawing.Point(22, 200);
            this.btnDecryptZip.Name = "btnDecryptZip";
            this.btnDecryptZip.Size = new System.Drawing.Size(173, 23);
            this.btnDecryptZip.TabIndex = 71;
            this.btnDecryptZip.Text = "Decrypt Notification";
            this.btnDecryptZip.UseVisualStyleBackColor = true;
            this.btnDecryptZip.Click += new System.EventHandler(this.btnDecryptZip_Click);
            // 
            // txtRecKeyPassword
            // 
            this.txtRecKeyPassword.Location = new System.Drawing.Point(22, 133);
            this.txtRecKeyPassword.Name = "txtRecKeyPassword";
            this.txtRecKeyPassword.PasswordChar = '*';
            this.txtRecKeyPassword.Size = new System.Drawing.Size(212, 20);
            this.txtRecKeyPassword.TabIndex = 70;
            // 
            // lblRecPass
            // 
            this.lblRecPass.AutoSize = true;
            this.lblRecPass.Location = new System.Drawing.Point(25, 117);
            this.lblRecPass.Name = "lblRecPass";
            this.lblRecPass.Size = new System.Drawing.Size(155, 13);
            this.lblRecPass.TabIndex = 69;
            this.lblRecPass.Text = "Certificate password (if needed)";
            // 
            // btnBrowseRecCert
            // 
            this.btnBrowseRecCert.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseRecCert.Location = new System.Drawing.Point(246, 87);
            this.btnBrowseRecCert.Name = "btnBrowseRecCert";
            this.btnBrowseRecCert.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseRecCert.TabIndex = 68;
            this.btnBrowseRecCert.Text = "...";
            this.btnBrowseRecCert.UseVisualStyleBackColor = true;
            this.btnBrowseRecCert.Click += new System.EventHandler(this.btnBrowseRecCert_Click);
            // 
            // txtReceiverCert
            // 
            this.txtReceiverCert.Location = new System.Drawing.Point(22, 90);
            this.txtReceiverCert.Name = "txtReceiverCert";
            this.txtReceiverCert.Size = new System.Drawing.Size(212, 20);
            this.txtReceiverCert.TabIndex = 67;
            // 
            // lblReceiverCert
            // 
            this.lblReceiverCert.AutoSize = true;
            this.lblReceiverCert.Location = new System.Drawing.Point(25, 74);
            this.lblReceiverCert.Name = "lblReceiverCert";
            this.lblReceiverCert.Size = new System.Drawing.Size(209, 13);
            this.lblReceiverCert.TabIndex = 66;
            this.lblReceiverCert.Text = "Receiver Certificate (Receiver Private Key)";
            // 
            // btnBrowseNotificationZip
            // 
            this.btnBrowseNotificationZip.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseNotificationZip.Location = new System.Drawing.Point(246, 40);
            this.btnBrowseNotificationZip.Name = "btnBrowseNotificationZip";
            this.btnBrowseNotificationZip.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseNotificationZip.TabIndex = 65;
            this.btnBrowseNotificationZip.Text = "...";
            this.btnBrowseNotificationZip.UseVisualStyleBackColor = true;
            this.btnBrowseNotificationZip.Click += new System.EventHandler(this.btnBrowseNotificationZip_Click);
            // 
            // txtNotificationZip
            // 
            this.txtNotificationZip.Location = new System.Drawing.Point(22, 40);
            this.txtNotificationZip.Name = "txtNotificationZip";
            this.txtNotificationZip.Size = new System.Drawing.Size(212, 20);
            this.txtNotificationZip.TabIndex = 64;
            // 
            // lblZipFile
            // 
            this.lblZipFile.AutoSize = true;
            this.lblZipFile.Location = new System.Drawing.Point(22, 24);
            this.lblZipFile.Name = "lblZipFile";
            this.lblZipFile.Size = new System.Drawing.Size(41, 13);
            this.lblZipFile.TabIndex = 63;
            this.lblZipFile.Text = "Zip File";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage3.Controls.Add(this.cmdCheckSignature);
            this.tabPage3.Controls.Add(this.lblSchemaFolder);
            this.tabPage3.Controls.Add(this.btnSchemaFolder);
            this.tabPage3.Controls.Add(this.txtSchemaFolder);
            this.tabPage3.Controls.Add(this.btnSchemaFile);
            this.tabPage3.Controls.Add(this.txtSchemaFile);
            this.tabPage3.Controls.Add(this.lblSchemaFile);
            this.tabPage3.Controls.Add(this.btnCheckSchema);
            this.tabPage3.Controls.Add(this.btnBrowsePayload);
            this.tabPage3.Controls.Add(this.txtSignedPayloadFile);
            this.tabPage3.Controls.Add(this.lblSignedPayloadFile);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(852, 438);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Utilities";
            // 
            // cmdCheckSignature
            // 
            this.cmdCheckSignature.Location = new System.Drawing.Point(25, 66);
            this.cmdCheckSignature.Name = "cmdCheckSignature";
            this.cmdCheckSignature.Size = new System.Drawing.Size(103, 23);
            this.cmdCheckSignature.TabIndex = 101;
            this.cmdCheckSignature.Text = "Check Signature";
            this.cmdCheckSignature.UseVisualStyleBackColor = true;
            this.cmdCheckSignature.Click += new System.EventHandler(this.cmdCheckSignature_Click);
            // 
            // lblSchemaFolder
            // 
            this.lblSchemaFolder.AutoSize = true;
            this.lblSchemaFolder.Location = new System.Drawing.Point(516, 24);
            this.lblSchemaFolder.Name = "lblSchemaFolder";
            this.lblSchemaFolder.Size = new System.Drawing.Size(78, 13);
            this.lblSchemaFolder.TabIndex = 95;
            this.lblSchemaFolder.Text = "Schema Folder";
            // 
            // btnSchemaFolder
            // 
            this.btnSchemaFolder.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSchemaFolder.Location = new System.Drawing.Point(740, 39);
            this.btnSchemaFolder.Name = "btnSchemaFolder";
            this.btnSchemaFolder.Size = new System.Drawing.Size(29, 23);
            this.btnSchemaFolder.TabIndex = 94;
            this.btnSchemaFolder.Text = "...";
            this.btnSchemaFolder.UseVisualStyleBackColor = true;
            this.btnSchemaFolder.Click += new System.EventHandler(this.btnSchemaFolder_Click);
            // 
            // txtSchemaFolder
            // 
            this.txtSchemaFolder.Location = new System.Drawing.Point(516, 42);
            this.txtSchemaFolder.Name = "txtSchemaFolder";
            this.txtSchemaFolder.Size = new System.Drawing.Size(212, 20);
            this.txtSchemaFolder.TabIndex = 93;
            // 
            // btnSchemaFile
            // 
            this.btnSchemaFile.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSchemaFile.Location = new System.Drawing.Point(740, 82);
            this.btnSchemaFile.Name = "btnSchemaFile";
            this.btnSchemaFile.Size = new System.Drawing.Size(29, 23);
            this.btnSchemaFile.TabIndex = 92;
            this.btnSchemaFile.Text = "...";
            this.btnSchemaFile.UseVisualStyleBackColor = true;
            this.btnSchemaFile.Click += new System.EventHandler(this.btnSchemaFile_Click);
            // 
            // txtSchemaFile
            // 
            this.txtSchemaFile.Location = new System.Drawing.Point(516, 82);
            this.txtSchemaFile.Name = "txtSchemaFile";
            this.txtSchemaFile.Size = new System.Drawing.Size(212, 20);
            this.txtSchemaFile.TabIndex = 91;
            // 
            // lblSchemaFile
            // 
            this.lblSchemaFile.AutoSize = true;
            this.lblSchemaFile.Location = new System.Drawing.Point(516, 66);
            this.lblSchemaFile.Name = "lblSchemaFile";
            this.lblSchemaFile.Size = new System.Drawing.Size(105, 13);
            this.lblSchemaFile.TabIndex = 90;
            this.lblSchemaFile.Text = "XML File To Validate";
            // 
            // btnCheckSchema
            // 
            this.btnCheckSchema.Location = new System.Drawing.Point(516, 108);
            this.btnCheckSchema.Name = "btnCheckSchema";
            this.btnCheckSchema.Size = new System.Drawing.Size(100, 25);
            this.btnCheckSchema.TabIndex = 89;
            this.btnCheckSchema.Text = "Check Schema";
            this.btnCheckSchema.UseVisualStyleBackColor = true;
            this.btnCheckSchema.Click += new System.EventHandler(this.btnCheckSchema_Click);
            // 
            // btnBrowsePayload
            // 
            this.btnBrowsePayload.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowsePayload.Location = new System.Drawing.Point(249, 40);
            this.btnBrowsePayload.Name = "btnBrowsePayload";
            this.btnBrowsePayload.Size = new System.Drawing.Size(29, 23);
            this.btnBrowsePayload.TabIndex = 85;
            this.btnBrowsePayload.Text = "...";
            this.btnBrowsePayload.UseVisualStyleBackColor = true;
            this.btnBrowsePayload.Click += new System.EventHandler(this.btnBrowsePayload_Click);
            // 
            // txtSignedPayloadFile
            // 
            this.txtSignedPayloadFile.Location = new System.Drawing.Point(25, 40);
            this.txtSignedPayloadFile.Name = "txtSignedPayloadFile";
            this.txtSignedPayloadFile.Size = new System.Drawing.Size(212, 20);
            this.txtSignedPayloadFile.TabIndex = 84;
            // 
            // lblSignedPayloadFile
            // 
            this.lblSignedPayloadFile.AutoSize = true;
            this.lblSignedPayloadFile.Location = new System.Drawing.Point(25, 24);
            this.lblSignedPayloadFile.Name = "lblSignedPayloadFile";
            this.lblSignedPayloadFile.Size = new System.Drawing.Size(100, 13);
            this.lblSignedPayloadFile.TabIndex = 83;
            this.lblSignedPayloadFile.Text = "Signed Payload File";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage4.Controls.Add(this.btnBrowseDownloadFolder);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.txtDownFolder);
            this.tabPage4.Controls.Add(this.cmbSFTPServers);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.txtDecryptPassword);
            this.tabPage4.Controls.Add(this.lblDecryptPassword);
            this.tabPage4.Controls.Add(this.btnBrowseDecKey);
            this.tabPage4.Controls.Add(this.txtDecryptKey);
            this.tabPage4.Controls.Add(this.lblDecryptKey);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.password);
            this.tabPage4.Controls.Add(this.username);
            this.tabPage4.Controls.Add(this.btnCheckNotification);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(852, 438);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "SFTP Settings";
            // 
            // btnBrowseDownloadFolder
            // 
            this.btnBrowseDownloadFolder.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseDownloadFolder.Location = new System.Drawing.Point(245, 257);
            this.btnBrowseDownloadFolder.Name = "btnBrowseDownloadFolder";
            this.btnBrowseDownloadFolder.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseDownloadFolder.TabIndex = 127;
            this.btnBrowseDownloadFolder.Text = "...";
            this.btnBrowseDownloadFolder.UseVisualStyleBackColor = true;
            this.btnBrowseDownloadFolder.Click += new System.EventHandler(this.btnBrowseDownloadFolder_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 13);
            this.label6.TabIndex = 126;
            this.label6.Text = "Notification Download Folder";
            // 
            // txtDownFolder
            // 
            this.txtDownFolder.Location = new System.Drawing.Point(21, 259);
            this.txtDownFolder.Name = "txtDownFolder";
            this.txtDownFolder.Size = new System.Drawing.Size(212, 20);
            this.txtDownFolder.TabIndex = 125;
            // 
            // cmbSFTPServers
            // 
            this.cmbSFTPServers.FormattingEnabled = true;
            this.cmbSFTPServers.Location = new System.Drawing.Point(21, 34);
            this.cmbSFTPServers.Name = "cmbSFTPServers";
            this.cmbSFTPServers.Size = new System.Drawing.Size(254, 21);
            this.cmbSFTPServers.TabIndex = 124;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 123;
            this.label5.Text = "SFTP Server";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 233);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 122;
            // 
            // txtDecryptPassword
            // 
            this.txtDecryptPassword.Location = new System.Drawing.Point(21, 210);
            this.txtDecryptPassword.Name = "txtDecryptPassword";
            this.txtDecryptPassword.PasswordChar = '*';
            this.txtDecryptPassword.Size = new System.Drawing.Size(212, 20);
            this.txtDecryptPassword.TabIndex = 121;
            // 
            // lblDecryptPassword
            // 
            this.lblDecryptPassword.AutoSize = true;
            this.lblDecryptPassword.Location = new System.Drawing.Point(21, 194);
            this.lblDecryptPassword.Name = "lblDecryptPassword";
            this.lblDecryptPassword.Size = new System.Drawing.Size(155, 13);
            this.lblDecryptPassword.TabIndex = 120;
            this.lblDecryptPassword.Text = "Certificate password (if needed)";
            // 
            // btnBrowseDecKey
            // 
            this.btnBrowseDecKey.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseDecKey.Location = new System.Drawing.Point(245, 164);
            this.btnBrowseDecKey.Name = "btnBrowseDecKey";
            this.btnBrowseDecKey.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseDecKey.TabIndex = 119;
            this.btnBrowseDecKey.Text = "...";
            this.btnBrowseDecKey.UseVisualStyleBackColor = true;
            this.btnBrowseDecKey.Click += new System.EventHandler(this.btnBrowseDecKey_Click);
            // 
            // txtDecryptKey
            // 
            this.txtDecryptKey.Location = new System.Drawing.Point(21, 167);
            this.txtDecryptKey.Name = "txtDecryptKey";
            this.txtDecryptKey.Size = new System.Drawing.Size(212, 20);
            this.txtDecryptKey.TabIndex = 118;
            // 
            // lblDecryptKey
            // 
            this.lblDecryptKey.AutoSize = true;
            this.lblDecryptKey.Location = new System.Drawing.Point(21, 151);
            this.lblDecryptKey.Name = "lblDecryptKey";
            this.lblDecryptKey.Size = new System.Drawing.Size(209, 13);
            this.lblDecryptKey.TabIndex = 117;
            this.lblDecryptKey.Text = "Receiver Certificate (Receiver Private Key)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 116;
            this.label2.Text = "SFTP Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 115;
            this.label1.Text = "SFTP Username";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(21, 117);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(100, 20);
            this.password.TabIndex = 114;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(21, 74);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(100, 20);
            this.username.TabIndex = 113;
            // 
            // btnCheckNotification
            // 
            this.btnCheckNotification.Location = new System.Drawing.Point(21, 290);
            this.btnCheckNotification.Name = "btnCheckNotification";
            this.btnCheckNotification.Size = new System.Drawing.Size(114, 23);
            this.btnCheckNotification.TabIndex = 112;
            this.btnCheckNotification.Text = "Check Notification";
            this.btnCheckNotification.UseVisualStyleBackColor = true;
            this.btnCheckNotification.Click += new System.EventHandler(this.btnCheckNotification_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 488);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.FolderBrowserDialog dlgOpenFolder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox chkAutoSendSFTP;
        private System.Windows.Forms.CheckBox chkSendFolder;
        private System.Windows.Forms.CheckBox chkSignatureValidation;
        private System.Windows.Forms.CheckBox chkSchemaValidation;
        private System.Windows.Forms.RadioButton radCBC;
        private System.Windows.Forms.RadioButton radECB;
        private System.Windows.Forms.CheckBox chkM1O2;
        private System.Windows.Forms.Button cmdPopulateSettings;
        private System.Windows.Forms.Button btnBrowseHCTACert;
        private System.Windows.Forms.TextBox txtHCTACode;
        private System.Windows.Forms.Label lblHCTACode;
        private System.Windows.Forms.TextBox txtHCTACertPassword;
        private System.Windows.Forms.Label lblEncryptionHCTAPassword;
        private System.Windows.Forms.TextBox txtHCTACert;
        private System.Windows.Forms.Label lblHCTAKey;
        private System.Windows.Forms.Button btnSignXML;
        private System.Windows.Forms.TextBox txtKeyCertPassword;
        private System.Windows.Forms.Label lblKeyEncryptionCertPassword;
        private System.Windows.Forms.Button btnBrowseKeyCert;
        private System.Windows.Forms.TextBox txtKeyCert;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCertPass;
        private System.Windows.Forms.Label lblCertPass;
        private System.Windows.Forms.Button btnBrowseCert;
        private System.Windows.Forms.TextBox txtCert;
        private System.Windows.Forms.Label lblCert;
        private System.Windows.Forms.Button btnBrowseXml;
        private System.Windows.Forms.TextBox txtXmlFile;
        private System.Windows.Forms.Label lblLoadXML;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.TextBox txtNotificationFolder;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Button btnDecryptZip;
        private System.Windows.Forms.TextBox txtRecKeyPassword;
        private System.Windows.Forms.Label lblRecPass;
        private System.Windows.Forms.Button btnBrowseRecCert;
        private System.Windows.Forms.TextBox txtReceiverCert;
        private System.Windows.Forms.Label lblReceiverCert;
        private System.Windows.Forms.Button btnBrowseNotificationZip;
        private System.Windows.Forms.TextBox txtNotificationZip;
        private System.Windows.Forms.Label lblZipFile;
        private System.Windows.Forms.ComboBox cmbTaxYear;
        private System.Windows.Forms.Label lblTaxYear;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lblSchemaFolder;
        private System.Windows.Forms.Button btnSchemaFolder;
        private System.Windows.Forms.TextBox txtSchemaFolder;
        private System.Windows.Forms.Button btnSchemaFile;
        private System.Windows.Forms.TextBox txtSchemaFile;
        private System.Windows.Forms.Label lblSchemaFile;
        private System.Windows.Forms.Button btnCheckSchema;
        private System.Windows.Forms.Button btnBrowsePayload;
        private System.Windows.Forms.TextBox txtSignedPayloadFile;
        private System.Windows.Forms.Label lblSignedPayloadFile;
        private System.Windows.Forms.Button cmdCheckSignature;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ComboBox cmbSFTPServers;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDecryptPassword;
        private System.Windows.Forms.Label lblDecryptPassword;
        private System.Windows.Forms.Button btnBrowseDecKey;
        private System.Windows.Forms.TextBox txtDecryptKey;
        private System.Windows.Forms.Label lblDecryptKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Button btnCheckNotification;
        private System.Windows.Forms.Button btnBrowseDownloadFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDownFolder;
    }
}

