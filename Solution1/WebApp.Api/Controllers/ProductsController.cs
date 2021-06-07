using Aow.Application.ProductAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace WebApp.Api.Controllers
{
    //[Route("api/[controller]")]    
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("api/Products/GetProducts")]
        public IActionResult GetProducts([FromServices] GetProducts getProducts) =>
            Ok(getProducts.Do());


        [HttpGet("api/Products/{id}")]
        public IActionResult GetProduct(Guid id,
           [FromServices] GetProduct getProduct) =>
           Ok(getProduct.Do(id));

        [Authorize]
        [HttpPost("api/Products/CreateProduct")]
        public async Task<IActionResult> CreateProduct(
            [FromBody] CreateProduct.CreateRequest request,
            [FromServices] CreateProduct createProduct) =>
            Ok((await createProduct.Do(request)));

        [Authorize]
        [HttpPut("api/Products/{id}")]
        public async Task<IActionResult> PutCompany(Guid id, [FromBody] UpdateProduct.Request request, [FromServices] UpdateProduct updateProduct)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            try
            {
                
                Ok((await updateProduct.Do(request)));
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("api/Products/{id}")]
        public async Task<IActionResult> DeleteProduct(
            Guid id,
            [FromServices] DeleteProduct deleteProduct) =>
            Ok((await deleteProduct.Do(id)));

    }
}
