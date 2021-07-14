using System;
using System.Collections.Generic;

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
        public IEnumerable<CartProductImage> ProductImages { get; set; }
    }
    public class CartProductImage
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public long Width { get; set; }
        public string RelativePath { get; set; }
        public string GlobalPath { get; set; }
        public string Type { get; set; }
        public string Extention { get; set; }
    }
}
