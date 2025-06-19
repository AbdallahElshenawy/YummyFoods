using Microsoft.EntityFrameworkCore;
using YummyFoods.Data;
using YummyFoods.Models;
using YummyFoods.Repository.IRepository;

namespace YummyFoods.Repository
{
    public class ShoppingCartRepository(ApplicationDbContext context) : IShoppingCartRepository
    {
        public async Task<bool> ClearCartAsync(string? userId)
        {
            var cartItems = await context.ShoppingCarts.Where(u => u.UserId == userId).ToListAsync();
            context.ShoppingCarts.RemoveRange(cartItems);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId)
        {
            return await context.ShoppingCarts.Where(u => u.UserId == userId).Include(u => u.Product).ToListAsync();
        }

        public async Task<bool> UpdateCartAsync(string userId, int productId, int updateBy)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var cart = await context.ShoppingCarts.FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == productId);
            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    ProductId = productId,
                    Count = updateBy
                };

                await context.ShoppingCarts.AddAsync(cart);
            }
            else
            {
                cart.Count += updateBy;
                if (cart.Count <= 0)
                {
                    context.ShoppingCarts.Remove(cart);
                }
            }
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<int> GetTotalCartCountAsync(string? userId)
        {
            int cartCount = 0;
            var cartItems = await context.ShoppingCarts.Where(u => u.UserId == userId).ToListAsync();

            foreach (var item in cartItems)
            {
                cartCount += item.Count;
            }
            return cartCount;
        }
        
    }
}
