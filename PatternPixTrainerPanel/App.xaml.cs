using PatternPixTrainerPanel.Utilities;
using System;
using System.Windows;

namespace PatternPixTrainerPanel
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Seed the database with test data
                DatabaseSeeder.SeedDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error seeding database: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
