using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Dtos;
using Product.Application.Services.Contracts;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }


        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(SuccessResponse<RegisterResponse>), 200)]
        public async Task<ActionResult> Register(RegisterDto model)
        {
            var data = await _service.RegisterUser(model);

            return Ok(data);
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(SuccessResponse<LoginResponse>), 200)]
        public async Task<ActionResult> Login(LoginDto model)
        {
            var data = await _service.Login(model);

            return Ok(data);
        }
    }
}
