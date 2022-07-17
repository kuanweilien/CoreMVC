using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreMVC.Data;
using CoreMVC.Areas.Shop.Models;
using CoreMVC.Models.WebApi;
using System.Transactions;

namespace CoreMVC.Areas.Shop.Controllers
{
    [Route("api/shop/{action}/{id}")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly MariaDBContext _context;


        public ItemController(MariaDBContext context)
        {
            _context = context;
            
        }
        [HttpGet]
        //[ActionName("item")]
        public ApiResultModel<ItemModel> GetItemById(int id)
        {
            try
            {
                return new ApiResultSuccess<ItemModel>(_context.ItemModel.Where(x => x.Id == id).FirstOrDefault());
            }
            catch(Exception ex)
            {
                return new ApiResultError<ItemModel>(ex.Message);
            }
            
        }
        [HttpGet]
        //[Route("api/shop/{action}/all")]
        [ActionName("item")]
        public ApiResultModel<IEnumerable<ItemModel>> GetItems()
        {
            try
            {
                return new ApiResultSuccess<IEnumerable<ItemModel>>(_context.ItemModel);
            }
            catch (Exception ex)
            {
                return new ApiResultError<IEnumerable<ItemModel>>(ex.Message);
            }
        }
        [HttpPost]
        //[ActionName("item")]
        public ApiResultModel<string> CreateItem(ItemModel item)
        {
            try
            {
                int id;
                using (TransactionScope scope = new TransactionScope())
                {
                    _context.Add(item);
                    id = _context.SaveChanges();
                    scope.Complete();
                }
                return new ApiResultSuccess<string>(id.ToString());
            }
            catch (Exception ex)
            {
                return new ApiResultError<string>(ex.Message);
            }
        }

    }
}

