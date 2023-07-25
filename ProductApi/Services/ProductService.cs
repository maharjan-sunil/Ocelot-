using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using Serilog;

namespace ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly DbContextClass _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(DbContextClass dbContext, ILogger<ProductService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IEnumerable<Product> GetProductList()
        {
            try
            {
                var products = _dbContext.Products.ToList();
                _logger.LogInformation("Retrieved {Count} products", products.Count);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Unable to retrive products", ex);
            }

            return null;
        }

        public Product GetProductById(int id)
        {
            try
            {
                var product = _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefault();
                if (product == null)
                {
                    _logger.LogInformation("Unable to find a product with Id: {id}", id);
                    return null;
                }

                _logger.LogInformation("Retrieved a product with Id: {id}", id);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to find a product with Id: {id}", ex);
            }

            return null;
        }

        public Product AddProduct(Product product)
        {
            try
            {
                var checkProductExists = _dbContext.Products.AsNoTracking().Where(x => x.ProductId == product.ProductId).FirstOrDefault();
                if (checkProductExists != null)
                {
                    _logger.LogInformation("Product with Id: {Id} already exists", product.ProductId);
                    return null;
                }

                var result = _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                _logger.LogInformation("Successfully added the product with data: {product}", product);

                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to add a product with data: {product}", ex);
            }

            return null;
        }

        public Product UpdateProduct(Product product)
        {
            try
            {
                var checkProductExists = _dbContext.Products.AsNoTracking().Where(x => x.ProductId == product.ProductId).FirstOrDefault();
                if (checkProductExists == null)
                {
                    _logger.LogInformation("Product with Id: {Id} doesn't exists", product.ProductId);
                    return null;
                }

                var result = _dbContext.Products.Update(product);
                _dbContext.SaveChanges();
                _logger.LogInformation("Successfully updated the product with data: {product}", product);

                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to update a product with data: {product}", ex);
            }

            return null;
        }

        public bool? DeleteProduct(int id)
        {
            try
            {
                var checkProductExists = _dbContext.Products.AsNoTracking().Where(x => x.ProductId == id).FirstOrDefault();
                if (checkProductExists == null)
                {
                    _logger.LogInformation("Product with Id: {id} doesn't exists", id);
                    return null;
                }

                var filteredData = _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefault();
                var result = _dbContext.Remove(filteredData);
                _dbContext.SaveChanges();

                if (result != null)
                    _logger.LogInformation("Successfully deleted the product with Id: {id}", id);
                else
                    _logger.LogInformation("Unable to delete a product with Id: {id}", id);

                return result != null ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to delete a product with Id: {id}", ex);
            }

            return null;
        }
    }
}
