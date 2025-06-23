using Microsoft.EntityFrameworkCore;
using YummyFoods.Data;
using YummyFoods.Models;
using YummyFoods.Repository.IRepository;

namespace YummyFoods.Repository
{
    public class OrderRepository(ApplicationDbContext context) : IOrderRepository
    {
        public async Task<OrderHeader> CreateAsync(OrderHeader orderHeader)
        {
            orderHeader.OrderDate = DateTime.Now;
            await context.OrderHeaders.AddAsync(orderHeader)!;
            await context.SaveChangesAsync()!;
            return orderHeader;
        }

        public async Task<IEnumerable<OrderHeader>> GetAllAsync(string? userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return await context.OrderHeaders.Where(u => u.UserId == userId).ToListAsync();
            }
            return await context.OrderHeaders.ToListAsync();
        }

        public async Task<OrderHeader> GetAsync(int id)
        {
            return await context.OrderHeaders.Include(u => u.OrderDetails).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<OrderHeader> GetOrderBySessionIdAsync(string sessionId)
        {
            return await context.OrderHeaders
                .FirstOrDefaultAsync(u => u.SessionId == sessionId.ToString());
        }

        public async Task<OrderHeader> UpdateStatusAsync(int orderId, string status, string paymentId)
        {
            var orderHeader = context.OrderHeaders.FirstOrDefault(u => u.Id == orderId);
            if (orderHeader != null)
            {
                orderHeader.Status = status;
                if(!string.IsNullOrEmpty(paymentId))
                {
                    orderHeader.PaymentId = paymentId;
                }
                await context.SaveChangesAsync();
            }
            return orderHeader;
        }
    }
}