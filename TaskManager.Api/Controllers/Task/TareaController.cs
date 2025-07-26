using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Api.Controllers.Task
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        private readonly ITareaService _tareaService;

        public TareaController (ITareaService tareaService)
        {
            _tareaService = tareaService;
        }

        // GET: api/<TareaController>
        [HttpGet("GetAllTask")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _tareaService.GetAllTareaAsync();
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
                    Message = "An unexpected error has ocurred",
                    Details = ex.Message
                });

            }
        }

        // GET api/<TareaController>/5
        [HttpGet("GetTaskById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _tareaService.GetTareaByIdAsync(id);
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
                    Message = "An unexpected error has ocurred",
                    Details = ex.Message
                });
            }
        }

        // POST api/<TareaController>
        [HttpPost("AddTask")]
        public async Task<IActionResult> Post([FromBody] TareaAddDto tareaAddDto)
        {
            try
            {
                var result = await _tareaService.CreateTareaAsync(tareaAddDto);

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
                    Message = "An unexpected error has ocurred",
                    Details = ex.Message
                });
            }

        }

        // PUT api/<TareaController>/5
        [HttpPut("UpdateCategoriaById/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TareaUpdateDto tareaUpdateDto)
        {
            try
            {
                if (id != tareaUpdateDto.Id)
                {
                    return BadRequest(new { Message = "The task id no match." });
                }
                var result = await _tareaService.UpdateTareaAsync(tareaUpdateDto);
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
                    Message = "An unexpected error has ocurred",
                    Details = ex.Message
                });
            }
        }

        // DELETE api/<TareaController>/5

        [HttpDelete("DeleteTaskById/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _tareaService.DeleteTareaAsync(id);
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
                    Message = "An unexpected error has ocurred",
                    Details = ex.Message
                });
            }
        }




        // GETBYDATE api/<TareaController>/2025-07-22

        [HttpGet("FindByDueDate/{date}")]
        public async Task<IActionResult> FindByDueDate(DateOnly date)
        {
            try
            {
                var result = await _tareaService.FindByDueDateAsync(date);
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
                    Message = "An unexpected error has ocurred",
                    Details = ex.Message
                });
            }
        }



        // GETSTATUS api/<TareaController>/Pendiente

        [HttpGet("FindByStatus/{status}")]
        public async Task<IActionResult> FindByStatus(string status)
        {
            try
            {
                var result = await _tareaService.FindByStatusAsync(status);
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
                    Message = "An unexpected error has ocurred",
                    Details = ex.Message
                });
            }
        }

        //[HttpGet("test-error")]
        //public async Task<IActionResult> TestError()
        //{
        //    throw new InvalidOperationException("Exception of test");
        //}


    }
}
