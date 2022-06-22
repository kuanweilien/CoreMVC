using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreMVC.Models
{
    public class PhotoModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [MinLength(3)]
        public string? Title { get; set; }
        [StringLength(300)]
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public byte[]? Image { get; set; }
        
    }
}
