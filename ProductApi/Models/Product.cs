using ProductApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductStock { get; set; }

        public int? ProductCategoryId { get; set; }

        [ForeignKey(nameof(ProductCategoryId))]
        public ProductCategory? Category { get; set; } = null!;

    }
}
