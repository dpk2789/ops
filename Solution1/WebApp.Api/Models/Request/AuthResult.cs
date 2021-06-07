using System.Collections.Generic;

namespace WebApp.Api.Models.Request
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public int expires_in { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
