using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.ViewModels;

namespace Planets.Models
{
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PlanetProperty> PlanetProperties { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanetProperty>()
                .HasKey(pp => new { pp.PlanetId, pp.PropertyId });

            modelBuilder.Entity<PlanetProperty>()
                .HasOne(pp => pp.Planet)
                .WithMany(p => p.PlanetProperties)
                .HasForeignKey(pp => pp.PlanetId);

            modelBuilder.Entity<PlanetProperty>()
                .HasOne(pp => pp.Property)
                .WithMany(p => p.PlanetProperties)
                .HasForeignKey(pp => pp.PropertyId);
        }
    }
}
