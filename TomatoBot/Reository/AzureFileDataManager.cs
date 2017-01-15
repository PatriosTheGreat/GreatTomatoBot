using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TomatoBot.Reository
{
    public sealed class AzureFileDataManager<TData>
    {
        public AzureFileDataManager(string fileName)
        {
            _fileName = fileName;
        }

        public void SaveData(TData[] data)
        {
            lock (_lockRoot)
            {
                using (var memoryStream = new MemoryStream())
                {
                    Serializer.WriteObject(memoryStream, data);

                    memoryStream.Flush();
                    memoryStream.Position = 0;
                    GetBotDataBlock().UploadFromStream(memoryStream);
                }
            }
        }

        public IEnumerable<TData> LoadData()
        {
            lock (_lockRoot)
            {
                using (var memoryStream = new MemoryStream())
                {
                    GetBotDataBlock().DownloadToStream(memoryStream);
                    if (memoryStream.Length == 0)
                    {
                        return new TData[0];
                    }

                    memoryStream.Position = 0;
                    return (TData[])Serializer.ReadObject(memoryStream);
                }
            }
        }

        private ICloudBlob GetBotDataBlock()
        {
            var blobAcc = CloudConfigurationManager.GetSetting("BlobAccount");
            var blobKey = CloudConfigurationManager.GetSetting("BlobKey");

            var storageCredentials = new StorageCredentials(blobAcc, blobKey);
            var account = new CloudStorageAccount(storageCredentials, true);

            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ContainerName);
            return container.GetBlobReferenceFromServer(_fileName);
        }

        private readonly object _lockRoot = new object();
        private const string ContainerName = "tomatobotdata";
        private readonly string _fileName;
        private static readonly DataContractSerializer Serializer = new DataContractSerializer(typeof(TData[]));
    }
}