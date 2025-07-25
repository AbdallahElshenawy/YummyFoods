﻿using YummyFoods.Models;

namespace YummyFoods.Utility
{
    public static class StaticDetails
    {
        public static string RoleAdmin = "Admin";
        public static string RoleCustomer = "Customer";

        public static string StatusPending = "Pending";
        public static string StatusApproved = "Approved";
        public static string StatusReadyForPickUp = "ReadyForPickUp";
        public static string StatusCompleted = "Completed";
        public static string StatusCancelled = "Cancelled";
        public static List<OrderDetail> ConvertShoppingCartListToOrderDetail(List<ShoppingCart> shoppingCarts)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (var cart in shoppingCarts)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    ProductId = cart.ProductId,
                    Count = cart.Count,
                    Price = Convert.ToDouble(cart.Product.Price),
                    ProductName = cart.Product.Name
                };
                orderDetails.Add(orderDetail);
            }

            return orderDetails;
        }
    }
}
