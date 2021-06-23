using System;
using System.Collections.Generic;
using WebApp.UI.Models.cart;

namespace WebApp.UI.Models
{
    public interface ISessionManager
    {
        IEnumerable<TResult> GetCart<TResult>(Func<CartProductViewModel, TResult> selector);
        void AddProductToSession(CartProductRequest request);
        void RemoveProduct(Guid productId);
        void RemoveOneQuantityFromCartSession(CartProductRequest productViewModel);
        void ClearCart();

        void AddCustomerInformation(CustomerInformation customer);
        CustomerInformation GetCustomerInformation();
    }
}
