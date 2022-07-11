namespace CoreMVC.Models
{
    public class DialogModel
    {
        public string DialogId { get; set; } = "Dialog-" + Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Content { get; set; }
        public string PartialName{ get; set; }
        public object PartialModel { get; set; }
        public string DialogButtonName { get; set; } = "Open";
        public bool ShowCloseButton { get; set; } = false;
        public bool ShowSubmitButton { get; set; } = true;
        public string SubmitButtonName { get; set; } = "Submit";
        public bool AllowBackDrop { get; set; } = true;
        public string AspAreaName { get; set; } = "";
        public string AspControllerName { get; set; } = "";
        public string AspActionName { get; set; } = "";
        public DialogSize Size { get; set; } = DialogSize.Middle;
        public string SizeClass
        {
            get
            {
                switch (Size)
                {
                    case DialogSize.FullScreen:
                        return "modal-fullscreen";
                    case DialogSize.XL:
                        return "modal-xl";
                    case DialogSize.Large:
                        return "modal-lg";
                    case DialogSize.Small:
                        return "modal-sm";
                    default:
                        return "";
                }
            }
        }
        public enum DialogSize {FullScreen,XL,Large,Middle,Small}

    }
}
