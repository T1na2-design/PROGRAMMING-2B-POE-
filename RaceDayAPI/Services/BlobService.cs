using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace RaceDay.Services
{
    /// <summary>
    /// Azure Blob Storage implementation for file uploads.
    /// Requires Azure.Storage.Blobs NuGet package.
    /// Configuration keys: AzureBlobStorage:ConnectionString, AzureBlobStorage:ContainerName
    /// </summary>
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _configuration;

        public BlobService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null");

            try
            {
                // Get configuration values
                var connectionString = _configuration["AzureBlobStorage:ConnectionString"];
                var containerName = _configuration["AzureBlobStorage:ContainerName"];

                if (string.IsNullOrEmpty(connectionString))
                    throw new InvalidOperationException("AzureBlobStorage:ConnectionString is not configured");
                if (string.IsNullOrEmpty(containerName))
                    throw new InvalidOperationException("AzureBlobStorage:ContainerName is not configured");

                // Create blob service client from connection string
                var blobServiceClient = new BlobServiceClient(connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Create container if it doesn't exist
                await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                // Generate unique filename
                var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

                // Get blob client
                var blobClient = blobContainerClient.GetBlobClient(fileName);

                // Upload file
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                // Return the blob URL
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error uploading file to Azure Blob Storage: {ex.Message}", ex);
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is empty or null");

            try
            {
                // Get configuration values
                var connectionString = _configuration["AzureBlobStorage:ConnectionString"];
                var containerName = _configuration["AzureBlobStorage:ContainerName"];

                if (string.IsNullOrEmpty(connectionString))
                    throw new InvalidOperationException("AzureBlobStorage:ConnectionString is not configured");
                if (string.IsNullOrEmpty(containerName))
                    throw new InvalidOperationException("AzureBlobStorage:ContainerName is not configured");

                // Create blob service client
                var blobServiceClient = new BlobServiceClient(connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Get blob client and delete
                var blobClient = blobContainerClient.GetBlobClient(fileName);
                await blobClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting file from Azure Blob Storage: {ex.Message}", ex);
            }
        }
    }
}
