using PatternPixTrainerPanel.Data;
using PatternPixTrainerPanel.Model;
using System;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace PatternPixTrainerPanel.Utilities
{
    /**
     * \brief Dienstklasse zur Initialisierung und Befüllung der Datenbank mit Testdaten.
     */
    public static class DatabaseSeeder
    {
        /**
         * \brief Führt das Seeding der Datenbank durch.
         * 
         * Erstellt die Datenbank, falls sie noch nicht existiert, und fügt Testdaten
         * für Kinder und zugehörige Trainingseinheiten hinzu.
         * 
         * Wenn bereits Kinder vorhanden sind, wird der Vorgang übersprungen.
         */
        public static void SeedDatabase()
        {
            using (var context = new PatternPixDbContext())
            {
                // Erstellt die Datenbankstruktur, falls noch nicht vorhanden
                context.Database.EnsureCreated();

                // Nur fortfahren, wenn noch keine Kinder in der Datenbank vorhanden sind
                if (!context.Children.Any())
                {
                    // Beispielkinder anlegen
                    var emma = new Child
                    {
                        FirstName = "Emma",
                        LastName = "Jonas",
                        DateOfBirth = new DateTime(2018, 5, 12)
                    };

                    var noah = new Child
                    {
                        FirstName = "Noah",
                        LastName = "Binder",
                        DateOfBirth = new DateTime(2017, 8, 23)
                    };

                    var olivia = new Child
                    {
                        FirstName = "Olivia",
                        LastName = "Klein",
                        DateOfBirth = new DateTime(2016, 2, 3)
                    };

                    var linus = new Child
                    {
                        FirstName = "Linus",
                        LastName = "Braun",
                        DateOfBirth = new DateTime(2017, 11, 18)
                    };

                    var eva = new Child
                    {
                        FirstName = "Eva",
                        LastName = "Gartner",
                        DateOfBirth = new DateTime(2018, 9, 7)
                    };

                    var children = new List<Child> { emma, noah, olivia, linus, eva };

                    // Kinder zusätzlich in JSON-Datei speichern
                    var fileRepo = new FileChildRepository();
                    fileRepo.SaveChildren(children);

                    // Kinder in Datenbank speichern
                    context.Children.Add(emma);
                    context.Children.Add(noah);
                    context.Children.Add(olivia);
                    context.Children.Add(linus);
                    context.Children.Add(eva);

                    context.SaveChanges();

                    // Trainingsdaten zuweisen

                    // Trainings für Emma
                    context.Trainings.Add(new Training
                    {
                        ChildId = emma.Id,
                        Date = DateTime.Today.AddDays(-10),
                        TimeOfDay = new TimeSpan(9, 30, 0),
                        Symmetry = 'H',
                        Errors = 3,
                        TimeNeeded = 45
                    });

                    context.Trainings.Add(new Training
                    {
                        ChildId = emma.Id,
                        Date = DateTime.Today.AddDays(-5),
                        TimeOfDay = new TimeSpan(10, 15, 0),
                        Symmetry = 'V',
                        Errors = 2,
                        TimeNeeded = 40
                    });

                    context.Trainings.Add(new Training
                    {
                        ChildId = emma.Id,
                        Date = DateTime.Today.AddDays(-2),
                        TimeOfDay = new TimeSpan(14, 0, 0),
                        Symmetry = 'R',
                        Errors = 1,
                        TimeNeeded = 38
                    });

                    // Trainings für Noah
                    context.Trainings.Add(new Training
                    {
                        ChildId = noah.Id,
                        Date = DateTime.Today.AddDays(-7),
                        TimeOfDay = new TimeSpan(11, 0, 0),
                        Symmetry = 'H',
                        Errors = 4,
                        TimeNeeded = 105
                    });

                    context.Trainings.Add(new Training
                    {
                        ChildId = noah.Id,
                        Date = DateTime.Today.AddDays(-3),
                        TimeOfDay = new TimeSpan(13, 45, 0),
                        Symmetry = 'V',
                        Errors = 3,
                        TimeNeeded = 90
                    });

                    // Trainings für Olivia
                    context.Trainings.Add(new Training
                    {
                        ChildId = olivia.Id,
                        Date = DateTime.Today.AddDays(-12),
                        TimeOfDay = new TimeSpan(9, 0, 0),
                        Symmetry = 'B',
                        Errors = 5,
                        TimeNeeded = 60
                    });

                    context.Trainings.Add(new Training
                    {
                        ChildId = olivia.Id,
                        Date = DateTime.Today.AddDays(-6),
                        TimeOfDay = new TimeSpan(10, 30, 0),
                        Symmetry = 'H',
                        Errors = 4,
                        TimeNeeded = 58
                    });

                    // Trainings für Linus
                    context.Trainings.Add(new Training
                    {
                        ChildId = linus.Id,
                        Date = DateTime.Today.AddDays(-15),
                        TimeOfDay = new TimeSpan(15, 0, 0),
                        Symmetry = 'V',
                        Errors = 2,
                        TimeNeeded = 42
                    });

                    context.Trainings.Add(new Training
                    {
                        ChildId = linus.Id,
                        Date = DateTime.Today.AddDays(-8),
                        TimeOfDay = new TimeSpan(16, 15, 0),
                        Symmetry = 'R',
                        Errors = 1,
                        TimeNeeded = 97
                    });

                    context.Trainings.Add(new Training
                    {
                        ChildId = linus.Id,
                        Date = DateTime.Today.AddDays(-1),
                        TimeOfDay = new TimeSpan(14, 30, 0),
                        Symmetry = 'H',
                        Errors = 0,
                        TimeNeeded = 75
                    });

                    // Trainings für Eva
                    context.Trainings.Add(new Training
                    {
                        ChildId = eva.Id,
                        Date = DateTime.Today.AddDays(-9),
                        TimeOfDay = new TimeSpan(11, 45, 0),
                        Symmetry = 'V',
                        Errors = 3,
                        TimeNeeded = 48
                    });

                    context.SaveChanges();

                    Console.WriteLine("Database seeded successfully!");
                }
                else
                {
                    Console.WriteLine("Database already contains data. Skipping seed operation.");
                }
            }
        }
    }
}
