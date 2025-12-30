using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.AttachmentServices
{
    public class AttachmentServices : IAttachmentServices
    {
        private readonly List<string> _allowedExtensions = [".jpg", ".png", ".jpeg"];
        private const int MaxSize = 2 * 1024 * 1024; // 2MB

        public async Task<string?> UploadedAsync(IFormFile file, string folderName)
        {
            // 1. Validation: File existence and Size
            if (file == null || file.Length == 0 || file.Length > MaxSize)
                return null;

            // 2. Validation: Extension
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(extension))
                return null;

            // 3. Prepare Folder Path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 4. Generate Unique Name
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);

            // 5. Save File Asynchronously
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public async Task<bool> DeleteAsync(string folderName, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;

            // Construct the path to the file inside wwwroot
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName, fileName);

            if (File.Exists(filePath))
            {
                // File.Delete is generally fast, but we wrap it for a consistent async API
                return await Task.Run(() =>
                {
                    File.Delete(filePath);
                    return true;
                });
            }

            return false;
        }
    }
}