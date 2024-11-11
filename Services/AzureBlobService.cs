using Azure.Storage.Blobs;

namespace ABCRetailWebApplication.Services
{
    public class AzureBlobService
    {
        private readonly BlobContainerClient _blobContainerClient;
        public AzureBlobService(string connectionString, string containerName)
        {
            _blobContainerClient = new BlobContainerClient(connectionString, containerName);
            _blobContainerClient.CreateIfNotExists(); // Create the container if it doesn't exist
        }

        public async Task UploadBlob(string blobName, IFormFile file)
        {
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream);
            }
        }

        public Uri GetBlobUri(string blobName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
            return blobClient.Uri;
        }

        public async Task<List<string>> ListBlobs()
        {
            var blobs = new List<string>();
            await foreach (var blobItem in _blobContainerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }
            return blobs;
        }

        public async Task DeleteBlob(string blobName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<bool> BlobExists(string blobName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            return await blobClient.ExistsAsync();
        }
    }
}
