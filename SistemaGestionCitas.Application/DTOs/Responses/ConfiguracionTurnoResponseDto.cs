using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Application.DTOs.Responses
{
    public class ConfiguracionTurnoResponseDto
    {
        public int TurnoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public short HorariosId { get; set; }
        public int CantidadEstaciones { get; set; }
        public int DuracionMinutos { get; set; }
        public bool AunAceptaCitas { get; set; }
        public HorarioResponseDto Horario { get; set; } = null!;
    }
}
