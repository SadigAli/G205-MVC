using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.DTOs.BasketDTO
{
    public class BasketGetDTO
    {
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public int Count { get; set; }
        public double ProductPrice { get; set; }
    }
}
