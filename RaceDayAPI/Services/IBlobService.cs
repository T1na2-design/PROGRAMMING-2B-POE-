using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RaceDay.Services
{
    public interface IBlobService
    {
        /// <summary>
        /// Uploads a file to blob storage and returns the URL.
        /// </summary>
        Task<string> UploadFileAsync(IFormFile file);

        /// <summary>
        /// Deletes a file from blob storage.
        /// </summary>
        Task DeleteFileAsync(string fileName);
    }
}
