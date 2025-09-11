using Microsoft.EntityFrameworkCore;
using JustBeeInfrastructure.Models;

namespace JustBeeInfrastructure.Context;

public class JustBeeContext : DbContext
{
    public JustBeeContext(DbContextOptions<JustBeeContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Alveole> Alveoles { get; set; }
    public DbSet<Ville> Villes { get; set; }
    public DbSet<Departement> Departements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Person entity
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Pseudo).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Email).IsRequired().HasMaxLength(255);
            entity.Property(p => p.VilleCode).HasMaxLength(10);
            entity.Property(p => p.TokenVerification).HasMaxLength(100);
            entity.Ignore(p => p.DepartementCode); // Ignore the compatibility property

            // Relationship with Ville
            entity.HasOne(p => p.Ville)
                  .WithMany(v => v.Persons)
                  .HasForeignKey(p => p.VilleCode)
                  .HasPrincipalKey(v => v.Code)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Alveole entity
        modelBuilder.Entity<Alveole>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Nom).IsRequired().HasMaxLength(200);
            entity.Property(a => a.Description).HasMaxLength(1000);
            entity.Property(a => a.VilleCode).IsRequired().HasMaxLength(10);
            entity.Property(a => a.Email).IsRequired().HasMaxLength(255);
            entity.Property(a => a.TokenVerification).HasMaxLength(100);

            // Relationship with Ville
            entity.HasOne(a => a.Ville)
                  .WithMany(v => v.Alveoles)
                  .HasForeignKey(a => a.VilleCode)
                  .HasPrincipalKey(v => v.Code)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Ville entity
        modelBuilder.Entity<Ville>(entity =>
        {
            entity.HasKey(v => v.Code);
            entity.Property(v => v.Code).HasMaxLength(10);
            entity.Property(v => v.Nom).IsRequired().HasMaxLength(200);
            entity.Property(v => v.Region).IsRequired().HasMaxLength(100);
            entity.Property(v => v.Departement).IsRequired().HasMaxLength(100);
        });

        // Configure Departement entity
        modelBuilder.Entity<Departement>(entity =>
        {
            entity.HasKey(d => d.Code);
            entity.Property(d => d.Code).HasMaxLength(10);
            entity.Property(d => d.Nom).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Region).IsRequired().HasMaxLength(100);
        });

        // Add indexes for better performance
        modelBuilder.Entity<Person>()
            .HasIndex(p => p.Email)
            .IsUnique();

        modelBuilder.Entity<Person>()
            .HasIndex(p => p.VilleCode);

        modelBuilder.Entity<Alveole>()
            .HasIndex(a => a.Email);

        modelBuilder.Entity<Alveole>()
            .HasIndex(a => a.VilleCode);

        modelBuilder.Entity<Alveole>()
            .HasIndex(a => a.TokenVerification);
    }
}