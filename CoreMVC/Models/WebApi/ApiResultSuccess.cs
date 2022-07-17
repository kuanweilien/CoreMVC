namespace CoreMVC.Models.WebApi
{
    public class ApiResultSuccess<T>:ApiResultModel<T>
    {
        public ApiResultSuccess(T data)
        {
            base.Success = true;
            base.Message = data == null ? "NoDataFound" : "";
            base.Data = data;
        }
    }
}
