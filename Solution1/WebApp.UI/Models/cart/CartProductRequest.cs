using System;

namespace WebApp.UI.Models.cart
{
    public class CartProductRequest
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }     
        public int Qty { get; set; }
        public decimal Value { get; set; }
    }
}
