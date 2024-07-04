using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace WebAPI.Services.Interface
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions);
        void DeleteFile(string fileNameWithExtension);
    }
}
