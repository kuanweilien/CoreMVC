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
            HelloWorldModel model = new HelloWorldModel();
            model.CarouselModels = new List<CarouselModel>()
            {
                new CarouselModel(){PhotoName="Photo1",PhotoPath="/FileUploads/mountains-7ddde89.jpg" },
                new CarouselModel(){PhotoName="Photo2",PhotoPath="/FileUploads/p0b926m6.jpg" },
                new CarouselModel(){PhotoName="Photo3",PhotoPath="/FileUploads/somalaya-mountain.jpg" }
            };
            model.Dialog = new DialogModel()
            {
                Title = "This is Dialog Title",
                Content = "This is Dialog Content ! ",
                AllowBackDrop= false,
                AspControllerName= "HelloWorld"


            };

            return View(model);
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
