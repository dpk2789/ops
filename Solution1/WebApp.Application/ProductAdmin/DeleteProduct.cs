using OnlineShop.Domain.Interface;
using System;
using System.Threading.Tasks;

namespace Aow.Application.ProductAdmin
{
    [Service]
    public class DeleteProduct
    {
        private IProductRepository _productManager;

        public DeleteProduct(IProductRepository productManager)
        {
            _productManager = productManager;
        }

        public Task<int> Do(Guid id)
        {
            return _productManager.DeleteProduct(id);
        }
    }
}
