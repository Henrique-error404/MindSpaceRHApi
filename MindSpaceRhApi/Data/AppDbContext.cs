using Microsoft.EntityFrameworkCore;
using MindSpaceRhApi.Models;

namespace MindSpaceRhApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<WellnessMetric> WellnessMetrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração para garantir que o Nome do setor seja único
            modelBuilder.Entity<Department>()
                .HasIndex(d => d.Name)
                .IsUnique();
        }
    }
}