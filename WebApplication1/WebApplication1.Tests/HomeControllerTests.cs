using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using WebApplication1.Controllers;
using WebApplication1.Models;

namespace WebApplication1.Tests
{
    public class HomeControllerTests
    {
        class ModelCompleteFakeRepository : IRepository
        {
            public IEnumerable<Product> Products { get; } = new Product[]
            {
                new Product { Name = "P1", Price = 275M },
                new Product { Name = "P2", Price = 48.95M },
                new Product { Name = "P3", Price = 19.50M },
                new Product { Name = "P3", Price = 34.95M }
            };

            public void AddProduct (Product p)
            {
                // do nothing - not required for test
            }
        }

        [Fact]
        public void IndexActionModelIsComplete ()
        {
            // Arrange
            var controller = new HomeController ();
            controller.Repository = new ModelCompleteFakeRepository ();
            // Act
            var model = (controller.Index () as ViewResult)?.ViewData.Model
            as IEnumerable<Product>;
            // Assert
            // Assert.Equal (SimpleRepository.SharedRepository.Products, model,
            //     Comparer.Get<Product> ((p1, p2) => p1.Name == p2.Name &&
            //         p1.Price == p2.Price));
            Assert.Equal (controller.Repository.Products, model,
                Comparer.Get<Product> ((p1, p2) => p1.Name == p2.Name &&
                    p1.Price == p2.Price));
        }

        class ModelCompleteFakeRepositoryPricesUnder50 : IRepository
        {
            public IEnumerable<Product> Products { get; } = new Product[]
            {
                new Product { Name = "P1", Price = 5M },
                new Product { Name = "P2", Price = 48.95M },
                new Product { Name = "P3", Price = 19.50M },
                new Product { Name = "P3", Price = 34.95M }
            };
            public void AddProduct (Product p)
            {
                // do nothing - not required for test
            }
        }

        [Fact]
        public void IndexActionModelIsCompletePricesUnder50 ()
        {
            // Arrange
            var controller = new HomeController ();
            controller.Repository = new ModelCompleteFakeRepositoryPricesUnder50 ();
            // Act
            var model = (controller.Index () as ViewResult)?.ViewData.Model
            as IEnumerable<Product>;
            // Assert
            Assert.Equal (controller.Repository.Products, model,
                Comparer.Get<Product> ((p1, p2) => p1.Name == p2.Name &&
                    p1.Price == p2.Price));
        }
    }
}