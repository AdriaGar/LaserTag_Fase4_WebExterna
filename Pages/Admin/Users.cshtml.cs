using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fase4_WebExterna.Models;

namespace Fase4_WebExterna.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly LaserTagContext _context;

        public UsersModel(LaserTagContext context)
        {
            _context = context;
        }

        public List<Jugador> Usuarios { get; set; } = new();

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Account/Login");
            }

            Usuarios = _context.Jugadors.ToList();

            return Page();
        }

        // ❌ ELIMINAR USUARIO
        public IActionResult OnPostDelete(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Account/Login");
            }

            var user = _context.Jugadors.FirstOrDefault(u => u.IdJugador == id);

            if (user != null)
            {
                // 🔥 borrar reservas del usuario
                var reservas = _context.Reserves
                    .Where(r => r.EmailJugador == user.Email)
                    .ToList();

                _context.Reserves.RemoveRange(reservas);

                // 🔥 borrar partidas asociadas (simple)
                var partidasIds = reservas.Select(r => r.IdPartida).ToList();

                var partidas = _context.Partides
                    .Where(p => partidasIds.Contains(p.IdPartida))
                    .ToList();

                _context.Partides.RemoveRange(partidas);

                // 🔥 borrar usuario
                _context.Jugadors.Remove(user);

                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostToggleRole(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Account/Login");
            }

            var user = _context.Jugadors.FirstOrDefault(u => u.IdJugador == id);

            if (user != null)
            {
                var currentEmail = User.Identity.Name;

                if (user.Email == currentEmail)
                {
                    return RedirectToPage();
                }

                user.Rol = user.Rol == "Admin" ? "User" : "Admin";

                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}