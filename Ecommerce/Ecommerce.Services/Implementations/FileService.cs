using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Implementations
{
    public class FileService : IFileService
    {
        public void FileDelete(string wwwroot,string folder, string file)
        {
            string fullPath = Path.Combine(wwwroot,"uploads",folder,file);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task<(bool, string)> FileUpload(IFormFile file, string wwwroot, string folder)
        {
            if (file.Length / 1024 > 200) return (false, "File's length must be greater than 200kb");
            if (!file.ContentType.Contains("image")) return (false, "File must be an image");
            string folderPath = Path.Combine(wwwroot, "uploads", folder);
            if(!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string fileName = Guid.NewGuid() + file.FileName;
            string fullPath = Path.Combine(folderPath, fileName);
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return (true,fileName);
        }
    }
}
