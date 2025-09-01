using System.Security.Claims;
using TaskManager.Application.DTOs.Security.User;
using TaskManager.Application.Interfaces.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace TaskManager.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet("GetAllUser")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _userService.GetAllUserAsync();
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
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

        // GET api/<UserController>/5
        [HttpGet("GetUserById/{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
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
        [HttpGet("GetUserByUserName")]
        [Authorize]
        public async Task<IActionResult> Get(string userName)
        {
            try
            {
                var result = await _userService.GetUserByUserNameAsync(userName);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
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

        // POST api/<UserController>
        [HttpPost("CreateUser")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] UserAddDto userAddDto)
        {
            try
            {
                
                var userCreateIdClaim = User.FindFirst("userId") ?? User.FindFirst(ClaimTypes.NameIdentifier);

                if (userCreateIdClaim == null || !int.TryParse(userCreateIdClaim.Value, out int userCreateId))
                {
                    return Unauthorized("No se encontró un userId válido en el token.");
                }

                
                var result = await _userService.CreateUserAsync(userAddDto, userCreateId);

                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Error inesperado",
                    Details = ex.Message
                });
            }
        }



        // PUT api/<UserController>/5
        [HttpPut("UpdateUser/{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            try
            {
         
                if (userUpdateDto == null)
                    return BadRequest("Datos de usuario inválidos.");

              
                if (id != userUpdateDto.IdUser)
                    return BadRequest("El ID de usuario proporcionado no coincide con el del cuerpo de la solicitud.");

              
                var userClaim = User.FindFirst("userId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaim == null || !int.TryParse(userClaim.Value, out int userUpdateId))
                    return Unauthorized("No se encontró un userId válido en el token.");

      
                var result = await _userService.UserUpdateAsync(userUpdateDto, userUpdateId);

                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Ha ocurrido un error inesperado.",
                    Details = ex.Message
                });
            }
        }


    }
}
