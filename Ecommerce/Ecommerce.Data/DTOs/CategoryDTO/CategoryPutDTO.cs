using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.DTOs.CategoryDTO
{
    public class CategoryPutDTO
    {
        public int Id { get; set; }
        public string? Slug { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public IFormFile? File { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
