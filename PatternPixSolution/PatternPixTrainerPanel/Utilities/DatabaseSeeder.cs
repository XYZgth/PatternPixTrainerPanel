﻿using PatternPixTrainerPanel.Data;
using PatternPixTrainerPanel.Model;
using System;
using System.Linq;
using System.IO;
using System.Text.Json;
using PatternPixTrainerPanel.Repositories;

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
        public static void SeedDatabase(IChildRepository childRepository)
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

                    // Dependency Injection Kinder speichern
                    childRepository.SaveChildren(children);



                    // Trainingsdaten zuweisen

                    var trainings = new List<Training>();

                    // Trainings für Emma
                    trainings.Add(new Training
                    {
                        ChildId = emma.Id,
                        Date = DateTime.Today.AddDays(-10),
                        TimeOfDay = new TimeSpan(9, 30, 0),
                        Symmetry = "H",
                        Errors = 3,
                        TimeNeeded = 45
                    });

                    trainings.Add(new Training
                    {
                        ChildId = emma.Id,
                        Date = DateTime.Today.AddDays(-10),
                        TimeOfDay = new TimeSpan(9, 35, 0),
                        Symmetry = "V",
                        Errors = 2,
                        TimeNeeded = 40
                    });

                    trainings.Add(new Training
                    {
                        ChildId = emma.Id,
                        Date = DateTime.Today.AddDays(-10),
                        TimeOfDay = new TimeSpan(9, 40, 0),
                        Symmetry = "R",
                        Errors = 1,
                        TimeNeeded = 38
                    });

                    // Trainings für Noah
                    trainings.Add(new Training
                    {
                        ChildId = noah.Id,
                        Date = DateTime.Today.AddDays(-3),
                        TimeOfDay = new TimeSpan(13, 40, 0),
                        Symmetry = "H",
                        Errors = 4,
                        TimeNeeded = 105
                    });

                    trainings.Add(new Training
                    {
                        ChildId = noah.Id,
                        Date = DateTime.Today.AddDays(-3),
                        TimeOfDay = new TimeSpan(13, 45, 0),
                        Symmetry = "V",
                        Errors = 3,
                        TimeNeeded = 90
                    });

                    // Trainings für Olivia
                    trainings.Add(new Training
                    {
                        ChildId = olivia.Id,
                        Date = DateTime.Today.AddDays(-12),
                        TimeOfDay = new TimeSpan(10, 25, 0),
                        Symmetry = "B",
                        Errors = 5,
                        TimeNeeded = 60
                    });

                    trainings.Add(new Training
                    {
                        ChildId = olivia.Id,
                        Date = DateTime.Today.AddDays(-12),
                        TimeOfDay = new TimeSpan(10, 30, 0),
                        Symmetry = "H",
                        Errors = 4,
                        TimeNeeded = 58
                    });

                    // Trainings für Linus
                    trainings.Add(new Training
                    {
                        ChildId = linus.Id,
                        Date = DateTime.Today.AddDays(-8),
                        TimeOfDay = new TimeSpan(16, 10, 0),
                        Symmetry = "V",
                        Errors = 2,
                        TimeNeeded = 42
                    });

                    trainings.Add(new Training
                    {
                        ChildId = linus.Id,
                        Date = DateTime.Today.AddDays(-8),
                        TimeOfDay = new TimeSpan(16, 15, 0),
                        Symmetry = "R",
                        Errors = 1,
                        TimeNeeded = 97
                    });

                    trainings.Add(new Training
                    {
                        ChildId = linus.Id,
                        Date = DateTime.Today.AddDays(-8),
                        TimeOfDay = new TimeSpan(16, 20, 0),
                        Symmetry = "H",
                        Errors = 0,
                        TimeNeeded = 75
                    });

                    // Trainings für Eva
                    trainings.Add(new Training
                    {
                        ChildId = eva.Id,
                        Date = DateTime.Today.AddDays(-9),
                        TimeOfDay = new TimeSpan(11, 45, 0),
                        Symmetry = "V",
                        Errors = 3,
                        TimeNeeded = 48
                    });


                    var random = new Random();
                    var symmetries = new[] { "V", "H", "B", "R", "?" };

                    foreach (var child in children)
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            var date = DateTime.Today.AddDays(-random.Next(1, 60));
                            var time = new TimeSpan(random.Next(8, 17), random.Next(0, 60), 0); // zwischen 08:00 und 16:59
                            var symmetry = symmetries[random.Next(symmetries.Length)];
                            var errors = random.Next(0, 7); // 0–6 Fehler
                            var timeNeeded = random.Next(40, 121); // 40–120 Sekunden

                            trainings.Add(new Training
                            {
                                ChildId = child.Id,
                                Date = date,
                                TimeOfDay = time,
                                Symmetry = symmetry,
                                Errors = errors,
                                TimeNeeded = timeNeeded
                            });
                        }
                    }

                    // Dependency Injection Trainings speichern
                    childRepository.SaveTrainings(trainings);

                    //Success
                }
                else
                {
                    Console.WriteLine("Database already contains data. Skipping seed operation.");
                }
            }
        }
    }
}
