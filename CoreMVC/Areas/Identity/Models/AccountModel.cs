using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreMVC.Models;
namespace CoreMVC.Areas.Identity.Models
{
    public class AccountModel:IdentityUser
    {
        [PersonalData]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [PersonalData]
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        [NotMapped]
        public bool Checked { get; set; }=false;

        #region--Role--
        [NotMapped]
        public IEnumerable<AccountRoleModel> Roles { get; set; }
        [NotMapped]
        public DialogModel AssignRoleDialog { get; set; }
        #endregion
    }
}
