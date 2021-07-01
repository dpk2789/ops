using System;

namespace OnlineShop.Domain.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Extention { get; set; }
        public decimal Path { get; set; }
        public Guid ProductId { get; set; }
        public Order Order { get; set; }

    }
}
