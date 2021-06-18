using System;

namespace WebApp.UI.Models
{
    public class CartProductViewModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Value { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
    }
}
