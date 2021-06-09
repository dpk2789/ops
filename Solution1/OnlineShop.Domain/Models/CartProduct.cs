using System;

namespace OnlineShop.Domain.Models
{
    public class CartProduct
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Qty { get; set; }
        public decimal Value { get; set; }
        public string UserId { get; set; }
    }
}
