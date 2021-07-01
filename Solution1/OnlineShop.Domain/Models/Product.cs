using System;
using System.Collections.Generic;

namespace OnlineShop.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
