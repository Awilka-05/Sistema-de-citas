using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Application.DTOs.Responses;
using SistemaGestionCitas.Application.Services;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Domain.Value_Objects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaGestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRegistrarUsuario _registrarUsuarioService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(
            IUsuarioService usuarioService, 
            IRegistrarUsuario registrarUsuarioService, 
            IUsuarioRepository usuarioRepository,
            ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _registrarUsuarioService = registrarUsuarioService;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        // GET: api/<UsuarioController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarios = await _usuarioService.GetAllAsync();
            var response = usuarios.Adapt<IEnumerable<UsuarioResponseDto>>();
            return Ok(response);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _usuarioService.GetByIdAsync(id);

            if (usuario == null)
            {
                return NotFound(new { Error = "Usuario no encontrado." });
            }

            var response = usuario.Adapt<UsuarioResponseDto>();
            return Ok(response);
        }

        // GET api/<UsuarioController>/correo/{correo}
        [HttpGet("correo/{correo}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetByCorreo(string correo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _usuarioRepository.GetByCorreoAsync(correo);

            if (usuario == null)
            {
                return NotFound(new { Error = "Usuario no encontrado." });
            }

            var response = usuario.Adapt<UsuarioResponseDto>();
            return Ok(response);
        }

        // GET api/<UsuarioController>/cedula/{cedula}
        [HttpGet("cedula/{cedula}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetByCedula(string cedula)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _usuarioRepository.GetByNCedulaAsync(cedula);

            if (usuario == null)
            {
                return NotFound(new { Error = "Usuario no encontrado." });
            }

            var response = usuario.Adapt<UsuarioResponseDto>();
            return Ok(response);
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario([FromBody] CrearUsuarioDto crearUsuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si ya existe un usuario con la misma cédula
            if (await _usuarioRepository.ExisteCedulaAsync(crearUsuarioDto.Cedula))
            {
                return BadRequest(new { Error = "Ya existe un usuario con esta cédula." });
            }

            // Verificar si ya existe un usuario con el mismo correo
            if (await _usuarioRepository.ExisteCorreoAsync(crearUsuarioDto.Correo))
            {
                return BadRequest(new { Error = "Ya existe un usuario con este correo electrónico." });
            }

            // Mapear el DTO a la entidad Usuario
            var usuario = new Usuario
            {
                Nombre = new Nombre(crearUsuarioDto.Nombre),
                Cedula = new Cedula(crearUsuarioDto.Cedula),
                Correo = new Correo(crearUsuarioDto.Correo),
                Contrasena = crearUsuarioDto.Contrasena,
                Telefono = crearUsuarioDto.Telefono,
                // Agregar otros campos según sea necesario
            };

            try
            {
                var usuarioCreado = await _registrarUsuarioService.RegistrarUsuarioAsync(usuario);
                var response = usuarioCreado.Adapt<UsuarioResponseDto>();
                return CreatedAtAction(nameof(GetById), new { id = response.IdUsuario }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                return BadRequest(new { Error = "Error al registrar el usuario." });
            }
        }

        // POST api/<UsuarioController>/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioResponseDto>> Login([FromBody] LoginUsuarioDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _usuarioRepository.GetByCorreoAndPasswordAsync(loginDto.Correo, loginDto.Contrasena);

            if (usuario == null)
            {
                return Unauthorized(new { Error = "Credenciales inválidas." });
            }

            var response = usuario.Adapt<UsuarioResponseDto>();
            return Ok(response);
        }

        // GET api/<UsuarioController>/existe/cedula/{cedula}
        [HttpGet("existe/cedula/{cedula}")]
        public async Task<ActionResult<bool>> ExisteCedula(string cedula)
        {
            var existe = await _usuarioRepository.ExisteCedulaAsync(cedula);
            return Ok(existe);
        }

        // GET api/<UsuarioController>/existe/correo/{correo}
        [HttpGet("existe/correo/{correo}")]
        public async Task<ActionResult<bool>> ExisteCorreo(string correo)
        {
            var existe = await _usuarioRepository.ExisteCorreoAsync(correo);
            return Ok(existe);
        }
    }
}