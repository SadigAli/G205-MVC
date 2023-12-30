using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.DTOs.CategoryDTO
{
    public class CategoryPostDTO
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
