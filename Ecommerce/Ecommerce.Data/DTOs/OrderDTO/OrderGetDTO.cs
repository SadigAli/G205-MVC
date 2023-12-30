using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.DTOs.OrderDTO
{
    public class OrderGetDTO
    {
        [StringLength(100)]
        public string? Address { get; set; }
        [StringLength(20)]
        public string? Phone { get; set; }
        [StringLength(20)]
        public string ZipCode { get; set; }
    }
}
