using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.Entities
{
    public class AppUser : IdentityUser
    {
        [StringLength(20)]
        public string Firstname { get; set; } = string.Empty;
        [StringLength(20)]
        public string Lastname { get; set; } = string.Empty;
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        [StringLength(36)]
        public string ActivateToken { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
    }
}
