using System.Collections.Generic;

namespace WebApplication1.Models
{
    public interface IRepository
    {
        IEnumerable<Product> Products { get; }
        void AddProduct (Product p);
    }
}