using System.ComponentModel.DataAnnotations;

namespace WebApp.Api.Models.Request
{
    public class UserRegisterRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
