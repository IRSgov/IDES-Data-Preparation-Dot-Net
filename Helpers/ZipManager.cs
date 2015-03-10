using System.IO;
using System.IO.Compression;

namespace WindowsFormsApplication1
{
    class ZipManager
    {
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
