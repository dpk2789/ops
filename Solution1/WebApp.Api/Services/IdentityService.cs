using Aow.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.Api.Models.Request;

namespace WebApp.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {

            };
            return user;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthResult
                {
                    ErrorMessages = new[] { "User does not exist" }
                };

            };

            var checkPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!checkPassword)
            {
                return new AuthResult
                {
                    ErrorMessages = new[] { "Not Valid Password" }
                };
            }

            return await GenerateTokenForUserAuthResult(user);
        }

        public async Task<AuthResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return new AuthResult
                {
                    ErrorMessages = new[] { "User Allready Exist" }
                };

            };

            var newUser = new AppUser
            {
                Email = email,
                UserName = email
            };
            var createUser = await _userManager.CreateAsync(newUser, password);
            if (!createUser.Succeeded)
            {
                return new AuthResult
                {
                    ErrorMessages = createUser.Errors.Select(x => x.Description)
                };
            }

            return await GenerateTokenForUserAuthResult(newUser);
        }

        private async Task<AuthResult> GenerateTokenForUserAuthResult(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var key = Encoding.ASCII.GetBytes("aaaaaaaaabbbbbbbbbbbbbbbbbbbbbbbbbbb");
            var claims = new List<Claim>();
            foreach (var userClaim in userClaims)
            {
                claims.Add(new Claim(userClaim.Type, userClaim.Value));
            };
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, user.Id));           
            var appIdentity = new ClaimsIdentity(claims);

            //var newTest = new ClaimsIdentity(new Claim[]
            //       {
            //        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //        new Claim(JwtRegisteredClaimNames.Email, user.Email),
            //        new Claim("Id", user.Id),
            //       });
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = appIdentity,
                Expires = DateTime.UtcNow.AddHours(5),
                Issuer = "abc123456789",
                Audience = "abc123456789",
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var Securitytoken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokenstring = new JwtSecurityTokenHandler().WriteToken(Securitytoken);
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenstring);
            var claim = token.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            //token.Header.Add("kid", "");
            //token.Payload.Remove("iss");
            //token.Payload.Add("iss", "your issuer");

            var tokenString = tokenHandler.WriteToken(Securitytoken);

            return new AuthResult
            {
                Success = true,
                Token = tokenString,
                expires_in = 5
            };
        }
    }
}
