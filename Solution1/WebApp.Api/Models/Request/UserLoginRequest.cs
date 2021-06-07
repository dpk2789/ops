using System.ComponentModel.DataAnnotations;

namespace WebApp.Api.Models.Request
{
    public class UserLoginRequest
    {
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }
    }
}
