using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fase4_WebExterna.Models;

[Table("Puntuacio")]
public partial class Puntuacio
{
    [Key]
    public int IdPuntuacio { get; set; }

    public int IdJugador { get; set; }

    public int IdPartida { get; set; }

    public int Punts { get; set; }

    public int Kills { get; set; }

    public int Morts { get; set; }

    [ForeignKey("IdJugador")]
    [InverseProperty("Puntuacios")]
    public virtual Jugador IdJugadorNavigation { get; set; } = null!;

    [ForeignKey("IdPartida")]
    [InverseProperty("Puntuacios")]
    public virtual Partide IdPartidaNavigation { get; set; } = null!;
}
