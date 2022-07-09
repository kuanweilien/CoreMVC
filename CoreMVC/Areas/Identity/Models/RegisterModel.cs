using System.ComponentModel.DataAnnotations;

namespace CoreMVC.Areas.Identity.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPasswrod { get; set; }
    }
}
