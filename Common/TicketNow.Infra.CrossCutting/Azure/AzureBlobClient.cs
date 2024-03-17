using Azure.Storage.Blobs;

namespace TicketNow.Infra.CrossCutting.Azure;

public class AzureBlobClient
{
    private readonly string _connectionString;

    public AzureBlobClient(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task Upload(Stream content, string blobName, string containerName)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(content, true);
    }
}