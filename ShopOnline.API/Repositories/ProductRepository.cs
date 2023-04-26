using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Database;
using ShopOnline.API.Entities;
using ShopOnline.Models.Repositories.Contracts;

namespace ShopOnline.Models.Repositories
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

        public Task<Product> GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            throw new NotImplementedException();
        }
    }
}
