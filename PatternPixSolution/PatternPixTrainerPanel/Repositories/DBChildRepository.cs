using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatternPixTrainerPanel.Data;
using PatternPixTrainerPanel.Model;

namespace PatternPixTrainerPanel.Repositories
{
    /**
     * \brief Implementierung des \c IChildRepository zur Speicherung in einer Datenbank.
     * 
     * Diese Klasse nutzt Entity Framework Core zum Speichern und Laden von Kindern und Trainingsdaten
     * in einer SQLite Datenbank.
     */
    internal class DBChildRepository : IChildRepository
    {
        private readonly PatternPixDbContext _context;

        /**
         * \brief Konstruktor, der den Datenbank-Kontext setzt.
         * 
         * \param context Der EF Core Datenbank-Kontext.
         */
        public DBChildRepository(PatternPixDbContext context)
        {
            _context = context;
        }

        /**
         * \brief Speichert eine Liste von Kindern in der Datenbank.
         * 
         * Fügt die Kinder zur Datenbank hinzu und speichert die Änderungen.
         * 
         * \param children Die zu speichernden Kind-Objekte.
         */
        public void SaveChildren(List<Child> children)
        {
            _context.Children.AddRange(children);
            _context.SaveChanges();
        }

        /**
         * \brief Speichert eine Liste von Trainings in der Datenbank.
         * 
         * Fügt die Trainings zur Datenbank hinzu und speichert die Änderungen.
         * 
         * \param trainings Die zu speichernden Training-Objekte.
         */
        public void SaveTrainings(List<Training> trainings)
        {
            _context.Trainings.AddRange(trainings);
            _context.SaveChanges();
        }

        /**
         * \brief Lädt alle Kinder inklusive zugehöriger Trainings aus der Datenbank.
         * 
         * \return Eine Liste von \c Child -Objekten mit geladenen Trainings.
         */
        public List<Child> LoadChildren()
        {
            return _context.Children
                .Include(c => c.Trainings)
                .ToList();
        }

    }
}
