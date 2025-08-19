using Mapster;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Application.DTOs.Responses;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaGestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LugarController : ControllerBase
    {
        private readonly ILugarService _lugarService;
        private readonly ILogger<LugarController> _logger;

        public LugarController(ILugarService lugarService, ILogger<LugarController> logger)
        {
            _lugarService = lugarService;
            _logger = logger;
        }

        // GET: api/<LugarController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LugarResponseDto>>> GetAll()
        {
            var result = await _lugarService.GetAllAsync();

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }

            var response = result.Value.Adapt<IEnumerable<LugarResponseDto>>();
            return Ok(response);
        }

        // GET api/<LugarController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LugarResponseDto>> GetById(short id)
        {
            var result = await _lugarService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return NotFound(ModelState);
            }

            var response = result.Value.Adapt<LugarResponseDto>();
            return Ok(response);
        }

        // POST api/<LugarController>
        [HttpPost]
        public async Task<ActionResult<LugarResponseDto>> Add([FromBody] LugarDto dto)
        {
            var lugar = dto.Adapt<Lugar>();

            var result = await _lugarService.AddAsync(lugar);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }
            var response = result.Value.Adapt<LugarResponseDto>();

            return Ok(response);
        }


        // PUT api/<LugarController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<LugarResponseDto>> Update(short id, [FromBody] LugarDto dto)
        {
            var lugar = dto.Adapt<Lugar>();
            lugar.LugarId = id;

            var result = await _lugarService.UpdateAsync(lugar);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }

            var response = result.Value.Adapt<LugarResponseDto>();
            return Ok(response);
        }
    }
}
