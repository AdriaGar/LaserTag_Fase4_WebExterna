using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fase4_WebExterna.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Net.Http;

namespace Fase4_WebExterna.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly LaserTagContext _context;

        public ProfileModel(LaserTagContext ctx)
        {
            _context = ctx;
        }

        public Jugador Jugador { get; set; }
        public List<Reserf> Reserves { get; set; }

        // NUEVO → estadísticas del backend GO
        public PerfilJugadorStats Estadisticas { get; set; }

        public void OnGet()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                Response.Redirect("/Account/Login");
                return;
            }

            // Cargar jugador
            Jugador = _context.Jugadors.FirstOrDefault(j => j.Email == email);

            // Cargar reservas + partida asociada
            Reserves = _context.Reserves
                .Include(r => r.IdPartidaNavigation)
                .Where(r => r.EmailJugador == email && r.EstatReserva == "Activa")
                .ToList();

            // ==========================
            // Obtener estadísticas del backend GO
            // ==========================

            try
            {
                var client = new HttpClient();

                string jugadorId = "jugador_" + Jugador.IdJugador;

                var url = $"http://192.168.0.100:3000/jugador/{jugadorId}/perfil-complet";

                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;

                    Estadisticas = JsonSerializer.Deserialize<PerfilJugadorStats>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                }
            }
            catch
            {
                // Si falla el endpoint no rompe el perfil
                Estadisticas = null;
            }
        }

        public IActionResult OnPostCancel(int idReserva)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
                return RedirectToPage("/Account/Login");

            // Buscar reserva
            var reserva = _context.Reserves.FirstOrDefault(r => r.IdReserva == idReserva && r.EmailJugador == email);
            if (reserva == null)
                return RedirectToPage();

            // Buscar partida asociada
            var partida = _context.Partides.FirstOrDefault(p => p.IdPartida == reserva.IdPartida);

            // ELIMINAR primero reserva
            _context.Reserves.Remove(reserva);

            // ELIMINAR también la partida
            if (partida != null)
                _context.Partides.Remove(partida);

            _context.SaveChanges();

            return RedirectToPage();
        }
    }
}