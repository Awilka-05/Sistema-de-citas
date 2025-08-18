using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Application.DTOs.Responses;
using SistemaGestionCitas.Application.Services;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaGestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitaController : ControllerBase
    {
        private readonly ICitaService _citaService;
        private readonly ICancelarCitaService _cancelarCitaService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IReservarCitaService _reservarCitaService;
        private readonly ILogger<CitaController> _logger;

        public CitaController(ICitaService citaService, ICancelarCitaService cancelarCitaService, IUsuarioRepository usuarioRepository, IReservarCitaService reservarCitaService,
            ILogger<CitaController> logger)
        {
            _citaService = citaService;
            _cancelarCitaService = cancelarCitaService;
            _usuarioRepository = usuarioRepository;
            _reservarCitaService = reservarCitaService;
            _logger = logger;
        }
        // GET: api/<CitaController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaResponseDto>>> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _citaService.GetAllAsync();

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }

            var response = result.Value.Adapt<IEnumerable<CitaResponseDto>>();
            return Ok(response);
        }

        // GET api/<CitaController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CitaResponseDto>> GetById(short id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _citaService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return NotFound(ModelState);
            }

            var response = result.Value.Adapt<CitaResponseDto>();
            return Ok(response);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<CitaResponseDto>>> GetByUsuario(int usuarioId)
        {
            var result = await _citaService.GetByUsuarioAsync(usuarioId);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }

            var response = result.Value.Adapt<IEnumerable<CitaResponseDto>>();
            return Ok(response);
        }

        // POST api/<CitaController>
        [HttpPost]
        public async Task<IActionResult> ReservarCita([FromBody] CrearCitaDto crearCitaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _usuarioRepository.GetByIdAsync(crearCitaDto.IdUsuario);
            if (usuario == null)
            {
                return BadRequest(new { Error = "El usuario proporcionado no existe." });
            }

            // 3. Mapear el DTO a la entidad Cita antes de llamar al servicio
            var cita = new Cita
            {
                IdUsuario = crearCitaDto.IdUsuario,
                FechaCita = crearCitaDto.FechaCita,
                TurnoId = crearCitaDto.TurnoId,
                FranjaId = crearCitaDto.FranjaId, 
                LugarId = crearCitaDto.LugarId,
                ServicioId = crearCitaDto.ServicioId
            };

            var result = await _reservarCitaService.ReservarCitaAsync(cita);

            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error });
            }

            var response = result.Value.Adapt<CitaResponseDto>();
            return CreatedAtAction(nameof(GetById), new { id = response.IdCita }, response);
        }

        // PUT api/<CitaController>/5
        [HttpPut("{id}/cancelar")]
        public async Task<ActionResult<CancelarCitaResponseDto>> CancelarCita([FromBody] CancelarCitaDto cancelarCitaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _usuarioRepository.GetByIdAsync(cancelarCitaDto.IdUsuario);
            if (usuario == null)
            {
                return BadRequest(new { Error = "El usuario proporcionado no existe." });
            }

            var result = await _cancelarCitaService.CancelarCitaAsync(cancelarCitaDto.IdCita, usuario);

            if (result.IsFailure)
            {
                if (result.Error.Contains("no existe"))
                    return NotFound(result.Error);

                ModelState.AddModelError("Error", result.Error);
                return BadRequest(ModelState);
            }
            var response = new CancelarCitaResponseDto
            {
                Mensaje = $"La cita con ID '{cancelarCitaDto.IdCita}' ha sido cancelada exitosamente."
            };
            return Ok(response);
        }

        //Este no es necesario 
        // DELETE api/<CitaController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
