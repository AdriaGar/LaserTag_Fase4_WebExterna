using Microsoft.AspNetCore.Mvc;
using Fase4_WebExterna.Models;
using BCrypt.Net;

namespace Fase4_WebExterna.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly LaserTagContext _context;

        public AuthController(LaserTagContext context)
        {
            _context = context;
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (_context.Jugadors.Any(j => j.Email == request.Email))
            {
                return BadRequest(new { message = "Email ya registrado" });
            }

            string hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var jugador = new Jugador
            {
                Nom = request.Nom,
                Cognoms = request.Cognoms,
                Email = request.Email,
                ContrasenyaHash = hash,
                DataRegistre = DateTime.Now.ToString("yyyy-MM-dd"),
                Rol = "User"
            };

            _context.Jugadors.Add(jugador);
            _context.SaveChanges();

            return Ok(new { message = "Usuario registrado correctamente" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            var user = _context.Jugadors.FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
                return Unauthorized(new { message = "Email incorrecto" });

            bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.ContrasenyaHash);

            if (!validPassword)
                return Unauthorized(new { message = "Contraseña incorrecta" });

            return Ok(new
            {
                message = "Login correcto",
                user = new
                {
                    id = user.IdJugador,
                    nom = user.Nom,
                    email = user.Email,
                    rol = user.Rol
                }
            });
        }
    }

    // =========================
    // REQUEST MODEL
    // =========================
    public class RegisterRequest
    {
        public string Nom { get; set; }
        public string Cognoms { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}