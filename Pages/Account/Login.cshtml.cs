using BCrypt.Net;
using Fase4_WebExterna.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Fase4_WebExterna.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly LaserTagContext _context;

        public LoginModel(LaserTagContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public string ErrorMessage { get; set; }

        public class LoginInputModel
        {
            public string Email { get; set; }
            public string Contrasenya { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Buscar usuario por email
            var usuari = _context.Jugadors.FirstOrDefault(j => j.Email == Input.Email);

            if (usuari == null)
            {
                ErrorMessage = "Email incorrecto.";
                return Page();
            }

            // Verificar contraseña
            bool valid = BCrypt.Net.BCrypt.Verify(Input.Contrasenya, usuari.ContrasenyaHash);

            if (!valid)
            {
                ErrorMessage = "Contraseña incorrecta.";
                return Page();
            }

            // ? SESSION (lo que ya tenías)
            HttpContext.Session.SetString("UserEmail", usuari.Email);
            HttpContext.Session.SetString("UserName", usuari.Nom);

            // ? AUTHENTICATION (LO NUEVO)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuari.Nom),
                new Claim(ClaimTypes.Email, usuari.Email),
                new Claim(ClaimTypes.NameIdentifier, usuari.IdJugador.ToString())
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToPage("/Index");
        }
    }
}