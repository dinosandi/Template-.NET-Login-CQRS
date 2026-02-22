
namespace ComponentManagement.Application.Interfaces
{
    public interface IFileService
    {
        Task<string?> SaveFileAsync(byte[] fileData, string fileName, CancellationToken cancellationToken = default);
        
        Task<bool> DeleteFileAsync(string relativePath);
    }
}
