using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fase4_WebExterna.Models;

namespace Fase4_WebExterna.Pages.Partides
{
    public class ReservarModel : PageModel
    {
        private readonly LaserTagContext _context;

        public ReservarModel(LaserTagContext context)
        {
            _context = context;
        }

        public Partide Partida { get; set; }

        public IActionResult OnGet(int id)
        {
            // 1. Usuari logat?
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
                return RedirectToPage("/Account/Login");

            // 2. Comprovar partida
            Partida = _context.Partides.FirstOrDefault(p => p.IdPartida == id);

            if (Partida == null)
                return RedirectToPage("/Partides/Index");

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
                return RedirectToPage("/Account/Login");

            var partida = _context.Partides.FirstOrDefault(p => p.IdPartida == id);
            if (partida == null)
                return RedirectToPage("/Partides/Index");

            // Crear reserva
            var reserva = new Reserf
            {
                EmailJugador = email,
                IdPartida = id,
                DataReserva = DateTime.Now.ToString("yyyy-MM-dd"),
                EstatReserva = "Activa"
            };

            _context.Reserves.Add(reserva);
            _context.SaveChanges();

            return RedirectToPage("/Partides/ReservaConfirmada");
        }
    }
}
