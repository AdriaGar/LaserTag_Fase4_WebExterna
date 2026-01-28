using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fase4_WebExterna.Models;

public partial class Partide
{
    [Key]
    public int IdPartida { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string DataInici { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? DataFi { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ModeJoc { get; set; } = null!;

    [InverseProperty("IdPartidaNavigation")]
    public virtual ICollection<Puntuacio> Puntuacios { get; set; } = new List<Puntuacio>();

    [InverseProperty("IdPartidaNavigation")]
    public virtual ICollection<Reserf> Reserves { get; set; } = new List<Reserf>();
}
