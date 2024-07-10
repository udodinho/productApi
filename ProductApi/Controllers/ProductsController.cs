using Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Dtos;
using Product.Application.Helpers;
using Product.Application.Services.Contracts;
using Product.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResponse<List<ProductItem>>), 200)]
        public async Task<IActionResult> Get([FromQuery] ResourceParameters parameter)
        {
            var result = await _service.Products(parameter, nameof(Get), Url);
            return Ok(result);
        }

        [HttpPost("{userId}")]
        [ProducesResponseType(typeof(SuccessResponse<ProductsResponse>), 200)]
        public async Task<IActionResult> Create([FromRoute] string userId, CreateProductDto model)
        {
            var result = await _service.CreateProduct(userId, model);
            return Ok(result);
        }

        [HttpGet("{userId}/single")]
        [ProducesResponseType(typeof(SuccessResponse<ProductsResponse>), 200)]
        public async Task<IActionResult> GetOne([FromRoute] string userId, [FromQuery] string productId)
        {
            var result = await _service.Product(userId, productId);
            return Ok(result);
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(SuccessResponse<ProductsResponse>), 200)]
        public async Task<IActionResult> Update([FromRoute] string userId, [FromQuery] string productId, UpdateProductDto model)
        {
            var result = await _service.UpdateProduct(userId, productId, model);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(SuccessResponse<List<ProductItem>>), 200)]
        public async Task<IActionResult> Delete([FromRoute] string userId, [FromQuery] string productId)
        {
            var result = await _service.DeleteProduct(userId, productId);
            return Ok(result);
        }
    }
}
