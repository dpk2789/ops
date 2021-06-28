using Aow.Application.Cart;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Order;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Api.Services;

namespace WebApp.Api.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public OrderController(IIdentityService identityService)
        {
            _identityService = identityService;
        }


        [HttpPost("api/Order/CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder.OrderRequest request, [FromServices] GetCartItems getCart,
            [FromServices] CreateOrder createOrder)
        {
            var user = await _identityService.GetUserByEmail(request.UserId);
            var cart = getCart.Do(user.Id);
            //decimal GetTotalCharge() => cart.Sum(x => x.Value * x.Qty);
            request.UserId = user.Id;
            request.Email = user.Email;
            
            request.Stocks = cart.Select(x => new CreateOrder.OrderRequestItems
            {
                ProductId = x.ProductId,
                Qty = x.Qty
            }).ToList();
            var success = await createOrder.Do(request);

            if (success)
            {
                return Ok("Item Added to cart");
            }

            return BadRequest("Failed to add to cart");
        }
    }
}
