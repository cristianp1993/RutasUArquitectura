using ProyectoU2025.Models;
using System.Net.Http.Headers;

namespace ProyectoU2025.Services
{
    // DeepSeekService.cs
    public class DeepSeekService
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiKey;

        public DeepSeekService(IConfiguration config, HttpClient httpClient)
        {
            _httpClient = new HttpClient();
            _apiKey = config["DeepSeek:ApiKey"];
            _httpClient.BaseAddress = new Uri(config["DeepSeek:Endpoint"]);
        }

        public async Task<string> GetRespuestaAsync(string contexto, string preguntaUsuario)
        {
            var request = new
            {
                model = "deepseek-v3",
                messages = new[] {
                new { role = "system", content = contexto },
                new { role = "user", content = preguntaUsuario }
            }
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            var response = await _httpClient.PostAsJsonAsync("", request);
            var result = await response.Content.ReadFromJsonAsync<DeepSeekResponseModel>();
            return result.Choices[0].Message.Content;
        }
    }
}
