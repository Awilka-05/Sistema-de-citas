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
    public class HorarioController : ControllerBase
    {
        private readonly IHorarioService _horarioService;

        public HorarioController(IHorarioService horarioService)
        {
            _horarioService = horarioService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(short id)
        {
            var result = await _horarioService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return NotFound(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _horarioService.GetAllAsync();
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<HorarioResponseDto>> Add([FromBody] HorarioDto dto)
        {
            var horario = dto.Adapt<Horario>();

            var result = await _horarioService.AddAsync(horario);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }
            var response = result.Value.Adapt<HorarioResponseDto>();

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<HorarioResponseDto>> Update(short id, [FromBody] HorarioDto dto)
        {
            var lugar = dto.Adapt<Horario>();
            lugar.HorarioId = id;

            var result = await _horarioService.UpdateAsync(lugar);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }

            var response = result.Value.Adapt<HorarioResponseDto>();
            return Ok(response);
        }
    }
}
