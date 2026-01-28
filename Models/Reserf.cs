using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fase4_WebExterna.Models;

public partial class Reserf
{
    [Key]
    public int IdReserva { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string EmailJugador { get; set; } = null!;

    public int IdPartida { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string DataReserva { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string EstatReserva { get; set; } = null!;

    [StringLength(5)]
    [Unicode(false)]
    public string HoraReserva { get; set; } = null!;

    [ForeignKey("EmailJugador")]
    [InverseProperty("Reserves")]
    public virtual Jugador EmailJugadorNavigation { get; set; } = null!;

    [ForeignKey("IdPartida")]
    [InverseProperty("Reserves")]
    public virtual Partide IdPartidaNavigation { get; set; } = null!;
}
