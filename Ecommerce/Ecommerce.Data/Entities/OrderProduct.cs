using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.Entities
{
    public class OrderProduct : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductColorSizeId { get; set; }
        public Order Order { get; set; }
        public ProductColorSize ProductColorSize { get; set; }
        public int Count { get; set; }
        public double ProductPrice { get; set; }
    }
}
