using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreMVC.Models.User
{
    public class UserGroupModel
    {
        public int Id { get; set; }
        [Required]
        public int UserId{ get; set; }
        [Required]
        public int GroupId { get; set; }
    }
}
