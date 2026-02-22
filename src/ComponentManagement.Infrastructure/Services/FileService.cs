using ComponentManagement.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ComponentManagement.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment _env;
        public FileService(IHostEnvironment env) => _env = env;

        public async Task<string?> SaveFileAsync(byte[] fileData, string fileName, CancellationToken cancellationToken = default)
        {
            if (fileData == null || fileData.Length == 0)
                return null;

            // simpan di folder wwwroot/uploads
            var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueName);

            await File.WriteAllBytesAsync(filePath, fileData, cancellationToken);

            // return path relatif (untuk akses dari browser)
            return $"/uploads/{uniqueName}";
        }
        public async Task<bool> DeleteFileAsync(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return false;

            // Pastikan path tidak berisi karakter berbahaya (security reason)
            relativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString())
                                       .TrimStart(Path.DirectorySeparatorChar);

            var fullPath = Path.Combine(_env.ContentRootPath, "wwwroot", relativePath);

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
                return true;
            }

            return false;
        }
        

    }
}
