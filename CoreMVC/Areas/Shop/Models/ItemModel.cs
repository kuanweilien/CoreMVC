namespace CoreMVC.Areas.Shop.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[]? Image { get; set; }
        public DateTime CreationDate { get; set; }= DateTime.Now;
    }
}
