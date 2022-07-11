namespace CoreMVC.Models
{
    public class HelloWorldModel
    {
        public string Name { get; set; }
        public IEnumerable<CarouselModel> CarouselModels { get; set; }
        public DialogModel Dialog { get;set; }
    }
}
