namespace WebApp.Api.Models.Response
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public int expires_in { get; set; }
    }
}
