using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Database;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;

namespace ShopOnline.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _dbContext;
        public ProductRepository(ShopOnlineDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await _dbContext.Products.ToListAsync();

            return products;
        }

        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await _dbContext.ProductCategories.ToListAsync();

            return categories;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await _dbContext.ProductCategories.FindAsync(id);

            return category;
        }

        public async Task<Product> GetItem(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            return product;
        }
    }
}
