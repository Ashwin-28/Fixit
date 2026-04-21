using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fixit_API.Services;
using Fixit_API.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Fixit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authServices;

        public AuthController(AuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _authServices.Register(registerDTO);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(new { token = result.Token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _authServices.Login(loginDTO);
            if (!result.Success)
            {
                return Unauthorized(new { message = result.Error });
            }

            return Ok(new { message="Login Successfull" });
        }
    }
}
