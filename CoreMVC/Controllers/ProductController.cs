using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreMVC.Models;
using System.Security;

namespace CoreMVC.Controllers
{
    public class ProductController : Controller
    {
        ProductModel[] products = new ProductModel[]
        {
            new ProductModel { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new ProductModel { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new ProductModel { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };

        public IEnumerable<ProductModel> GetAllProducts()
        {
            return products;
        }
        public IActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
