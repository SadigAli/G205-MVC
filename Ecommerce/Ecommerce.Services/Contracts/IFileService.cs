using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Contracts
{
    public interface IFileService
    {
        public Task<(bool, string)> FileUpload(IFormFile file, string wwwroot, string folder);
        public void FileDelete(string wwwroot,string folder, string file);
    }
}
