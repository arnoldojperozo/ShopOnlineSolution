using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Database;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ShopOnlineDbContext _dbContext;

    public ShoppingCartRepository(ShopOnlineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task<bool> CartItemExists(int cartId, int productId)
    {
        return await _dbContext.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
    }
    
    public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAdd)
    {
        if (!await CartItemExists(cartItemToAdd.CartId, cartItemToAdd.ProductId))
        {
            var item = await (from p in _dbContext.Products
                where p.Id == cartItemToAdd.ProductId
                select new CartItem
                {
                    CartId = cartItemToAdd.CartId,
                    ProductId = cartItemToAdd.ProductId,
                    Qty = cartItemToAdd.Qty
                }).SingleOrDefaultAsync();

            if (item == null) return null;
            
            var result = await _dbContext.CartItems.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        return null;
    }

    public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        throw new NotImplementedException();
    }

    public async Task<CartItem> DeleteItem(int id)
    {
        var item = await _dbContext.CartItems.FindAsync(id);

        if (item != null)
        {
            _dbContext.CartItems.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        return item;
    }

    public async Task<CartItem> GetItem(int id)
    {
        return await (from cart in _dbContext.Carts
            join cartItem in _dbContext.CartItems
                on cart.Id equals cartItem.CartId
            where cartItem.Id == id
            select new CartItem
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                Qty = cartItem.Qty,
                CartId = cartItem.CartId
            }).SingleOrDefaultAsync();
    }
    
    public async Task<IEnumerable<CartItem>> GetItems(int userId)
    {
        return await (from cart in _dbContext.Carts
            join cartItem in _dbContext.CartItems
                on cart.Id equals cartItem.CartId
            where cart.UserId == userId
            select new CartItem
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                Qty = cartItem.Qty,
                CartId = cartItem.CartId
            }).ToListAsync();
    }
}