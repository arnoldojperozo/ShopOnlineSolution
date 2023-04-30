using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products, IEnumerable<ProductCategory> productCategories)
        {
            return (from product in products
                join productCategory in productCategories
                    on product.CategoryId equals productCategory.Id
                    select new ProductDto
                    {
                        Id = product.Id,
                        CategoryId = product.CategoryId,
                        CategoryName = productCategory.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        Name = product.Name,
                        Price = product.Price,
                        Qty = product.Qty
                    }).ToList();
        }
        
        public static ProductDto ConvertToDto(this Product product, ProductCategory productCategory)
        {
            return new ProductDto
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                CategoryName = productCategory.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Name = product.Name,
                Price = product.Price,
                Qty = product.Qty
            };
        }
    }
}
