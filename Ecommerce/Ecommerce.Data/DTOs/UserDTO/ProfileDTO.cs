using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.DTOs.UserDTO
{
    public class ProfileDTO
    {
        [StringLength(100)]
        public string? FirstName { get; set; }
        [StringLength(100)]
        public string? LastName { get; set; }
        [StringLength(100),EmailAddress]
        public string Email { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
        [StringLength(100)]
        public string? Address { get; set; }
        [StringLength(20)]
        public string? Phone { get; set; }
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

    }
}
