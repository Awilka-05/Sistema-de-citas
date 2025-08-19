using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly IRegistrarUsuario _registrarUsuario;
        private readonly ITokenProvider _tokenProvider;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper, IRegistrarUsuario registrarUsuario)
        {
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _registrarUsuario = registrarUsuario ;
        }

        /// <summary>
        /// Obtiene todos los usuarios "Porque aja que mas va traer. ATT: ALNA;)".
        /// </summary>
        /// <returns>Lista de usuarios.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetAllAsync()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllAsync();
                var usuarioDtos = _mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios);
                return Ok(usuarioDtos);
            }
            catch (Exception ex)
            {
                // registro sabroso para el error en un log
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener los usuarios.");
            }
        }

        /// <summary>
        /// Este va devolver un usuario por su ID. Confiando en papa Dios que no eplote🙏🙏🙏
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Si un usuario no es encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioResponseDto>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID debe ser un número positivo.");
            }

            try
            {
                var usuario = await _usuarioService.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound($"Usuario con ID {id} no encontrado.");
                }

                var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuario);
                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                // otro registro sabroso para el error en un log😏😏
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener el usuario.");
            }
        }

        /// <summary>
        /// Bueno este disque que crea un usuario, pero no se que va a hacer."LMAO es Joking"
        /// </summary>
        /// <param name="CreateUsuarioDto">Datos del usuario a crear</param>
        /// <returns>Usuario creado</returns>
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Registro")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpPost("Registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar([FromBody] CrearUsuarioDto dto)
        {
            var usuario = dto.Adapt<Usuario>(); // Mapear DTO a entidad
            var resultado = await _registrarUsuario.RegistrarUsuarioAsync(usuario);

            if (resultado.IsFailure)
                return BadRequest(new { mensaje = resultado.Error });

            return Ok(resultado.Value);
        }
        //public async Task<ActionResult<UsuarioResponseDto>> CreateAsync([FromBody] Application.DTOs.Requests.CrearUsuarioDto createUsuarioDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //        try
        //        {
        //            //Vamos a hacer un rico mapeo de DTO a entidad
        //            var usuario = _mapper.Map<Domain.Entities.CrearUsuarioDto>(createUsuarioDto);
        //            var usarioCreado = await _usuarioService.GetAllAsync();
        //            var usuarioDto = _mapper.Map<UsuarioResponseDto>(usarioCreado);
        //            return CreatedAtAction(
        //                nameof(GetByIdAsync),
        //                new { id = UsuarioResponseDto.IdUsuario },
        //                usuarioDto);
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //        catch (InvalidOperationException ex)
        //        {
        //            return Conflict(ex.Message); // Para casos como cédula duplicada
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log exception here
        //            return StatusCode(StatusCodes.Status500InternalServerError,
        //                "Error interno del servidor al crear usuario");
        //        }
        //    }

        /*  /// <summary>
          /// Actualiza un usuario existente
          /// </summary>
          /// <param name="id">ID del usuario</param>
          /// <param name="updateUsuarioDto">Datos actualizados del usuario</param>
          /// <returns>Usuario actualizado</returns>
          [HttpPut("{id:int}")]
          [ProducesResponseType(StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(StatusCodes.Status500InternalServerError)]
          public async Task<ActionResult<UsuarioDto>> UpdateAsync(int id, [FromBody] UpdateUsuarioDto updateUsuarioDto)
          {
              if (id <= 0)
                  return BadRequest("El ID debe ser mayor que cero");

              if (id != updateUsuarioDto.Id)
                  return BadRequest("El ID de la URL no coincide con el ID del objeto");

              if (!ModelState.IsValid)
                  return BadRequest(ModelState);

              try
              {
                  // Verificar si existe
                  var usuarioExistente = await _usuarioService.GetByIdAsync(id);
                  if (usuarioExistente == null)
                      return NotFound($"Usuario con ID {id} no encontrado");

                  // Mapear y actualizar
                  var usuario = _mapper.Map<Usuario>(updateUsuarioDto);
                  var usuarioActualizado = await _usuarioService.UpdateAsync(usuario);

                  var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuarioActualizado);
                  return Ok(usuarioDto);
              }
              catch (ArgumentException ex)
              {
                  return BadRequest(ex.Message);
              }
              catch (Exception ex)
              {
                  // Log exception here
                  return StatusCode(StatusCodes.Status500InternalServerError,
                      $"Error interno del servidor al actualizar usuario con ID {id}");
              }
          }

          // DELETE api/<UsuarioController>/5
          [HttpDelete("{id}")]
          public void Delete(int id)
          {
          }
      }*/
        //    return null;
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Login")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<UsuarioResponseDto>> CreateAsync([FromBody] Application.DTOs.Requests.CrearUsuarioDto createUsuarioDto)
        //{
        //}

        [HttpPost("Login")]
        [AllowAnonymous] // Este endpoint no requiere autenticación
        public async Task<IActionResult> Login([FromBody] LoginUsuarioDto loginDto)
        {
           
            var usuario = await _usuarioService.GetByCorreoAndPasswordAsync(loginDto.Correo, loginDto.Contrasena);

            // 2. Si el usuario no es encontrado, retorna un error
            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." });
            }

            // 3. Si las credenciales son correctas, genera el token JWT
           // var token = _tokenProvider.Create(usuario);

            // 2. Retornas el token en la respuesta
            return Ok();
        }

    }
}