using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.Entities
{
    public class Product : BaseEntity
    {
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Slug { get; set; }
        public double SalePrice { get; set; }
        public double Discount { get; set; } = 0;
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductColor> ProductColors { get; set; }

        public double Price()
        {
            return SalePrice - Discount;
        }
    }
}
