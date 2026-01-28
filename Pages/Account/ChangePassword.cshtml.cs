using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fase4_WebExterna.Models;
using BCrypt.Net;

namespace Fase4_WebExterna.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly LaserTagContext _context;

        public ChangePasswordModel(LaserTagContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ChangePasswordInputModel Input { get; set; }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public class ChangePasswordInputModel
        {
            public string ContraseñaActual { get; set; }
            public string NuevaContraseña { get; set; }
            public string ConfirmarContraseña { get; set; }
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                Response.Redirect("/Account/Login");
            }
        }

        public IActionResult OnPost()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (email == null)
                return RedirectToPage("/Account/Login");

            var jugador = _context.Jugadors.FirstOrDefault(j => j.Email == email);

            if (jugador == null)
            {
                ErrorMessage = "Error interno. Usuario no encontrado.";
                return Page();
            }

            // 1. Validar contraseña actual
            bool valid = BCrypt.Net.BCrypt.Verify(Input.ContraseñaActual, jugador.ContrasenyaHash);

            if (!valid)
            {
                ErrorMessage = "La contraseña actual es incorrecta.";
                return Page();
            }

            // 2. Validar coincidencia nueva contraseña
            if (Input.NuevaContraseña != Input.ConfirmarContraseña)
            {
                ErrorMessage = "La nueva contraseña no coincide con la confirmación.";
                return Page();
            }

            // 3. Generar nuevo hash
            string nuevoHash = BCrypt.Net.BCrypt.HashPassword(Input.NuevaContraseña);

            // 4. Guardar cambios
            jugador.ContrasenyaHash = nuevoHash;
            _context.SaveChanges();

            SuccessMessage = "Contraseña cambiada exitosamente.";
            return Page();
        }
    }
}
