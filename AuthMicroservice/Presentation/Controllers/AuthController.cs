using AuthMicroservice.Application.DTOs;
using AuthMicroservice.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthMicroservice.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var token = await _authService.AuthenticateAsync(userDto);
            if (token == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new { token });
        }
    }
}
