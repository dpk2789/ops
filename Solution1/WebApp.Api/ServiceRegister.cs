using Aow.Application;
using Aow.Context.Repository;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Context.Repository;
using OnlineShop.Domain.Interface;
using System.Linq;
using System.Reflection;
using WebApp.Api.Services;

namespace WebApp.Api
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(
                this IServiceCollection @this)
        {
            var serviceType = typeof(Service);
            var definedTypes = serviceType.Assembly.DefinedTypes;

            var services = definedTypes
                .Where(x => x.GetTypeInfo().GetCustomAttribute<Service>() != null);

            foreach (var service in services)
            {
                @this.AddTransient(service);
            }

            //@this.AddTransient<GetProducts>();
            @this.AddScoped<IIdentityService, IdentityService>();
            @this.AddTransient<IProductRepository, ProductRepository>();
            @this.AddTransient<ICartRepository, CartRepository>();
            @this.AddTransient<IOrderRepository, OrderRepository>();
            return @this;
        }
    }
        
}
