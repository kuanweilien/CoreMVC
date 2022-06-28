using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using CoreMVC.Models;
using CoreMVC.Data;

namespace CoreMVC.Controllers
{
    public class HelloWorldController : Controller
    {
        private readonly MariaDBContext _context;

        public HelloWorldController(MariaDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /HelloWorld/Welcome/ 

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
        public IActionResult GetMovies()
        {
            var result = _context.MovieModel;

            return Json(result);
        }
    }
}
