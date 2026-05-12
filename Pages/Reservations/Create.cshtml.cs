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

        // NUEVO
        [BindProperty]
        public string SalaSeleccionada { get; set; }

        [BindProperty]
        public string ModeJocSeleccionat { get; set; }

        public List<string> HoresDisponibles { get; set; }

        // GENERAR HORAS

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

            // REGENERAR HORAS

            GenerarHores();

            // VALIDAR FECHA

            if (string.IsNullOrEmpty(DataSeleccionada))
            {
                ModelState.AddModelError("", "Has de seleccionar una data.");
                return Page();
            }

            // VALIDAR HORA

            if (string.IsNullOrEmpty(HoraSeleccionada))
            {
                ModelState.AddModelError("", "Has de seleccionar una hora.");
                return Page();
            }

            // VALIDAR SALA

            if (string.IsNullOrEmpty(SalaSeleccionada))
            {
                ModelState.AddModelError("", "Has de seleccionar una sala.");
                return Page();
            }

            // VALIDAR MODO

            if (string.IsNullOrEmpty(ModeJocSeleccionat))
            {
                ModelState.AddModelError("", "Has de seleccionar un mode de joc.");
                return Page();
            }

            // VALIDAR FECHA PASADA

            DateTime data = DateTime.Parse($"{DataSeleccionada} {HoraSeleccionada}");

            if (data < DateTime.Now)
            {
                ModelState.AddModelError("", "No pots reservar un dia/hora passats.");
                return Page();
            }

            // VALIDAR SI LA SALA YA ESTÁ RESERVADA

            var reservaExistente = _context.Reserves
                .Include(r => r.IdPartidaNavigation)
                .FirstOrDefault(r =>
                    r.DataReserva == DataSeleccionada &&
                    r.HoraReserva == HoraSeleccionada &&
                    r.Sala == SalaSeleccionada &&
                    r.EstatReserva == "Activa"
                );

            if (reservaExistente != null)
            {
                ModelState.AddModelError("", "Aquesta sala ja està reservada en aquesta hora.");
                return Page();
            }

            // CREAR PARTIDA

            string dataIniciText = data.ToString("yyyy-MM-dd HH:mm");

            var novaPartida = new Partide
            {
                DataInici = dataIniciText,
                DataFi = data.AddHours(1).ToString("yyyy-MM-dd HH:mm"),
                ModeJoc = ModeJocSeleccionat
            };

            _context.Partides.Add(novaPartida);

            _context.SaveChanges();

            // CREAR RESERVA

            var reserva = new Reserf
            {
                EmailJugador = email,
                IdPartida = novaPartida.IdPartida,
                DataReserva = DataSeleccionada,
                HoraReserva = HoraSeleccionada,
                EstatReserva = "Activa",
                Sala = SalaSeleccionada
            };

            _context.Reserves.Add(reserva);

            _context.SaveChanges();

            return RedirectToPage("/Reservations/Success");
        }
    }
}