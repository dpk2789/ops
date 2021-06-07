using System;
using System.Collections.Generic;

namespace WebApp.UI.Models
{
    public interface ISessionManager
    {
        IEnumerable<TResult> GetCart<TResult>(Func<CartProductViewModel, TResult> selector);
        void AddProductToSession(Guid Id);
        void RemoveProduct(Guid stockId, int qty);       
        void ClearCart();
    }
}
