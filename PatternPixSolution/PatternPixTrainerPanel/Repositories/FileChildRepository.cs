using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PatternPixTrainerPanel.Model;
using System.IO;
using PatternPixTrainerPanel.Data;

namespace PatternPixTrainerPanel.Repositories
{
    /**
     * \brief Implementierung des IChildRepository zur Speicherung in JSON-Dateien.
     * 
     * Diese Klasse speichert und lädt Kinder- sowie Trainingsdaten mithilfe des
     * \c FileJSONRepository als JSON-Dateien im Dateisystem.
     */
    internal class FileChildRepository : IChildRepository
    {
        private readonly FileJSONRepository _childRepo = new FileJSONRepository("children");
        private readonly FileJSONRepository _trainingRepo = new FileJSONRepository("trainings");

        /**
         * \brief Speichert eine Liste neuer Kinder.
         * 
         * Neue Kinder erhalten fortlaufende IDs und werden an die bestehende Liste angehängt.
         * 
         * \param Children Die zu speichernden Kind-Objekte.
         */
        public void SaveChildren(List<Child> Children)
        {
            // Vorhandene Kinder laden
            var existingChildren = _childRepo.Load<Child>();


            // Höchste existierende ID ermitteln
            int maxId = existingChildren.Any() ? existingChildren.Max(c => c.Id) : 0;

            // IDs für neue Kinder zuweisen
            foreach (var child in Children)
            {
                child.Id = ++maxId;
            }



            // Neue Kinder anhängen
            existingChildren.AddRange(Children);

            // Gesamte Liste speichern
            _childRepo.Save(existingChildren);
        }

        /**
         * \brief Speichert eine Liste neuer Trainingsdaten.
         * 
         * Neue Trainings erhalten fortlaufende IDs und werden an die bestehende Liste angehängt.
         * 
         * \param trainings Die zu speichernden Training-Objekte.
         */
        public void SaveTrainings(List<Training> trainings)
        {
            // Vorhandene Trainings laden
            var existingTraining = _trainingRepo.Load<Training>();


            // Höchste existierende ID ermitteln
            int maxId = existingTraining.Any() ? existingTraining.Max(t => t.Id) : 0;

            // IDs für neue Trainings zuweisen
            foreach (var training in trainings)
            {
                training.Id = ++maxId;
            }



            // Neue Trainings anhängen
            existingTraining.AddRange(trainings);

            // Gesamte Trainings speichern
            _trainingRepo.Save(existingTraining);
        }

        /**
         * \brief Lädt alle gespeicherten Kinder und verknüpft ihre Trainingsdaten.
         * 
         * Trainingsdaten werden anhand der \c ChildId zugeordnet.
         * 
         * \return Eine Liste aller Kinder mit ihren zugehörigen Trainings.
         */
        public List<Child> LoadChildren()
        {
            var children = _childRepo.Load<Child>();
            var trainings = _trainingRepo.Load<Training>();

            // Trainings zu den jeweiligen Kindern zuordnen
            foreach (var child in children)
            {
                child.Trainings = trainings
                    .Where(t => t.ChildId == child.Id)
                    .ToList();
            }

            return children;
        }

    }
}