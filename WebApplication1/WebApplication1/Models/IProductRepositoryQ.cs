using System.Linq;
namespace WebApplication1.Models
{
    public interface IProductRepositoryQ
    {
        IQueryable<Product> Products { get; }
    }
}