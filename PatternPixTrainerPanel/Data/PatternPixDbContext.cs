using Microsoft.EntityFrameworkCore;
using PatternPixTrainerPanel.Model;

namespace PatternPixTrainerPanel.Data
{
    public class PatternPixDbContext : DbContext
    {
        public DbSet<Child> Children { get; set; }
        public DbSet<Training> Trainings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure the database connection string
            optionsBuilder.UseSqlite("Data Source=..\\..\\..\\..\\PatternPix.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between Child and Training
            modelBuilder.Entity<Child>()
                .HasMany(c => c.Trainings)
                .WithOne(t => t.Child)
                .HasForeignKey(t => t.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            // Additional configurations
            modelBuilder.Entity<Training>()
                .Property(t => t.TimeOfDay)
                .HasConversion(
                    v => v.Ticks,
                    v => TimeSpan.FromTicks(v));
        }
    }
}