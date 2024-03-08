using Microsoft.AspNetCore.Http;

namespace TalkBuddy.Service.Interfaces;

public interface IBlobService
{
    Task UploadFile(IFormFile fileModels);
    Task<Stream> GetFile(string fileName);
    Task<ICollection<string>> GetUrlAfterUploadedFile(List<IFormFile> files);
}