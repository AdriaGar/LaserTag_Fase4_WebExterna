using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fase4_WebExterna.Models;
using Microsoft.EntityFrameworkCore;

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
        }

        public IActionResult OnPostCancel(int idReserva)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
                return RedirectToPage("/Account/Login");

            // Buscar reserva
            var reserva = _context.Reserves.FirstOrDefault(r => r.IdReserva == idReserva && r.EmailJugador == email);
            if (reserva == null)
                return RedirectToPage(); // no existe o no pertenece al usuario

            // Buscar partida asociada
            var partida = _context.Partides.FirstOrDefault(p => p.IdPartida == reserva.IdPartida);

            // ELIMINAR primero reserva
            _context.Reserves.Remove(reserva);

            // ELIMINAR también la partida
            if (partida != null)
                _context.Partides.Remove(partida);

            _context.SaveChanges();

            return RedirectToPage(); // refresca el perfil
        }
    }
}
