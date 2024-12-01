namespace MobileNotMobileSecondAttempt.Data
{
    using Microsoft.EntityFrameworkCore;
    using MobileNotMobileSecondAttempt.Models; // Ensure this namespace matches your project structure

    public class AppDbContext : DbContext
    {
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Strength> Stiprumai { get; set; }
        public DbSet<User> Vartotojai { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Measurements
            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.HasKey(e => e.Matavimas); // Primary Key for Measurements
                                                 // Define an index on the "Atstumas" column
                entity.HasIndex(e => e.Atstumas);
                entity.ToTable("matavimai"); // Map to the 'matavimai' table in the database
            });

            // Configure Stiprumai
            modelBuilder.Entity<Strength>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary Key
                entity.ToTable("stiprumai"); // Map to the 'stiprumai' table in the database
            });

            // Configure Vartotojai
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary Key
                entity.ToTable("vartotojai"); // Map to the 'vartotojai' table in the database
            });
        }

    }
}
