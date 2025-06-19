using YummyFoods.Models;

namespace YummyFoods.Repository.IRepository
{
    public interface IShoppingCartRepository
    {

        Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId);
        Task<bool> UpdateCartAsync(string userId, int productId, int updateBy);
        Task<bool> ClearCartAsync(string? userId);
        public Task<int> GetTotalCartCountAsync(string? userId);

    }
}
