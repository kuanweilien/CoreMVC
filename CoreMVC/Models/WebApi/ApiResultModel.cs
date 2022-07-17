namespace CoreMVC.Models.WebApi
{
    public class ApiResultModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime ResponseTime { get; set; }= DateTime.Now;
        public T Data { get; set; }
    }
}
