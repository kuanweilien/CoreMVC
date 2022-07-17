namespace CoreMVC.Models
{
    public class AlertModel
    {
        public string Message { get; set; } = "";
        public BootStrap.Style Style { get; set; } = BootStrap.Style.Primary;
        public string CssClass 
        {
            get
            {
                switch (Style)
                {
                    case BootStrap.Style.Secondary: return "alert-secondary";
                    case BootStrap.Style.Success: return "alert-success";
                    case BootStrap.Style.Danger: return "alert-danger";
                    case BootStrap.Style.Warning: return "alert-warning";
                    case BootStrap.Style.Info: return "alert-info";
                    case BootStrap.Style.Light: return "alert-light";
                    case BootStrap.Style.Dark: return "alert-dark";
                    default: return "alert-primary";
                }
            }
        }
        

    }
}
