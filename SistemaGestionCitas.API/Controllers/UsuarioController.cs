using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Application.DTOs.Responses;
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

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioResponseDto>> CreateAsync([FromBody] CrearUsuarioDto createUsuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                try
                {
                    //Vamos a hacer un rico mapeo de DTO a entidad
                    var usuario = _mapper.Map<Usuario>(createUsuarioDto);
                    var usarioCreado = await _usuarioService.GetAllAsync();
                    var usuarioDto = _mapper.Map<UsuarioResponseDto>(usarioCreado);
                    return CreatedAtAction(
                        nameof(GetByIdAsync),
                        new { id = UsuarioResponseDto.IdUsuario },
                        usuarioDto);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    return Conflict(ex.Message); // Para casos como cédula duplicada
                }
                catch (Exception ex)
                {
                    // Log exception here
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error interno del servidor al crear usuario");
                }
            }

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
            return null;
        }
    }
}