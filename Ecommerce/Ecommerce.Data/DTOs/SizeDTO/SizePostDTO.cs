using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.DTOs.SizeDTO
{
    public class SizePostDTO
    {
        [StringLength(5)]
        public string Name { get; set; }
    }
}
