namespace ProyectoU2025.Models.ViewModels
{
    public class SalonViewModel
    {
        // Horario de inicio de la clase
        public TimeSpan? HorarioInicio { get; set; }

        // Horario de fin de la clase
        public TimeSpan? HorarioFin { get; set; }

        // Fecha de inicio de la clase
        public DateTime? FechaInicio { get; set; }

        // Fecha de fin de la clase
        public DateTime? FechaFin { get; set; }

        // Código del salón
        public string SalonCodigo { get; set; }

        public string SalonNombre { get; set; }

        // Nombre del edificio
        public string EdificioNombre { get; set; }

        // Nombre de la sede
        public string SedeNombre { get; set; }

        // Nombre de la asignatura
        public string AsignaturaNombre { get; set; }

        // Nombre del docente
        public string DocenteNombre { get; set; }

        // Fecha de inicio del cambio de aula (si existe)
        public DateTime? CambioFechaInicio { get; set; }

        // Fecha de fin del cambio de aula (si existe)
        public DateTime? CambioFechaFin { get; set; }

        // Motivo del cambio de aula (si existe)
        public string CambioMotivo { get; set; }

        // Nuevo código del salón en caso de cambio de aula (si existe)
        public string NuevoSalonID { get; set; }

        public string RutaEdificio { get; set; }
        public string Dia { get; set; }                
        public TimeSpan? HoraInicio { get; set; }      
        public TimeSpan? HoraFin { get; set; }
        public string? NuevoSalonNombre { get; set; }
        public string? NuevoBloqueNombre { get; set; }
        public string? NuevoEdificioNombre { get; set; }
        public string? NuevaSedeNombre { get; set; }
        public string SalonImagenUrl { get; set; }
        public string EdificioImagenUrl { get; set; }
    }
}
