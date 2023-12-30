using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.Entities
{
    public class Category : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Slug { get; set; }
        [StringLength(100)]
        public string? Image { get; set; } = string.Empty;
    }
}
