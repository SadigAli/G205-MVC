using System.ComponentModel.DataAnnotations;

namespace EdgeCut.Models
{
    public class Message : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(1000)]
        public string? Text { get; set; }=string.Empty;
        public bool? IsRead { get; set; } = false;
    }
}
