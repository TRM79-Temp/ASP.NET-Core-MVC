using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests
{
    public class HomeControllerTests
    {
        class ModelCompleteFakeRepository : IProductRepository
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

        class ModelCompleteFakeRepositoryPricesUnder50 : IProductRepository
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

        //

        class ModelCompleteFakeRepository2 : IProductRepository
        {
            public IEnumerable<Product> Products { get; set; }
            public void AddProduct (Product p)
            {
                // do nothing - not required for test
            }
        }

        [Theory]
        [ClassData (typeof (ProductTestData))]
        public void IndexActionModelIsComplete2 (Product[] products)
        {
            // Arrange
            var controller = new HomeController ();
            controller.Repository = new ModelCompleteFakeRepository2
            {
                Products = products
            };
            // Act
            var model = (controller.Index () as ViewResult)?.ViewData.Model
            as IEnumerable<Product>;
            // Assert
            Assert.Equal (controller.Repository.Products, model,
                Comparer.Get<Product> ((p1, p2) => p1.Name == p2.Name &&
                    p1.Price == p2.Price));
        }

        //

        class PropertyOnceFakeRepository : IProductRepository
        {
            public int PropertyCounter { get; set; } = 0;
            public IEnumerable<Product> Products
            {
                get
                {
                    PropertyCounter++;
                    return new [] { new Product { Name = "P1", Price = 100 } };
                }
            }
            public void AddProduct (Product p)
            {
                // do nothing - not required for test
            }
        }

        [Fact]
        public void RepositoryPropertyCalledOnce ()
        {
            // Arrange
            var repo = new PropertyOnceFakeRepository ();
            var controller = new HomeController { Repository = repo };
            // Act
            var result = controller.Index ();
            // Assert
            Assert.Equal (1, repo.PropertyCounter);
        }

        //

        [Theory]
        [ClassData (typeof (ProductTestData))]
        public void IndexActionModelIsCompleteMoq (Product[] products)
        {
            // Arrange
            var mock = new Mock<IProductRepository> ();
            mock.SetupGet (m => m.Products).Returns (products);
            var controller = new HomeController { Repository = mock.Object };
            // Act
            var model = (controller.Index () as ViewResult)?.ViewData.Model
            as IEnumerable<Product>;
            // Assert
            Assert.Equal (controller.Repository.Products, model,
                Comparer.Get<Product> ((p1, p2) => p1.Name == p2.Name &&
                    p1.Price == p2.Price));
        }

        [Fact]
        public void RepositoryPropertyCalledOnceMoq ()
        {
            // Arrange
            var mock = new Mock<IProductRepository> ();
            mock.SetupGet (m => m.Products)
                .Returns (new [] { new Product { Name = "P1", Price = 100 } });
            var controller = new HomeController { Repository = mock.Object };
            // Act
            var result = controller.Index ();
            // Assert
            mock.VerifyGet (m => m.Products, Times.Once);
        }
    }
}