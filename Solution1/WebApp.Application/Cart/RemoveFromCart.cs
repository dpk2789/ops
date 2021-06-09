using OnlineShop.Domain.Interface;
using System;
using System.Threading.Tasks;

namespace Aow.Application.Cart
{
    [Service]
    public class RemoveFromCart
    {
        private ICartRepository _cartRepository;

        public RemoveFromCart(
            ICartRepository stockManager)
        {
            _cartRepository = stockManager;
        }

        public class RemoveFromCartRequest
        {
            public Guid ProductId { get; set; }
            public string UserId { get; set; }
            public int Qty { get; set; }
        }

        public async Task<bool> Do(RemoveFromCartRequest request)
        {
            if (request.Qty <= 0)
            {
                return false;
            }

            var success = await _cartRepository.DeleteCart(request.ProductId, request.UserId);

            if (success != 0)
            {
                return true;
            }

            return false;
        }
    }
}
