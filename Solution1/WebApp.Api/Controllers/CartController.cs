using Aow.Application.Cart;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("api/Cart/addtocart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCart.AddToCartRequest request, [FromServices] AddToCart addToCart)
        {
            var user = await _identityService.GetUserByEmail(request.UserName);
            request.UserName = user.Id;
            var success = await addToCart.Do(request);
            if (success)
            {
                return Ok("Item Added to cart");
            }

            return BadRequest("Failed to add to cart");
        }


    }
}
