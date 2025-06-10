using PatternPixTrainerPanel.Data;
using PatternPixTrainerPanel.Repositories;
using PatternPixTrainerPanel.Utilities;
using PatternPixTrainerPanel.View;
using System;
using System.Windows;

namespace PatternPixTrainerPanel
{
    /**
     * \brief Einstiegspunkt der Anwendung.
     * 
     * Initialisiert das Repository und startet die Hauptansicht.
     */
    public partial class App : Application
    {
        /// \brief Statisches Repository für Kind-Daten, wird je nach Modus gesetzt.
        public static IChildRepository ChildRepository { get; private set; }

        /**
         * \brief Wird beim Start der Anwendung aufgerufen.
         * 
         * Initialisiert das Repository, lädt Testdaten und startet die Hauptansicht.
         * 
         * \param e Startparameter der Anwendung.
         */
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            PatternPixDbContext dbContext = new PatternPixDbContext();
            try
            {
                SetRepositoryMode(1);

                // Seed the database with test data
                if (ChildRepository != null)
                {
                    DatabaseSeeder.SeedDatabase(ChildRepository);
                }

                MainView view = new MainView();
                int mode = view.GetSelectedRepositoryMode();
                

                SetRepositoryMode(mode);
                
                // Seed the database with test data
                if (ChildRepository != null)
                {
                    DatabaseSeeder.SeedDatabase(ChildRepository);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error seeding database: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /**
         * \brief Setzt den Repository-Modus (Datenbank oder Datei).
         * 
         * \param mode 0 = Datenbank-Modus, 1 = Datei-Modus.
         */
        public static void SetRepositoryMode(int mode)
        {
            PatternPixDbContext dbContext = new PatternPixDbContext();

            if (mode == 0)
            {
                ChildRepository = new DBChildRepository(dbContext);
            }
            else
            {
                ChildRepository = new FileChildRepository();
            }
        }
    }
}
