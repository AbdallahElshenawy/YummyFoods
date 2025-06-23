using Microsoft.AspNetCore.Components;
using Stripe.Checkout;
using YummyFoods.Models;
using YummyFoods.Repository;
using YummyFoods.Repository.IRepository;
using YummyFoods.Utility;

namespace YummyFoods.Services
{
    public class PaymentService(NavigationManager navigationManager, IOrderRepository orderRepository)
    {
        public Session CreateStripeCheckoutSession(OrderHeader orderHeader)
        {

            var lineItems = orderHeader.OrderDetails
                .Select(order => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmountDecimal = (decimal?)order.Price * 100,  
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = order.ProductName
                        }

                    },
                    Quantity = order.Count
                }).ToList();

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = $"{navigationManager.BaseUri}order/confirmation/{{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{navigationManager.BaseUri}cart",
                LineItems = lineItems,
                Mode = "payment",
            };

            var service = new SessionService();
            var session = service.Create(options);

            return session;
        }

        public async Task<OrderHeader> CheckPaymentStatusAndUpdateOrder(string sessionId)
        {
            OrderHeader orderHeader = await orderRepository.GetOrderBySessionIdAsync(sessionId);
            var service = new SessionService();
            var session = service.Get(sessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                await orderRepository.UpdateStatusAsync(orderHeader.Id, StaticDetails.StatusApproved, session.PaymentIntentId);
            }
            return orderHeader;
        }
    }
}