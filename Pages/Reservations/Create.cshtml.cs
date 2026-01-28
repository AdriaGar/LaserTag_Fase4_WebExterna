using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fase4_WebExterna.Models;
using Microsoft.EntityFrameworkCore;

namespace Fase4_WebExterna.Pages.Reservations
{
    public class CreateModel : PageModel
    {
        private readonly LaserTagContext _context;

        public CreateModel(LaserTagContext context)
        {
            _context = context;
            HoresDisponibles = new List<string>();
        }

        public List<Partide> Partides { get; set; }

        [BindProperty]
        public int IdPartidaSeleccionada { get; set; }

        [BindProperty]
        public string DataSeleccionada { get; set; }

        [BindProperty]
        public string HoraSeleccionada { get; set; }

        public List<string> HoresDisponibles { get; set; }


        // 🔵 Método para regenerar horas (lo usaremos en GET y POST)
        private void GenerarHores()
        {
            HoresDisponibles = new List<string>();
            for (int hora = 9; hora <= 21; hora++)
            {
                HoresDisponibles.Add($"{hora:00}:00");
            }
        }

        public void OnGet()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                Response.Redirect("/Account/Login");
                return;
            }

            Partides = _context.Partides.ToList();
            GenerarHores();
        }

        public IActionResult OnPost()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (email == null)
                return RedirectToPage("/Account/Login");

            // Regenerar horas en POST
            GenerarHores();

            // Validar fecha
            if (string.IsNullOrEmpty(DataSeleccionada))
            {
                ModelState.AddModelError("", "Has de seleccionar una data.");
                return Page();
            }

            // Validar hora
            if (string.IsNullOrEmpty(HoraSeleccionada))
            {
                ModelState.AddModelError("", "Has de seleccionar una hora.");
                return Page();
            }

            DateTime data = DateTime.Parse($"{DataSeleccionada} {HoraSeleccionada}");

            if (data < DateTime.Now)
            {
                ModelState.AddModelError("", "No pots reservar un dia/hora passats.");
                return Page();
            }

            // 🔒 1️⃣ VALIDAR SI YA EXISTE UNA PARTIDA A ESA HORA
            string dataIniciText = data.ToString("yyyy-MM-dd HH:mm");

            var partidaExistente = _context.Partides
                .FirstOrDefault(p => p.DataInici == dataIniciText);

            if (partidaExistente != null)
            {
                ModelState.AddModelError("", "Ja existeix una partida reservada en aquesta data i hora.");
                return Page();
            }

            // 2️⃣ CREAR PARTIDA
            var novaPartida = new Partide
            {
                DataInici = dataIniciText,
                DataFi = data.AddHours(1).ToString("yyyy-MM-dd HH:mm"),
                ModeJoc = "Estàndard"
            };

            _context.Partides.Add(novaPartida);
            _context.SaveChanges();

            // 3️⃣ CREAR RESERVA
            var reserva = new Reserf
            {
                EmailJugador = email,
                IdPartida = novaPartida.IdPartida,
                DataReserva = DataSeleccionada,
                HoraReserva = HoraSeleccionada,
                EstatReserva = "Activa"
            };

            _context.Reserves.Add(reserva);
            _context.SaveChanges();

            return RedirectToPage("/Reservations/Success");
        }

    }
}
