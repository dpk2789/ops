namespace WebApp.UI.Helpers
{
    public class IdentityUrls
    {
       // public const string Rootlocal = "https://localhost:44347";
        public const string Rootlocal = "http://api.robustpackagingeshop.com";
        public static class Identity
        {
            public const string Login = Rootlocal + "/api/user/login";
            public const string Register = Rootlocal + "/api/user/register";
        }

    }
}
