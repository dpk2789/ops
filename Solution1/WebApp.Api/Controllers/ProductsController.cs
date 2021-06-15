﻿using Aow.Application.ProductAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApp.Api.Models.Response;

namespace WebApp.Api.Controllers
{
    //[Route("api/[controller]")]    
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("api/Products/GetProducts")]
        public IActionResult GetProducts([FromServices] GetProducts getProducts) =>
            Ok(getProducts.Do());


        [HttpGet("api/Products/GetProduct")]
        public IActionResult GetProduct(Guid id,
           [FromServices] GetProduct getProduct) =>
           Ok(getProduct.Do(id));

        [Authorize]
        [HttpPost("api/Products/CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProduct.CreateRequest request, [FromServices] CreateProduct createProduct)
        {
            try
            {
                var response = await createProduct.Do(request);
                return Ok(new ApiResponse
                {
                    Msg = "Successfully added !!",
                    Success = true
                });

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }


        [Authorize]
        [HttpPut("api/Products/UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProduct.Request request, [FromServices] UpdateProduct updateProduct)
        {
            try
            {
                var response = await updateProduct.Do(request);
                return Ok(new UpdateProduct.Response
                {
                    Id = response.Id,
                    Name = response.Name,
                    Description = response.Description,
                    Value = response.Value
                });

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpDelete("api/Products/DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(Guid id, [FromServices] DeleteProduct deleteProduct)
        {
            var result = await deleteProduct.Do(id);
            if (result > 0)
            {
                return Ok(new ApiResponse
                {
                    Msg = "Successfully Deleted !!",
                    Success = true
                });
            }
            return BadRequest();
        }


    }
}
