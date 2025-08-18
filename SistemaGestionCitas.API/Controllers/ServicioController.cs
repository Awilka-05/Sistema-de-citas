using Mapster;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Application.DTOs.Responses;
using SistemaGestionCitas.Application.Services;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaGestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _servicioService;
        private readonly ILogger<ServicioController> _logger;

        public ServicioController(IServicioService servicioService, ILogger<ServicioController> logger)
        {
            _servicioService = servicioService;
            _logger = logger;
        }

        // GET: api/<ServicioController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioResponseDto>>> GetAll()
        {
            var result = await _servicioService.GetAllAsync();

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }

            var response = result.Value.Adapt<IEnumerable<ServicioResponseDto>>();
            return Ok(response);
        }

        // GET api/<ServicioController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioResponseDto>> GetById(short id)
        {
            var result = await _servicioService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return NotFound(ModelState);
            }

            var response = result.Value.Adapt<ServicioResponseDto>();
            return Ok(response);
        }

        // POST api/<ServicioController>
        [HttpPost]
        public async Task<ActionResult<ServicioResponseDto>> Add([FromBody] ServicioDto dto)
        {
            var servicio = dto.Adapt<Servicio>();

            var result = await _servicioService.AddAsync(servicio);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }
            var response = result.Value.Adapt<ServicioResponseDto>();

            return Ok(response);
        }
        // PUT api/<ServicioController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ServicioResponseDto>> Update(short id, [FromBody] ServicioDto dto)
        {
            var servicio = dto.Adapt<Servicio>();
            servicio.ServicioId = id;

            var result = await _servicioService.UpdateAsync(servicio);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }

            var response = result.Value.Adapt<LugarResponseDto>();
            return Ok(response);
        }

        // DELETE api/<ServicioController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(short id)
        {
            var result = await _servicioService.DeleteAsync(id);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }

            return Ok("Servicio eliminado con éxito");
        }
    }
}
