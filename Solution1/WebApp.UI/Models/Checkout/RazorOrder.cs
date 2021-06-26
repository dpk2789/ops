namespace WebApp.UI.Models.Checkout
{
    public class RazorOrder
    {
        public string id { get; set; }
        public string entity { get; set; }
        public decimal amount { get; set; }
        public decimal amount_paid { get; set; }
        public string currency { get; set; }
        public string receipt { get; set; }
        public string status { get; set; }
        public string offer_id { get; set; }
        public string attempts { get; set; }
        public string[] notes { get; set; }
        public string created_at { get; set; }
    }
}
