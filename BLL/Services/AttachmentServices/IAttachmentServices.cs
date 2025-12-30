using Microsoft.AspNetCore.Http;


namespace BLL.Services.AttachmentServices
{
    public interface IAttachmentServices
    {
        // Changed return type to Task<string?>
        Task<string?> UploadedAsync(IFormFile file, string folderName);

        // Changed return type to Task<bool>
        Task<bool> DeleteAsync(string folderName, string fileName);
    }
}
