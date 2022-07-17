namespace CoreMVC.Areas.Identity.Models
{
    public class ResetPasswrodModel
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
