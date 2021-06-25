using System;
using System.Collections.Generic;

namespace OnlineShop.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string OrderRef { get; set; }
        public string RazorPayReference { get; set; }
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
