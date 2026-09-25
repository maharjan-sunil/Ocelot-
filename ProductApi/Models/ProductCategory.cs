using ProductAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class ProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
