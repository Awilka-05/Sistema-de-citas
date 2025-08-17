using Mapster;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAll()
        {
            var result = await _lugarService.GetAllAsync();
            return Ok(result);
        }

        // GET api/<LugarController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LugarResponseDto>> GetById(short id)
        {
            var result = await _lugarService.GetByIdAsync(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var response = result.Value.Adapt<LugarResponseDto>();
            return Ok(response);
        }

        // POST api/<LugarController>
        [HttpPost]
        public async Task<IActionResult> Add(Lugar lugar)
        {
            var result = await _lugarService.AddAsync(lugar);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        // PUT api/<LugarController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Lugar lugar)
        {
            if (id != lugar.LugarId) return BadRequest("ID no coincide.");
            var result = await _lugarService.UpdateAsync(lugar);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        // DELETE api/<LugarController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var result = await _lugarService.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
