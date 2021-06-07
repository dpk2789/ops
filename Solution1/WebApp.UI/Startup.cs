using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebApp.UI.Helpers;
using WebApp.UI.Models;

namespace WebApp.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddRazorPages();
            services
                .AddMvc(o => o.EnableEndpointRouting = false);
            services.AddTransient<IEmailSender, EmailSender>(i =>
                   new EmailSender(
                       Configuration["EmailSender:Host"],
                       Configuration.GetValue<int>("EmailSender:Port"),
                       Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                       Configuration["EmailSender:UserName"],
                       Configuration["EmailSender:Password"]
                   )
               );

            services.AddSession(options =>
            {
                options.Cookie.Name = "Cart";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(20);
            });
            services.AddScoped<ISessionManager, SessionManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseMvcWithDefaultRoute();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //    endpoints.MapControllers();
            //});
        }
    }
}
