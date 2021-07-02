using System;

namespace OnlineShop.Domain.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Extention { get; set; }
        public long Length { get; set; }
        public string RelativePath { get; set; }
        public string GlobalPath { get; set; }
        public Guid ProductId { get; set; }
        public Product Order { get; set; }

    }
}
