using TaskManager.Application.DTOs.Security.User;
using TaskManager.Application.Interfaces.Services.Security;
using TaskManager.Application.Services.Security;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Api.Controllers.Security
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
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            try
            {
                if (request == null)
                {
                    return BadRequest(new { Message = "Invalid login request." });
                }
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new { Message = "Email and password are required." });
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authService.LoginAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error has occurred.",
                    Details = ex.Message
                });
            }


        }
    }
}
