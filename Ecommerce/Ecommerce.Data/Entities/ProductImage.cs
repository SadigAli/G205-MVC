using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.Entities
{
    public class ProductImage : BaseEntity
    {
        [StringLength(100)]
        public string Image { get; set; }
        public ImageStatus Status { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

    public enum ImageStatus
    {
        Poster,
        Hover,
        Normal
    }
}
