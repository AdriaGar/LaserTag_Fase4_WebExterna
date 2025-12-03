using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fase4_WebExterna.Models;

public partial class Equip
{
    [Key]
    public int IdEquip { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nom { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Color { get; set; }

    [ForeignKey("IdEquip")]
    [InverseProperty("IdEquips")]
    public virtual ICollection<Jugador> IdJugadors { get; set; } = new List<Jugador>();
}
