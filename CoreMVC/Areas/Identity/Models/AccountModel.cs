using Microsoft.AspNetCore.Identity;
namespace CoreMVC.Areas.Identity.Models
{
    public class AccountModel:IdentityUser
    {
        [PersonalData]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [PersonalData]
        public DateTime UpdateDate { get; set; } = DateTime.Now;

    }
}
