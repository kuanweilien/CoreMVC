using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoreMVC.Areas.Identity.Models
{
    public class AccountRoleModel:IdentityRole,ICloneable
    {
        [PersonalData]
        public DateTime CreationDate { get; set; }
        [PersonalData]
        public DateTime UpdateDate { get; set; }
        [NotMapped]
        public bool Checked { get; set; } = false;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
