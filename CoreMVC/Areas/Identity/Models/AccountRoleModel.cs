using Microsoft.AspNetCore.Identity;
namespace CoreMVC.Areas.Identity.Models
{
    public class AccountRoleModel:IdentityRole
    {
        [PersonalData]
        public DateTime CreationDate { get; set; }
        [PersonalData]
        public DateTime UpdateDate { get; set; }
    }
}
