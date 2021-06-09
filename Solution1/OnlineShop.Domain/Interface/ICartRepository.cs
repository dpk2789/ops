using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Interface
{
    public interface ICartRepository
    {
        IEnumerable<TResult> GetCartProducts<TResult>(string userId, Func<CartProduct, TResult> selector);
        Task<int> CreateCart(CartProduct stock);
        Task<int> DeleteCart(Guid productId, string userId);
        Task<int> UpdateCart(List<CartProduct> stockList);
        Task<int> AddOneToCart(Guid productId, int qty, string userId);

    }
}
