using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.DTOs.ColorDTO
{
    public class ColorPostDTO
    {
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
