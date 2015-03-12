using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace WindowsFormsApplication1
{
    class ZipManager
    {
        /// <summary>
        /// Creates a Zip file in Memory
        /// </summary>
        /// <param name="fileName">Name of the file to be zipped</param>
        /// <param name="file">Content of the file</param>
        /// <returns>Byte Array with the Zip file</returns>
        public static byte[] ZipContent(string fileName, byte[] file)
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            files.Add(fileName, file);
            return ZipContent(files);
        }

        /// <summary>
        /// Creates a Zip file in Memory from a list of files
        /// </summary>
        /// <param name="files">Dictionary with the file names and file content</param>
        /// <returns>Byte Array with the Zip file (including all files)</returns>
        public static byte[] ZipContent(Dictionary<string, byte[]> files)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                // create a new zip archiv in the memory stream
                using (ZipArchive memZipFile = new ZipArchive(memStream, ZipArchiveMode.Create, true))
                {
                    foreach (var entry in files)
                    {
                        // create a new entry in the zip file
                        var tmpFile = memZipFile.CreateEntry(entry.Key);
                        // write the content to the new file
                        using (Stream fileContent = tmpFile.Open())
                        {
                            using (BinaryWriter writer = new BinaryWriter(fileContent))
                            {
                                // write the content of the file
                                writer.Write(entry.Value);
                            }
                        }
                    }
                }

                return memStream.ToArray();
            }
        }


        public static void CreateArchive(string inputFileName, string outputFileName)
        {
            using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
            {
                using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    string entryName = Path.GetFileName(inputFileName);
                    archive.CreateEntryFromFile(inputFileName, entryName, CompressionLevel.Optimal);
                }
            }
        }

        public static void UpdateArchive(string inputFileName, string outputFileName)
        {
            using (FileStream fs = new FileStream(outputFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Update))
                {
                    string entryName = Path.GetFileName(inputFileName);
                    archive.CreateEntryFromFile(inputFileName, entryName, CompressionLevel.Optimal);
                }
            }
        }

        public static string ExtractArchive(string inputFileName, string outputFolder)
        {
            string zipFileName = Path.GetFileNameWithoutExtension(inputFileName);
            string zipFolderPath = outputFolder + "\\" + zipFileName;

            using (ZipArchive archive = ZipFile.Open(inputFileName, ZipArchiveMode.Update))
            {

                archive.ExtractToDirectory(zipFolderPath);
            }

            return zipFolderPath;

        }

        public static void ExtractArchive(string inputFileName, string outputFolder, bool noPath)
        {

            string zipFileName = Path.GetFileName(inputFileName);
            string zipFolderPath = inputFileName.Replace(zipFileName, "");

            using (ZipArchive archive = ZipFile.Open(inputFileName, ZipArchiveMode.Update))
            {

                archive.ExtractToDirectory(zipFolderPath);
            }



        }
    }




}
