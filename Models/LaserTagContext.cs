using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Fase4_WebExterna.Models;

public partial class LaserTagContext : DbContext
{
    public LaserTagContext()
    {
    }

    public LaserTagContext(DbContextOptions<LaserTagContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Equip> Equips { get; set; }

    public virtual DbSet<Jugador> Jugadors { get; set; }

    public virtual DbSet<Partide> Partides { get; set; }

    public virtual DbSet<Puntuacio> Puntuacios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LaserTagBD;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equip>(entity =>
        {
            entity.HasKey(e => e.IdEquip).HasName("PK__Equips__07DC4B1E6F73BB25");
        });

        modelBuilder.Entity<Jugador>(entity =>
        {
            entity.HasKey(e => e.IdJugador).HasName("PK__Jugadors__99E320169A61C12B");

            entity.Property(e => e.ContrasenyaHash).HasDefaultValue("");

            entity.HasMany(d => d.IdEquips).WithMany(p => p.IdJugadors)
                .UsingEntity<Dictionary<string, object>>(
                    "JugadorEquip",
                    r => r.HasOne<Equip>().WithMany()
                        .HasForeignKey("IdEquip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_JE_Equips"),
                    l => l.HasOne<Jugador>().WithMany()
                        .HasForeignKey("IdJugador")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_JE_Jugadors"),
                    j =>
                    {
                        j.HasKey("IdJugador", "IdEquip").HasName("PK_JugadorEquips");
                        j.ToTable("Jugador_Equips");
                    });
        });

        modelBuilder.Entity<Partide>(entity =>
        {
            entity.HasKey(e => e.IdPartida).HasName("PK__Partides__6ED660C79C30B303");
        });

        modelBuilder.Entity<Puntuacio>(entity =>
        {
            entity.HasKey(e => e.IdPuntuacio).HasName("PK__Puntuaci__A682A6EDFB770C44");

            entity.HasOne(d => d.IdJugadorNavigation).WithMany(p => p.Puntuacios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Puntuacio_Jugador");

            entity.HasOne(d => d.IdPartidaNavigation).WithMany(p => p.Puntuacios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Puntuacio_Partida");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
