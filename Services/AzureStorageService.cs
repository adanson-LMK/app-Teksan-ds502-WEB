using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace app_Teksan_ds502.Services
{
    public class AzureStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public AzureStorageService(IConfiguration config)
        {
            _blobServiceClient = new BlobServiceClient(config["AzureStorage:ConnectionString"]);
            _containerName = config["AzureStorage:ContainerName"];
        }

        public async Task<string> UploadFileAsync(string filePath, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(filePath, overwrite: true);

            return blobClient.Uri.ToString(); // ✅ Retorna la URL del archivo
        }
    }
}
