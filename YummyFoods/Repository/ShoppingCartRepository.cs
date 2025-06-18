using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YummyFoods.Data;
using YummyFoods.Models;
using YummyFoods.Repository.IRepository;

namespace YummyFoods.Repository
{
    public class ShoppingCartRepository(ApplicationDbContext context) : IShoppingCartRepository
    {
        public async Task<bool> ClearCartAsync(string? userId)
        {
            var carItems = await context.ShoppingCarts
                .Where(c => c.UserId == userId)
                .ToListAsync()!;
            context.ShoppingCarts.RemoveRange(carItems);
            return await context.SaveChangesAsync()! > 0;
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId)
        {
            return await context.ShoppingCarts.Where(c => c.UserId == userId).Include(u=>u.Product).ToListAsync()!; 
        }

        public async Task<bool> UpdateCartAsync(string userId, int productId, int updateBy)
        {
            if (string.IsNullOrEmpty(userId))
                return false;
            var cartItem = await context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem is null)
            {
                cartItem = new ShoppingCart
                {
                    UserId = userId,
                    ProductId = productId,
                    Count = updateBy
                };
                await context.ShoppingCarts.AddAsync(cartItem)!;
            }
            else
            {
                cartItem.Count += updateBy;
                if (cartItem.Count <= 0)
                {
                    context.ShoppingCarts.Remove(cartItem);
                }
            }
            return await context.SaveChangesAsync()! > 0;
        }
     
    }
}
