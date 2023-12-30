using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.DTOs.ProductDTO
{
    public class ProductColorGetDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        //public List<string> Sizes { get; set; }
        public List<int> SizeIds { get; set; }
        public int TotalCount { get; set; }
        public List<ProductColorSizeDTO> Sizes { get; set; }
    }
}
