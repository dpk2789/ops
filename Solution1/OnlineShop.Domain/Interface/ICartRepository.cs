using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Interface
{
    public interface ICartRepository
    {
        Task<int> CreateCart(CartProduct stock);
        Task<int> DeleteCart(Guid id);
        Task<int> UpdateCart(List<CartProduct> stockList);

    }
}
