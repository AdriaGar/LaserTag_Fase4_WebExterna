using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fase4_WebExterna.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}