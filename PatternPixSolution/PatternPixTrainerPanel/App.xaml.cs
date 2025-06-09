using PatternPixTrainerPanel.Data;
using PatternPixTrainerPanel.Repositories;
using PatternPixTrainerPanel.Utilities;
using PatternPixTrainerPanel.View;
using System;
using System.Windows;

namespace PatternPixTrainerPanel
{
    public partial class App : Application
    {
        public static IChildRepository ChildRepository { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            PatternPixDbContext dbContext = new PatternPixDbContext();
            try
            {
                MainView view = new MainView();
                int mode = view.GetSelectedRepositoryMode();
                //int mode = 1;

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
