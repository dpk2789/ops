using Aow.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;

namespace Aow.Context
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(connectionString));
          

        
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {              
                var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
                ctx.Database.Migrate();
                EnsureUsers(scope);
            }
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var alice = userMgr.FindByNameAsync("alice").Result;
            if (alice == null)
            {
                alice = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                };
                var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(alice, new Claim[]
                {
                          new Claim("Name", "Alice Smith"),                        
                          new Claim("WebSite", "http://alice.com"),
                            new Claim("Role", "admin"),
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("alice created");
            }
            else
            {
                Log.Debug("alice already exists");
            }

            var bob = userMgr.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new IdentityUser
                {
                    UserName = "bob",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, new Claim[]
                {
                          new Claim("Name", "bob Smith"),
                          new Claim("WebSite", "http://bob.com"),
                            new Claim("Role", "manager"),
                                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
        }


        
    }
}
