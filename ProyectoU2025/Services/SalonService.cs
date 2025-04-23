using ProyectoU2025.Repositories.Interfaces;

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
                // Validar que el valor de entrada no esté vacío
                if (string.IsNullOrWhiteSpace(valorInput))
                {
                    return System.Text.Json.JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = "El valor de búsqueda no puede estar vacío.",
                        data = (object)null
                    });
                }

                // Obtener el salón desde el repositorio
                var salon = await _salonRepository.GetSalonAsync(tipo, valorInput);
                if (salon == null)
                {
                    return System.Text.Json.JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = "No se pudo identificar la información con los datos proporcionados.",
                        data = (object)null
                    });
                }

                // Generar la respuesta según el tipo
                string response;

                switch (tipo.ToLower())
                {
                    case "docente":
                        response = $"El docente {salon.DocenteNombre} estará en el salón {salon.SalonCodigo}, que está en el edificio {salon.EdificioNombre}, en la sede {salon.SedeNombre}, y dictará la clase {salon.AsignaturaNombre}.";
                        break;

                    case "asignatura":
                        response = $"La clase de {salon.AsignaturaNombre} se dará en el salón {salon.SalonCodigo}, que está en el edificio {salon.EdificioNombre}, en la sede {salon.SedeNombre}.";
                        break;

                    case "clase":
                        response = $"La clase con código {valorInput} y asignatura {salon.AsignaturaNombre} se dará en el salón {salon.SalonCodigo}, que está en el edificio {salon.EdificioNombre}, en la sede {salon.SedeNombre}, y será dictada por el profesor {salon.DocenteNombre}.";
                        break;

                    case "salon":
                        response = $"El salón {salon.SalonCodigo} está en el edificio {salon.EdificioNombre}, en la sede {salon.SedeNombre}, y la clase la dictará el instructor {salon.DocenteNombre}.";
                        break;

                    default:
                        return System.Text.Json.JsonSerializer.Serialize(new
                        {
                            success = false,
                            message = "El tipo de búsqueda no es válido. Los valores permitidos son: 'docente', 'asignatura', 'clase' o 'salon'.",
                            data = (object)null
                        });
                }

                // Retornar la respuesta serializada
                return System.Text.Json.JsonSerializer.Serialize(new
                {
                    success = true,
                    message = response,
                    data = new
                    {
                        salon.SalonCodigo,
                        salon.EdificioNombre,
                        salon.SedeNombre,
                        salon.AsignaturaNombre,
                        salon.DocenteNombre,
                        salon.RutaEdificio
                    }
                });
            }
            catch (Exception ex)
            {
                // Capturar errores inesperados
                return System.Text.Json.JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "No se pudo realizar la petición. Ocurrió un error inesperado.",
                    data = (object)null,
                    errorDetails = ex.Message
                });
            }
        }
    }
}
