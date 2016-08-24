using System.IO;
using System.IO.Compression;

namespace WindowsFormsApplication1
{
    class ZipManager
    {

        /// <summary>
        /// This will create a zip archive 
        /// </summary>
        /// <param name="inputFileName">The path to the file that will be compressed</param>
        /// <param name="outputFileName">The name of the zip file that will be created</param>
        /// <returns></returns>
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

        /// <summary>
        /// This will add a file to an archive 
        /// </summary>
        /// <param name="inputFileName">The path to the file that will be compressed</param>
        /// <param name="outputFileName">The name of the zip file that will be modified</param>
        /// <returns></returns>
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

        /// <summary>
        /// This will decompress a zip file
        /// </summary>
        /// <param name="inputFileName">The path to the file that will be decompressed</param>
        /// <param name="outputFolder">The path of the folder that will contain the decompressed files</param>
        /// <returns>The path of the decompressed folder</returns>
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

        /// <summary>
        /// This will decompress a zip file, but is used when there is no need to return the decompressed folder name
        /// </summary>
        /// <param name="inputFileName">The path to the file that will be decompressed</param>
        /// <param name="outputFolder">The path of the folder that will contain the decompressed files</param>
        /// <returns></returns>
        public static void ExtractArchive(string inputFileName, string outputFolder, bool noPath)
        {

            string zipFileName = Path.GetFileName(inputFileName);
            string zipFolderPath = inputFileName.Replace(zipFileName, "");

            using (ZipArchive archive = ZipFile.Open(inputFileName, ZipArchiveMode.Update))
            {

                archive.ExtractToDirectory(outputFolder);
            }

        }
    }




}
