using System;
using System.Collections.Generic;

namespace OnlineShop.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PageTitle { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public decimal Value { get; set; }
        public IList<ProductImage> ProductImages { get; set; }
    }
}
