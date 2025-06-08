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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            PatternPixDbContext dbContext = new PatternPixDbContext();
            try
            {
                IChildRepository childRepository = null;
                MainView view = new MainView();
                int mode = view.GetSelectedRepositoryMode();

                if (mode == 0)
                {
                    childRepository = new DBChildRepository(dbContext);
                }
                else if (mode == 1)
                {
                    childRepository = new FileChildRepository();
                }
                else
                {
                    MessageBox.Show("Not Selected");
                }

                
                // Seed the database with test data
                if (childRepository != null)
                {
                    DatabaseSeeder.SeedDatabase(childRepository);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error seeding database: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
