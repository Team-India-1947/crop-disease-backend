using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace hackathon_template.Services; 

public class BlobService : IBlobService {
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string containerName = "htb-crop-images";
    private readonly string _connectionString;

    public BlobService(BlobServiceClient blobServiceClient, string connectionString) {
        _blobServiceClient = blobServiceClient;
        _connectionString = connectionString;
    }

    public async Task<string> StoreImage(IFormFile image) {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        
        string imageName = $"{Guid.NewGuid().ToString()}.jpg";
        using (var stream = image.OpenReadStream())
        {
            var binaryData = BinaryData.FromStream(stream);
            var response = await containerClient.UploadBlobAsync(imageName, binaryData);
        }
        string imageUrl = containerClient.Uri.AbsoluteUri;
        
        BlobSasBuilder sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerClient.Name,
            BlobName = containerClient.Name,
            Resource = "b", // "b" for blob
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(24)
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        // Append the SAS token to the blob URL
        string sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(containerClient.AccountName, _connectionString)).ToString();
        string protectedUrl = $"{imageUrl}?{sasToken}";
        return protectedUrl;
    }
}