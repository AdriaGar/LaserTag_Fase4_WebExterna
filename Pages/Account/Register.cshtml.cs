using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fase4_WebExterna.Models;
using System.ComponentModel.DataAnnotations;
using BCrypt.Net;

public class RegisterModel : PageModel
{
    private readonly LaserTagContext _context;

    public RegisterModel(LaserTagContext context)
    {
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        public string Nom { get; set; }

        public string Cognoms { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Contrasenya { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Contrasenya", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasenya { get; set; }
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        // ¿Email ya existe?
        if (_context.Jugadors.Any(j => j.Email == Input.Email))
        {
            ModelState.AddModelError("Input.Email", "Este email ya está registrado");
            return Page();
        }

        // Hash de la contraseña
        string hash = BCrypt.Net.BCrypt.HashPassword(Input.Contrasenya);

        var nou = new Jugador
        {
            Nom = Input.Nom,
            Cognoms = Input.Cognoms,
            Email = Input.Email,
            ContrasenyaHash = hash,
            DataRegistre = DateTime.Now.ToString("yyyy-MM-dd")
        };

        _context.Jugadors.Add(nou);
        _context.SaveChanges();

        return RedirectToPage("/Account/Login"); // lo crearemos luego
    }
}
