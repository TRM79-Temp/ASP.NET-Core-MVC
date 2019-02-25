using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IRepository Repository { get; set; } = SimpleRepository.SharedRepository;

        public IActionResult Index () => View (Repository.Products);

        [HttpGet]
        public IActionResult AddProduct () => View (new Product ());

        [HttpPost]
        public IActionResult AddProduct (Product p)
        {
            Repository.AddProduct (p);
            return RedirectToAction ("Index");
        }
    }
}