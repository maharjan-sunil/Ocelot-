using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.Models;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Services;

namespace ProductApiTest
{
    public class ProductServiceTests
    {
        private readonly DbContextOptions<DbContextClass> options = new DbContextOptionsBuilder<DbContextClass>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) //using a unique database name for each test
             .Options;
        private readonly Mock<ILogger<ProductService>> _loggerMock = new Mock<ILogger<ProductService>>();

        public ProductServiceTests()
        {

        }

        [Fact]
        public void GetProductList_ShouldReturnListOfProduct_WhenProductExists()
        {
            //Arrange
            IEnumerable<Product> products = Enumerable.Empty<Product>();
            using (var context = new DbContextClass(options, true))
            {
                context.Products.Add(new Product
                {
                    ProductId = 1,
                    ProductName = "ProductName",
                    ProductDescription = "ProductDescription",
                    ProductPrice = 10,
                    ProductStock = 10
                });
                context.Products.Add(new Product
                {
                    ProductId = 2,
                    ProductName = "ProductName2",
                    ProductDescription = "ProductDescription",
                    ProductPrice = 10,
                    ProductStock = 10
                });
                context.SaveChanges();
            }

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IProductService _productService = new ProductService(context, _loggerMock.Object);
                products = _productService.GetProductList();
            }

            //Assert
            Assert.Equal(2,products.Count());
        }

        [Fact]
        public void GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            //Arrange
            var productId = 3;
            Product product = new Product();
            using (var context = new DbContextClass(options, true))
            {
                context.Products.Add(new Product
                {
                    ProductId = productId,
                    ProductName = "ProductName3",
                    ProductDescription = "ProductDescription",
                    ProductPrice = 10,
                    ProductStock = 10
                });
                context.SaveChanges();
            }

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IProductService _productService = new ProductService(context, _loggerMock.Object);
                product = _productService.GetProductById(productId);
            }

            //Assert
            Assert.Equal(productId, product.ProductId);
        }

        [Fact]
        public void GetProductById_ShouldReturnNothing_WhenProductDoesNotExists()
        {
            //Arrange
            Product product = new Product();

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IProductService _productService = new ProductService(context, _loggerMock.Object);
                product = _productService.GetProductById(It.IsAny<int>());
            }

            //Assert
            Assert.Null(product);
        }

        [Fact]
        public void AddProduct_ShouldReturnProduct_WhenProductInsertedSuccessfully()
        {
            //Arrange
            Product product = new Product();

            Product productItem = new Product
            {
                ProductName = "ProductName100",
                ProductDescription = "ProductDescription",
                ProductPrice = 10,
                ProductStock = 10
            };

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IProductService _productService = new ProductService(context, _loggerMock.Object);
                product = _productService.AddProduct(productItem);
            }

            //Assert
            Assert.NotNull(product);
        }

        [Fact]
        public void AddProduct_ShouldReturnNull_WhenDuplicateProductIdIsPassed()
        {
            //Arrange
            Product product = new Product();
            var productId = 5;
            using (var context = new DbContextClass(options, true))
            {
                context.Products.Add(new Product
                {
                    ProductId = productId,
                    ProductName = "ProductName5",
                    ProductDescription = "ProductDescription",
                    ProductPrice = 10,
                    ProductStock = 10
                });
                context.SaveChanges();
            }

            Product productItem = new Product
            {
                ProductId = productId,
                ProductName = "ProductName5",
                ProductDescription = "ProductDescription",
                ProductPrice = 10,
                ProductStock = 10
            };

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IProductService _productService = new ProductService(context, _loggerMock.Object);
                product = _productService.AddProduct(productItem);
            }

            //Assert
            Assert.Null(product);
        }

        [Fact]
        public void UpdateProduct_ShouldReturnUpdatedProduct_WhenProductUpdatedSuccessfully()
        {
            //Arrange
            Product product = new Product();

            var productId = 6;
            using (var context = new DbContextClass(options, true))
            {
                context.Products.Add(new Product
                {
                    ProductId = productId,
                    ProductName = "ProductName6",
                    ProductDescription = "ProductDescription",
                    ProductPrice = 10,
                    ProductStock = 10
                });
                context.SaveChanges();
            }

            Product productItem = new Product
            {
                ProductId = productId,
                ProductName = "ProductNameChanged",
                ProductDescription = "ProductDescription",
                ProductPrice = 10,
                ProductStock = 10
            };

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IProductService _productService = new ProductService(context, _loggerMock.Object);
                product = _productService.UpdateProduct(productItem);
            }

            //Assert
            Assert.NotNull(product);
            Assert.Equal(productItem.ProductName, product.ProductName);
        }

        [Fact]
        public void UpdateProduct_ShouldReturnNull_WhenProductIdDoesnotExists()
        {
            //Arrange
            Product product = new Product();
            var productId = 10;

            Product productItem = new Product
            {
                ProductId = productId,
                ProductName = "ProductName10",
                ProductDescription = "ProductDescription",
                ProductPrice = 10,
                ProductStock = 10
            };

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IProductService _productService = new ProductService(context, _loggerMock.Object);
                product = _productService.UpdateProduct(productItem);
            }

            //Assert
            Assert.Null(product);
        }

        [Fact]
        public void DeleteProduct_ShouldReturnTrue_WhenProductDeletedSuccessfully()
        {
            //Arrange
            bool? result = null;

            var productId = 7;
            using (var context = new DbContextClass(options, true))
            {
                context.Products.Add(new Product
                {
                    ProductId = productId,
                    ProductName = "ProductName7",
                    ProductDescription = "ProductDescription",
                    ProductPrice = 10,
                    ProductStock = 10,
                    ProductCategoryId = 1
                });
                context.SaveChanges();
            }

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IProductService _productService = new ProductService(context, _loggerMock.Object);
                result = _productService.DeleteProduct(productId);
            }

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteProduct_ShouldReturnNull_WhenProductIdDoesnotExists()
        {
            //Arrange
            bool? result = null;
            var productId = 10;

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IProductService _productService = new ProductService(context, _loggerMock.Object);
                result = _productService.DeleteProduct(productId);
            }

            //Assert
            Assert.Null(result);
        }


        [Fact]
        public void DeleteCategory_WithLinkedProduct_BehaviorsDiverge()
        {
            // 1. Arrange: Create a Category and link a Product to it
            var categoryId = 1;
            using (var context = new DbContextClass(options, true))
            {
                var category = new ProductCategory { ProductCategoryId = categoryId, ProductCategoryName = "Electronics" };
                context.Categories.Add(category);

                context.Products.Add(new Product
                {
                    ProductId = 50,
                    ProductName = "Phone",
                    ProductDescription = "Smartphone",
                    ProductCategoryId = categoryId // Relational link
                });
                context.SaveChanges();
            }

            bool result = false;

            // 2. Act: Try to delete the Category while the Product still depends on it
            using (var context = new DbContextClass(options, true))
            {
                try
                {
                    var categoryToDelete = context.Categories.Find(categoryId);
                    context.Categories.Remove(categoryToDelete!);
                    context.SaveChanges();

                    result = true; // EF In-Memory reaches here successfully!
                }
                catch (DbUpdateException)
                {
                    result = false; // SQLite triggers this catch block because of the foreign key constraint
                }
            }

            // 3. Assert: The exact same line of code
            // -> EF Core In-Memory: 'result' is true. This line PASSES.
            // -> SQLite In-Memory:  'result' is false. This line FAILS.
            Assert.True(result);
        }





    }
}