using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }

        [HttpGet]
        public IActionResult ProductList()
        {
            var productList = productService.GetProductList();
            if (productList == null) return NotFound("Unable to retrive product");

            return Ok(productList);

        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = productService.GetProductById(id);
            if (product == null) return NotFound($"Unable to find a product with Id: {id}");

            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            var result = productService.AddProduct(product);
            if (result == null) return BadRequest($"Unable to add a product with data: {product}");

            return Ok(result);
        }

        [HttpPut]
        public IActionResult UpdateProduct(Product product)
        {
            var result = productService.UpdateProduct(product);
            if (result == null) return BadRequest($"Unable to update a product with data: {product}");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var result = productService.DeleteProduct(id);
            if (result == null) return BadRequest($"Unable to delete a product with Id: {id}");

            return Ok(result);
        }
    }
}

