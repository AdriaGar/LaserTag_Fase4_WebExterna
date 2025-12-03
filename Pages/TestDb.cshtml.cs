using Microsoft.AspNetCore.Mvc.RazorPages;
using Fase4_WebExterna.Models;

public class TestDbModel : PageModel
{
    private readonly LaserTagContext _context;

    public int TotalJugadors { get; set; }

    public TestDbModel(LaserTagContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        TotalJugadors = _context.Jugadors.Count();
    }
}
