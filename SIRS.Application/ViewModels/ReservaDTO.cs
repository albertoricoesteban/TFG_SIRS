﻿namespace SIRS.Application.ViewModels
{
    public class ReservaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Observaciones { get; set; }
        public string? NombreEdificio { get; set; }
        public string? NombreSala { get; set; }
        public DateTime FechaReserva  { get; set; }
        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }
        public bool? Aprobada { get; set; }
        public int EdificioId{ get; set; }
        public int SalaId { get; set; }
        public int TiempoTotal { get; set; }
        public int UsuarioId { get; set; }
    }
}
