using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace CoreMVC.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(240)]
        public string? Email { get; set; }
        [MaxLength(100)]
        [MinLength(3)]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        public bool? Locked { get; set; }
        public List<RoleModel>? Roles { get; set; }
        public List<GroupModel>? GroupModels { get; set; }
        public UserSex? Sex { get; set; }
        [MaxLength(512)]
        [MinLength(10)]
        public string? Address { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }= DateTime.Now;
        [Required]
        public DateTime UpdateDate { get; set; } = DateTime.Now;

    }
    public enum UserSex
    {
        Male ,
        Female
    }
}
