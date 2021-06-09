using Aow.Application.Cart;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Api.Services;

namespace WebApp.Api.Controllers
{
    //  [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public CartController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet("api/Cart/GetCartItems")]
        public async Task<IActionResult> GetCartItems(string userId, [FromServices] GetCartItems getCart)
        {
            var user = await _identityService.GetUserByEmail(userId);
            var cart = getCart.Do(user.Id);
            return Ok(cart);
        }

        [HttpPost("api/Cart/addtocart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCart.AddToCartRequest request, [FromServices] AddToCart addToCart)
        {
            var user = await _identityService.GetUserByEmail(request.UserId);
            request.UserId = user.Id;
            var success = await addToCart.Do(request);
            if (success)
            {
                return Ok("Item Added to cart");
            }

            return BadRequest("Failed to add to cart");
        }

        [HttpPost("api/Cart/AddOneToCart")]
        public async Task<IActionResult> AddOneToCart([FromBody] AddToCart.AddToCartRequest request, [FromServices] AddToCart addToCart)
        {
            var user = await _identityService.GetUserByEmail(request.UserId);
            request.UserId = user.Id;
            request.Qty = 1;
            var success = await addToCart.Do(request);

            if (success)
            {
                return Ok("Item Added to cart");
            }

            return BadRequest("Failed to add to cart");
        }


        [HttpPost("api/Cart/RemoveProduct")]
        public async Task<IActionResult> RemoveProduct([FromBody] RemoveFromCart.RemoveFromCartRequest request, 
            [FromServices] RemoveFromCart removeFromCart)
        {

            var user = await _identityService.GetUserByEmail(request.UserId);
            request.UserId = user.Id;
            var success = await removeFromCart.Do(request);

            if (success)
            {
                return Ok("Item removed from cart");
            }

            return BadRequest("Failed to remove item from cart");
        }

    }
}
