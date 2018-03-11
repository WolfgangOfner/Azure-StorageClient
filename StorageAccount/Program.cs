using System;
using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace StorageAccount
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create a CloudFileClient object for credentialed access to Azure Files.
            var fileClient = storageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            var share = fileClient.GetShareReference("YourFileShareName");

            if (share.Exists())
            {
                // Get a reference to the root directory for the share.
                var rootDir = share.GetRootDirectoryReference();

                var file = rootDir.GetFileReference("FileNameToDownload");

                PrintFileToConsole(file);

                UploadFile(share, rootDir);

                DownloadFile(file);
            }
        }

        private static void PrintFileToConsole(CloudFile file)
        {
            if (file.Exists())
            {
                Console.WriteLine(file.DownloadTextAsync().Result);
            }
        }

        private static void UploadFile(CloudFileShare share, CloudFileDirectory rootDir)
        {
            var sourceFile = share.GetRootDirectoryReference().GetFileReference("FileNameToUpload");
            sourceFile.UploadText("This content was uploaded from C#. \n yay!!!");
        }

        private static void DownloadFile(CloudFile file)
        {
            file.DownloadToFile("PathAndFileNameOnYourComputer", FileMode.CreateNew);
        }
    }
}