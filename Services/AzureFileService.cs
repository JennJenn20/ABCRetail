using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.AspNetCore.Http;

namespace ABCRetailWebApplication.Services
{
    public class AzureFileService
    {
        private readonly ShareClient _shareClient;

        public AzureFileService(string connectionString, string shareName)
        {
            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists(); // Create the file share if it doesn't exist
        }

        public async Task UploadFile(string fileName, IFormFile file)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadAsync(stream);
            }
        }

        public async Task<List<string>> ListFiles()
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var files = new List<string>();

            await foreach (ShareFileItem fileItem in directoryClient.GetFilesAndDirectoriesAsync())
            {
                files.Add(fileItem.Name);
            }

            return files;
        }

        public async Task DeleteFile(string fileName)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.DeleteIfExistsAsync();
        }

        public async Task<bool> FileExists(string fileName)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);
            return await fileClient.ExistsAsync();
        }
    }
}
