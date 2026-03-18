using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace Fase4_WebExterna.Controllers
{
    [ApiController]
    [Route("api/puntuaciones")]
    public class PuntuacionesController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public PuntuacionesController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPuntuaciones(string id)
        {
            var url = $"http://192.168.0.100:3000/partida/estadistiques?id={id}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500, "Error obteniendo puntuaciones");
            }

            var json = await response.Content.ReadAsStringAsync();

            return Content(json, "application/json");
        }
    }
}