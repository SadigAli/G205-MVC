using System.ComponentModel.DataAnnotations;

namespace EdgeCut.ViewModels
{
    public class LoginVM
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
