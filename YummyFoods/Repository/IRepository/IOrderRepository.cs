using YummyFoods.Models;

namespace YummyFoods.Repository.IRepository
{
    public interface IOrderRepository
    {
        public Task<OrderHeader> CreateAsync(OrderHeader orderHeader);
        public Task<OrderHeader> GetAsync(int id);
        public Task<IEnumerable<OrderHeader>> GetAllAsync(string? userId = null);
        public Task<OrderHeader> UpdateStatusAsync(int orderId, string status,string paymentId);
        public Task<OrderHeader> GetOrderBySessionIdAsync(string sessionId);

    }
}
