
using System.ComponentModel.DataAnnotations; 

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class LugarDto
    {
        [Required (ErrorMessage = "El nombre del lugar es requerido")]
        [StringLength(100, ErrorMessage = "El nombre del lugar no puede exceder los 100 caracteres")]
        public int Nombre { get; set; }
    }
}
