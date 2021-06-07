using System;

namespace OnlineShop.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
