using Microsoft.EntityFrameworkCore;
using PatternPixTrainerPanel.Model;

namespace PatternPixTrainerPanel.Data
{
    /**
     * \brief Datenbankkontext für die PatternPix-Anwendung.
     * 
     * Stellt den Zugriff auf die Datenbanktabellen Children und Trainings bereit
     * und konfiguriert die Datenbankverbindung sowie die Beziehungen zwischen Entitäten.
     */
    public class PatternPixDbContext : DbContext
    {
        /// \brief Tabelle mit Kind-Entitäten.
        public DbSet<Child> Children { get; set; }
        /// \brief Tabelle mit Training-Entitäten.
        public DbSet<Training> Trainings { get; set; }

        /**
         * \brief Konfiguriert die Datenbankverbindung.
         * 
         * Setzt die SQLite-Datenbankdatei als Datenquelle.
         * 
         * \param optionsBuilder Builder zur Konfiguration der DbContext-Optionen.
         */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=..\\..\\..\\..\\PatternPix.db");
        }

        /**
         * \brief Konfiguriert die Modellbeziehungen und weitere Einstellungen.
         * 
         * Definiert die Beziehung zwischen Child und Training sowie die Konvertierung des TimeOfDay-Feldes.
         * 
         * \param modelBuilder Builder zum Konfigurieren des Datenmodells.
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Beziehung: Ein Child hat viele Trainings, Trainings referenzieren ein Child
            modelBuilder.Entity<Child>()
                .HasMany(c => c.Trainings)
                .WithOne(t => t.Child)
                .HasForeignKey(t => t.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konvertiert TimeSpan (TimeOfDay) zu long (Ticks) und zurück
            modelBuilder.Entity<Training>()
                .Property(t => t.TimeOfDay)
                .HasConversion(
                    v => v.Ticks,
                    v => TimeSpan.FromTicks(v));
        }
    }
}
