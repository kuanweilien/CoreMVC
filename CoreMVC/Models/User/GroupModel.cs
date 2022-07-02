using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreMVC.Models.User
{
    public class GroupModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        [MinLength(3)]
        public string GroupName { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime UpdateDate { get; set; } = DateTime.Now;

    }
}
