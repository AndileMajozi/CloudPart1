using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace EventEase.Services
{
    public class BlobStorageService
    {
        private const string ContainerName = "venue-images";
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureStorage");
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadVenueImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return string.Empty;
            }

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
            if (!allowedTypes.Contains(imageFile.ContentType.ToLower()))
            {
                throw new InvalidOperationException("Only image files are allowed. Please upload JPG, PNG, WEBP or GIF.");
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var extension = Path.GetExtension(imageFile.FileName);
            var safeFileName = $"{Guid.NewGuid()}{extension}";
            var blobClient = containerClient.GetBlobClient(safeFileName);

            await using var stream = imageFile.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = imageFile.ContentType });

            return blobClient.Uri.ToString();
        }
    }
}
