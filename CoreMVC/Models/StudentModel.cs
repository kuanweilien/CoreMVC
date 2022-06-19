using System.ComponentModel.DataAnnotations;

namespace CoreMVC.Models
{
    public class StudentModel
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? StudentName { get; set; }

        [Range(1, 100)]
        [Required]
        public int? StudentNumber { get; set; }

        [Range(0, 1)]
        [Required]
        public int? Sex{ get; set; }
    }
}
