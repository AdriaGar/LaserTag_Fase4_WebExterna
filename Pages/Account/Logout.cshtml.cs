using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fase4_WebExterna.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // ?? CERRAR AUTENTICACIÓN (COOKIE)
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            // ?? LIMPIAR SESSION
            HttpContext.Session.Clear();

            // Volver al inicio
            return RedirectToPage("/Index");
        }
    }
}