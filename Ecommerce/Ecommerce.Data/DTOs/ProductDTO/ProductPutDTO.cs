using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.DTOs.ProductDTO
{
    public class ProductPutDTO
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public double SalePrice { get; set; }
        public double? Discount { get; set; }
        public string? Category { get; set; }
        public int CategoryId { get; set; }
        public string? PosterImage { get; set; }
        public List<string>? Colors { get; set; }
        public List<string>? Images { get; set; }
        public List<int>? ColorIds { get; set; }
        public IFormFile? PosterFile { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
