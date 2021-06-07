using Aow.Context;
using System.Threading.Tasks;
using WebApp.Api.Models.Request;

namespace WebApp.Api.Services
{
    public interface IIdentityService
    {
        Task<AuthResult> RegisterAsync(string Email, string Password);
        Task<AuthResult> LoginAsync(string Email, string Password);
        Task<AppUser> GetUserByEmail(string email);
    }
}
