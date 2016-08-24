using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSCP;

namespace WindowsFormsApplication1
{
    class SFTPManager
    {

        /// <summary>
        /// This will create an SFTP Session that can be used to open a connection 
        /// </summary>
        /// <param name="server">The URL of the SFTP server</param>
        /// <param name="username">The username for the account that will be used on the SFTP server</param>
        /// <param name="password">The password for the account that will be used on the SFTP server</param>
        /// <returns>The sessionOptions object</returns>
        public static SessionOptions CreateSFTPSession(string server, string username, string password)
        {

            if (string.IsNullOrWhiteSpace(username))
            {
                // username validation
                throw new Exception("The SFTP Username must be set!");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                // password validation
                throw new Exception("The SFTP Password must be set!");
            }

            if (string.IsNullOrWhiteSpace(server))
            {
                // server validation
                throw new Exception("The SFTP Server must be set!");
            }

            var sftpServer = server.Split(':')[1].Trim();
            var serverType = server.Split(':')[0].Trim();
            string fingerprint = "";
            // not currently used - SAT SshHostKeyFingerprint = "ssh-rsa 2048 64:25:44:96:0d:db:cc:10:3b:80:f3:2d:0e:24:bf:75"
            if (serverType == "PRODUCTION") {
                fingerprint = "ssh-rsa 2048 9c:6a:de:70:54:ba:a6:dc:03:72:6f:73:dd:a2:d9:13";
            }
            else
            {
                fingerprint = "ssh-rsa 2048 ca:8f:be:b4:c6:92:58:ef:3b:b2:d1:fd:63:c0:8e:a5";
            }
            // Setup session options
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = sftpServer,
                UserName = username,
                Password = password,
                PortNumber = 4022,
                SshHostKeyFingerprint = fingerprint
            };

            return sessionOptions;


        }

        /// <summary>
        /// This will connect to the Inbox/840 folder and download any notifications found there - if not previously downloaded based on our log file 
        /// </summary>
        /// <param name="sessionOptions">This holds the connection information such as server and account credentials</param>
        /// <param name="transmissions">This holds our log file information on transfers</param>
        /// <param name="downloadFolder">The folder that will hold the downloaded files</param>
        /// <returns></returns>
        public static void DownloadInbox(SessionOptions sessionOptions, Dictionary<string, string> transmissions, string downloadFolder)
        {

            string folderCheck = downloadFolder.Substring(downloadFolder.Length - 1, 1);
            if (folderCheck != "\\")
            {
                downloadFolder = downloadFolder + "\\";
            }

            using (Session session = new Session())
            {
                // Connect
                session.Open(sessionOptions);

                RemoteDirectoryInfo directory = session.ListDirectory("/Inbox/840/");
                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;


                foreach (RemoteFileInfo fileInfo in directory.Files)
                {
                    //if the length is greater than 2, there is a file for download
                    //we will check our log file to see if this one has already been downloaded and processed
                    if (fileInfo.Name.Length > 2)
                    {

                        //check to see if this is in our transmission log file 
                        if (transmissions.ContainsKey(fileInfo.Name) == false)
                        {
                            //we will download the file and try to match it with 
                            TransferOperationResult transferResult;
                            transferResult = session.GetFiles("/Inbox/840/" + fileInfo.Name, downloadFolder + fileInfo.Name, false, transferOptions);

                            // Throw on any error
                            transferResult.Check();

                            // SFTP options can be logged here if needed
                            //foreach (TransferEventArgs transfer in transferResult.Transfers)
                            //{
                            //    Console.WriteLine("Download of {0} succeeded", transfer.FileName);
                            //}
                        }

                    }
                    
                }
                session.Close();
            }
        }

        /// <summary>
        /// This will connect to the Outbox/840 folder and upload a data packet 
        /// </summary>
        /// <param name="sessionOptions">This holds the connection information such as server and account credentials</param>
        /// <param name="fileName">The path to the file being uploaded</param>
        /// <returns></returns>
        public static string UploadFile(SessionOptions sessionOptions, string filename)
        {
            
            using (Session session = new Session())
            {
                // Connect
                session.Open(sessionOptions);

                // Upload files
                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;
                //transferOptions.ResumeSupport.State = false;

                TransferOperationResult transferResult;
                transferResult = session.PutFiles(@filename, "/Outbox/840/", false, transferOptions);

                // Throw on any error
                transferResult.Check();

                // Print results
                string thisResult = "";
                foreach (TransferEventArgs transfer in transferResult.Transfers)
                {
                    //SFTP options can be logged here, if needed
                    //Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                    thisResult = "Upload of " + transfer.FileName + " succeeded";
                }
                session.Close();
                
                return thisResult;
            }
        }

    }
}
