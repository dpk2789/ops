using Aow.Application;
using OnlineShop.Domain.Interface;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Application.Order
{
    [Service]
    public class CreateOrder
    {
        private IOrderRepository _orderRepository;       

        public CreateOrder(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;           
        }

        public class OrderRequest
        {
            public string RazorpayPaymentId { get; set; }
            public string RazorpayOrderId { get; set; }
            public string RazorpaySignature { get; set; }
            public string RazorReference { get; set; }
            public string SessionId { get; set; }
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }
            public List<OrderRequestItems> Stocks { get; set; }
        }

        public class OrderRequestItems
        {
            public Guid ProductId { get; set; }
            public int Qty { get; set; }
        }

        public async Task<bool> Do(OrderRequest request)
        {           
            var orderByUser = new Domain.Models.Order
            {
                OrderRef = CreateOrderReference(),
                RazorPayReference = request.RazorReference,

                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                PostCode = request.PostCode,

                OrderProducts = request.Stocks.Select(x => new OrderProduct
                {
                    ProductId = x.ProductId,
                    Qty = x.Qty,
                }).ToList()
            };

            var success = await _orderRepository.CreateOrder(orderByUser) > 0;

            if (success)
            {            

                return true;
            }

            return false;
        }

        public string CreateOrderReference()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new char[12];
            var random = new Random();

            do
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] = chars[random.Next(chars.Length)];
            } while (_orderRepository.OrderReferenceExists(new string(result)));

            return new string(result);
        }
    }
}
