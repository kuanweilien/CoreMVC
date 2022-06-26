using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreMVC.Models
{
    public class PhotoModel
    {
        public int Id { get; set; }
        [NotMapped]
        public string CheckBoxId 
        {
            get
            {
                return "cb" + Id;
            }
        }
        [Required]
        [StringLength(100)]
        [MinLength(3)]
        public string? Title { get; set; }
        [StringLength(300)]
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public byte[]? Image { get; set; }
        [NotMapped]
        public string ImageUrl 
        { 
            get
            {
                return string.Format("data:image/jpg;base64,{0}", ImageBase64);
            } 
        }
        [NotMapped]
        public string ImageBase64
        {
            get
            {
                if (Image != null)
                    return Convert.ToBase64String(Image);
                if(!string.IsNullOrEmpty( imageBase64))
                    return imageBase64;
                return "";
            }
            set
            {
                imageBase64 = value;
            }
        }
        private string imageBase64;
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public string? FilePathSrc
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    return "/FileUploads/" + FileName;
                }
                return "";
            }
        }
    }
}
