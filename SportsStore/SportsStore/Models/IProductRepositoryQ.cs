using System.Linq;
namespace WebApplication1.Models
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
    }
}