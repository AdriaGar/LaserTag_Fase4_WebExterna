using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fase4_WebExterna.Models;

[Index("Email", Name = "UQ_Jugadors_Email", IsUnique = true)]
public partial class Jugador
{
    [Key]
    public int IdJugador { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nom { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? Cognoms { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string DataRegistre { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string ContrasenyaHash { get; set; } = null!;

    [InverseProperty("IdJugadorNavigation")]
    public virtual ICollection<Puntuacio> Puntuacios { get; set; } = new List<Puntuacio>();

    [InverseProperty("EmailJugadorNavigation")]
    public virtual ICollection<Reserf> Reserves { get; set; } = new List<Reserf>();

    [ForeignKey("IdJugador")]
    [InverseProperty("IdJugadors")]
    public virtual ICollection<Equip> IdEquips { get; set; } = new List<Equip>();
}
