using System;

namespace WebApp.UI.Models
{
    public class CartViewModel
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal RealValue { get; set; }
        public decimal Amount { get; set; }
        public int Qty { get; set; }
        public Guid ProductId { get; set; }
    }
}
