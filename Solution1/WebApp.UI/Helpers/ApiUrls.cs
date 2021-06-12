namespace WebApp.UI.Helpers
{
    public static class ApiUrls
    {
       // public const string Rootlocal = "https://localhost:44363/api";
        public const string Rootlocal = "http://api.robustpackagingeshop.com";
       
        public static class Cart
        {
            public const  string GetCartItems = Rootlocal + "/api/Cart/GetCartItems";
            public const string Get = Rootlocal + "/api/Cart/addtocart/{postId}";
            public const string Create = Rootlocal + "/api/Cart/addtocart/companies";
            public const string Update = Rootlocal + "/Cart/{postId}";
            public const string Delete = Rootlocal + "/Cart/{postId}";
        }

        public static class Product
        {
            public const string GetProducts = Rootlocal + "/api/Products/GetProducts";
            public const string GetProduct = Rootlocal + "/api/Products/addtocart/{postId}";
            public const string Create = Rootlocal + "/api/Products/addtocart/companies";
            public const string Update = Rootlocal + "/Products/{postId}";
            public const string Delete = Rootlocal + "/Products/{postId}";
        }

    }
}
