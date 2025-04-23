using ProyectoU2025.Repositories.Interfaces;
using System.Text.Json;

namespace ProyectoU2025.Services
{
    // SalonService.cs
    public class SalonService
    {
        private readonly ISalonRepository _salonRepository;
        //private readonly DeepSeekService _deepSeekService;

        public SalonService(ISalonRepository salonRepository)
        {
            _salonRepository = salonRepository;
            //_deepSeekService = deepSeekService;
        }

        public async Task<string> ObtenerUbicacionSalonAsync(string tipo, string valorInput)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(valorInput))
                {
                    return JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = "El valor de búsqueda no puede estar vacío.",
                        data = (object)null
                    });
                }

                var salones = await _salonRepository.GetSalonesAsync(tipo, valorInput);
                if (salones == null || !salones.Any())
                {
                    return JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = "No se encontraron coincidencias.",
                        data = (object)null
                    });
                }

                var mensajes = salones.Select(salon =>
                {
                    var dia = string.IsNullOrWhiteSpace(salon.Dia) ? "día no definido" : salon.Dia;
                    var horaInicio = salon.HoraInicio?.ToString(@"hh\:mm") ?? "hora no definida";
                    var horaFin = salon.HoraFin?.ToString(@"hh\:mm") ?? "hora no definida";
                    var horario = $" El día {dia} de {horaInicio} a {horaFin}.";

                    var cambio = string.Empty;
                    if (salon.CambioFechaInicio.HasValue && salon.CambioFechaFin.HasValue)
                    {
                        var fechaInicio = salon.CambioFechaInicio.Value.ToString("yyyy-MM-dd");
                        var fechaFin = salon.CambioFechaFin.Value.ToString("yyyy-MM-dd");
                        var motivo = string.IsNullOrWhiteSpace(salon.CambioMotivo) ? "Motivo no especificado" : salon.CambioMotivo;

                        var nuevaUbicacion = string.IsNullOrWhiteSpace(salon.NuevoSalonNombre)
                            ? ""
                            : $" Las clases se trasladarán al salón {salon.NuevoSalonNombre}, ubicado en el edificio {salon.NuevoEdificioNombre}, bloque {salon.NuevoBloqueNombre}, sede {salon.NuevaSedeNombre}.";

                        cambio = $" ⚠️ Cambio de aula entre el {fechaInicio} y el {fechaFin}. Motivo: {motivo}.{nuevaUbicacion}";
                    }

                    var mensajeTexto = tipo.ToLower() switch
                    {
                        "docente" => $"El docente {salon.DocenteNombre} estará en el salón {salon.SalonNombre}, en el edificio {salon.EdificioNombre}, sede {salon.SedeNombre}, dictando la clase {salon.AsignaturaNombre}.{horario}{cambio}",
                        "asignatura" => $"La clase de {salon.AsignaturaNombre} será en el salón {salon.SalonNombre}, edificio {salon.EdificioNombre}, sede {salon.SedeNombre}.{horario}{cambio}",
                        "clase" => $"La clase con código {valorInput}, asignatura {salon.AsignaturaNombre}, se dictará en el salón {salon.SalonNombre}, edificio {salon.EdificioNombre}, sede {salon.SedeNombre}, con el profesor {salon.DocenteNombre}.{horario}{cambio}",
                        "salon" => $"El salón {salon.SalonNombre} está en el edificio {salon.EdificioNombre}, sede {salon.SedeNombre}, con clase a cargo de {salon.DocenteNombre}.{horario}{cambio}",
                        _ => "Tipo de búsqueda no reconocido."
                    };
                    return new
                    {
                        mensajes = mensajeTexto,
                        ruta = salon.RutaEdificio
                    };
                }).ToList();


                return JsonSerializer.Serialize(new
                {
                    success = true,
                    message = $"{mensajes.Count} resultado(s) encontrados.",
                    data = mensajes
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Error inesperado.",
                    data = (object)null,
                    errorDetails = ex.Message
                });
            }
        }

    }
}
