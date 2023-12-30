using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.Entities
{
    public class Size : BaseEntity
    {
        [StringLength(5)]
        public string Name { get; set; }
    }
}
