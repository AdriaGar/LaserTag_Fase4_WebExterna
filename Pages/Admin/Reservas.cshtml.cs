using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Fase4_WebExterna.Models;

namespace Fase4_WebExterna.Pages.Admin
{
    public class ReservasModel : PageModel
    {
        private readonly LaserTagContext _context;

        public ReservasModel(LaserTagContext context)
        {
            _context = context;
        }

        public List<Reserf> Reservas { get; set; } = new();

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Account/Login");
            }

            Reservas = _context.Reserves
                .Include(r => r.IdPartidaNavigation)
                .ToList();

            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Account/Login");
            }

            var reserva = _context.Reserves.FirstOrDefault(r => r.IdReserva == id);

            if (reserva != null)
            {
                var otrasReservas = _context.Reserves
                    .Where(r => r.IdPartida == reserva.IdPartida && r.IdReserva != reserva.IdReserva)
                    .ToList();

                _context.Reserves.Remove(reserva);

                if (!otrasReservas.Any())
                {
                    var partida = _context.Partides
                        .FirstOrDefault(p => p.IdPartida == reserva.IdPartida);

                    if (partida != null)
                    {
                        _context.Partides.Remove(partida);
                    }
                }

                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}