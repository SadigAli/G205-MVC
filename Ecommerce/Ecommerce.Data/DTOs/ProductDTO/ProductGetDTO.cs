using Ecommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.DTOs.ProductDTO
{
    public class ProductGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public double SalePrice { get; set; }
        public double Discount { get; set; }
        public string Category { get; set; }
        public string PosterImage { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Images { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<ProductColorGetDTO> ProductColors { get; set; }
    }
}
