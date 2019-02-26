using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepositoryQ repository;

        public ProductController(IProductRepositoryQ repo)
        {
            repository = repo;
        }

        public ViewResult List() => View(repository.Products);
    }
}