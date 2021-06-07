using Aow.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Api.Models.Request;
using WebApp.Api.Models.Response;
using WebApp.Api.Services;

namespace WebApp.Api.Controllers
{
    //  [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        public UserController(IIdentityService identityService, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IEmailSender emailSender)
        {
            _identityService = identityService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpPost("api/user/register")]
        public async Task<IActionResult> register([FromBody] UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = request.Email, Email = request.Email };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = request.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return Ok(new AuthRegiterResponse
                        {
                            UserId = user.Id,
                            Msg = code,
                            Success = true
                        });
                    }
                }
                List<string> ErrorMessages = new List<string>();

                foreach (var error in result.Errors)
                {
                    ErrorMessages.Add(error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(new AuthRegiterResponse
                {
                    ErrorMessages = ErrorMessages,
                    Success = false
                });
            }
            var authResult = await _identityService.RegisterAsync(request.Email, request.Email);
            if (!authResult.Success)
            {
                return BadRequest(new AuthRegiterResponse
                {
                    ErrorMessages = authResult.ErrorMessages.ToList(),
                    Success = false
                });
            }
            return Ok();
        }


        [HttpPost("api/user/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResult = await _identityService.LoginAsync(request.email, request.password);
            if (!authResult.Success)
            {
                return BadRequest();
            }
            return Ok(new AuthResponse
            {
                Token = authResult.Token,
                Success = true,
                expires_in = authResult.expires_in
            });
        }
    }
}
