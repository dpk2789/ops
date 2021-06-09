using System;

namespace WebApp.UI.Models
{
    public class CartViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public decimal RealValue { get; set; }
        public int Qty { get; set; }
        public Guid ProductId { get; set; }
    }
}
