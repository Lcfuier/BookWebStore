using BussinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Interface;
using Entity.Models;
using Entity.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _data;

        public OrderService(IUnitOfWork data)
        {
            _data = data;
        }

        public async Task<IEnumerable<Order>> GetAllOrderByUserId(string userId)
        {
            QueryOptions<Order> options = new()
            {
                Includes = "OrderDetails.Book",
                Where = c => c.customerId.Equals(userId)
            };

            return await _data.Order.ListAllAsync(options);
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _data.Order.ListAllAsync(new QueryOptions<Order>());
        }
        public async Task UpdateStatus(Guid Id,string? orderStatus, string? paymentStatus )
        {
            var order = await _data.Order.GetAsync(Id);
            if (order != null)
            {
                order.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    order.PaymentStatus = paymentStatus;
                }
            }
            else
            {
                // Log here, this should not thrown
                throw new NullReferenceException("Order Header is null");
            }
           await _data.SaveAsync();
        }
        public async Task UpdateStripePaymentId(Guid Id, string sessionId, string paymentIntentId)
        {
            var order = await _data.Order.GetAsync(Id);
            if (order != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    order.SessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    order.PaymentIntentId = paymentIntentId;
                }
            }
            else
            {
                // Log here, this should not thrown
                throw new NullReferenceException("Order Header is null");
            }
            await _data.SaveAsync();    
        }
        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            QueryOptions<Order> options = new()
            {
                Includes = "OrderDetails.Book",
                Where = c => c.OrderId == id
            };

            return await _data.Order.GetAsync(options);
        }

        public async Task<Order?> GetOrderByCustomerIdAsync(string id)
        {
            QueryOptions<Order> options = new()
            {
                Includes = "OrderDetails.Book",
                Where = c => c.customerId.Equals(id)
            };

            return await _data.Order.GetAsync(options);
        }

        public async Task AddOrderAsync(Order order)
        {
            _data.Order.Add(order);
            await _data.SaveAsync();
        }

        public async Task UpdateOrderAsync(Order category)
        {
            _data.Order.Update(category);
            await _data.SaveAsync();
        }

        public async Task RemoveOrderAsync(Order category)
        {
            _data.Order.Remove(category);
            await _data.SaveAsync();
        }
    }
}
