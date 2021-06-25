using Aow.Application.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Api.Services;

namespace WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public OrderController(IIdentityService identityService)
        {
            _identityService = identityService;
        }


        [HttpPost("api/Cart/createorder")]
        public async Task<IActionResult> CreateOrder(string userName, [FromServices] GetCartItems getCart, [FromServices] CreateOrder createOrder)
        {
            var user = await _identityService.GetUserByEmail(userName);
            var cart = getCart.Do(user.Id);

            var success = await createOrder.Do(new CreateOrder.Request
            {
                RazorReference = "abc",
                FirstName = user.UserName,               
                Email = user.UserName,
                PhoneNumber = user.PhoneNumber,              
                
                Stocks = cart.Select(x => new CreateOrder.OrderRequestItems
                {
                    ProductId = x.ProductId,
                    Qty = x.Qty
                }).ToList()
            });

            if (success)
            {
                return Ok("Item Added to cart");
            }

            return BadRequest("Failed to add to cart");
        }
    }
}
