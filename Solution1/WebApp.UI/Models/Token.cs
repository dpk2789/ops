namespace WebApp.UI.Models
{
    public class Token
    {
        public string token { get; set; }
        public int expires_in { get; set; }
        public string ClientName { get; set; }
        public string ClientPassword { get; set; }
        public string scope { get; set; }
    }
}
