using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TalkBuddy.Common.Helpers;
using TalkBuddy.Service.Interfaces;
using TalkBuddy.Service.Settings;

namespace TalkBuddy.Service.Implementations;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly AzureSettings _azureSettings;

    public BlobService(IConfiguration configuration)
    {
        _azureSettings = configuration.GetSection(nameof(AzureSettings)).Get<AzureSettings>() ?? throw new Exception("invalid azure settings configuration.");
        _blobServiceClient = new BlobServiceClient(_azureSettings.AzureBlobStorage);
    }
    
    public async Task UploadFile(IFormFile fileModels)
    {
        var containerInstance = _blobServiceClient.GetBlobContainerClient(_azureSettings.BlobContainer);
        var blobInstance = containerInstance.GetBlobClient(fileModels.FileName.TrimSpaceString());
        await blobInstance.UploadAsync(fileModels.OpenReadStream());
    }

    public async Task<Stream> GetFile(string fileName)
    {
        var containerInstance = _blobServiceClient.GetBlobContainerClient(_azureSettings.BlobContainer);
        var blobInstance = containerInstance.GetBlobClient(fileName);
        var downloadResult = await blobInstance.DownloadAsync();
        return downloadResult.Value.Content;
    }

    public async Task<ICollection<string>> GetUrlAfterUploadedFile(List<IFormFile> files)
    {
        var listUrl = new List<string>();
        var containerInstance = _blobServiceClient.GetBlobContainerClient(_azureSettings.BlobContainer);
        
        foreach (var file in files)
        {
            var blobInstance = containerInstance.GetBlobClient(StringInterpolationHelper.GenerateUniqueFileName(file.FileName,10));
            await blobInstance.UploadAsync(file.OpenReadStream());
            listUrl.Add(blobInstance.Uri.ToString());
        }
        return listUrl;
    }
}
