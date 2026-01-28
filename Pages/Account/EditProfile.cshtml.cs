using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fase4_WebExterna.Models;

namespace Fase4_WebExterna.Pages.Account
{
    public class EditProfileModel : PageModel
    {
        private readonly LaserTagContext _context;

        public EditProfileModel(LaserTagContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EditProfileInputModel Input { get; set; }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public class EditProfileInputModel
        {
            public string Nom { get; set; }
            public string Cognoms { get; set; }
        }

        public void OnGet()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (email == null)
            {
                Response.Redirect("/Account/Login");
                return;
            }

            var jugador = _context.Jugadors.FirstOrDefault(j => j.Email == email);

            Input = new EditProfileInputModel
            {
                Nom = jugador.Nom,
                Cognoms = jugador.Cognoms
            };
        }

        public IActionResult OnPost()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (email == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var jugador = _context.Jugadors.FirstOrDefault(j => j.Email == email);

            if (jugador == null)
            {
                ErrorMessage = "No se pudo cargar el usuario.";
                return Page();
            }

            // Guardar cambios
            jugador.Nom = Input.Nom;
            jugador.Cognoms = Input.Cognoms;
            _context.SaveChanges();

            // Actualitzar sessió (perquè el canvi es vegi al perfil)
            HttpContext.Session.SetString("UserName", Input.Nom);

            SuccessMessage = "Cambios guardados correctamente.";
            return Page();
        }
    }
}
