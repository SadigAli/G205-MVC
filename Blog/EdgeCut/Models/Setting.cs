using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdgeCut.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string AboutTitle { get; set; }
        [StringLength(100)]
        public string? AboutImage { get; set; }
        [StringLength(1000)]
        public string AboutDesc { get; set; } = string.Empty;
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Location { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
    }
}
