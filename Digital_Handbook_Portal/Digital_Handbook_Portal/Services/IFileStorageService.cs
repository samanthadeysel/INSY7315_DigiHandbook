using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Digital_Handbook_Portal.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        void DeleteFile(string relativePath);
    }
}