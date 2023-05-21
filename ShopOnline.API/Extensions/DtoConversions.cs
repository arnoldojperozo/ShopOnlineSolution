using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from productCategory in productCategories
                select new ProductCategoryDto
                {
                    Id = productCategory.Id,
                    Name = productCategory.Name,
                    IconCSS = productCategory.IconCSS
                }).ToList();
        }

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

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems,
            IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                join product in products
                    on cartItem.ProductId equals product.Id
                select new CartItemDto
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    ProductImageURL = product.ImageURL,
                    Price = product.Price,
                    Qty = cartItem.Qty,
                    CartId = cartItem.CartId,
                    TotalPrice = product.Price * cartItem.Qty
                }).ToList();
        }
        
        public static CartItemDto ConvertToDto(this CartItem cartItem,
            Product product)
        {
            return new CartItemDto
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    ProductImageURL = product.ImageURL,
                    Price = product.Price,
                    Qty = cartItem.Qty,
                    CartId = cartItem.CartId,
                    TotalPrice = product.Price * cartItem.Qty
                };
        }

    }
}
