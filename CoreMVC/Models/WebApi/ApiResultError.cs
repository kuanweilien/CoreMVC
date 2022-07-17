namespace CoreMVC.Models.WebApi
{
    public class ApiResultError<T>:ApiResultModel<T>
    {
        public ApiResultError(string message)
        {
            Success = false;
            Message = message;
        }


    }
}
