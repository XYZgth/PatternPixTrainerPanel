using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatternPixTrainerPanel.Model;

namespace PatternPixTrainerPanel.Repositories
{
    /**
     * \brief Schnittstelle zum Speichern und Laden von Kindern und Trainingsdaten.
     * 
     * Diese Schnittstelle definiert Methoden zur Persistierung und zum Abrufen von
     * \c Child - und \c Training -Objekten aus einer Datei oder Datenbank.
     */
    public interface IChildRepository
    {
        /**
         * \brief Speichert eine Liste von Kindern.
         * 
         * \param children Die zu speichernden Kind-Objekte.
         */
        void SaveChildren(List<Child> children);

        /**
         * \brief Speichert eine Liste von Trainings.
         * 
         * \param trainings Die zu speichernden Training-Objekte.
         */
        void SaveTrainings(List<Training> trainings);

        /**
         * \brief Lädt eine Liste gespeicherter Kinder.
         * 
         * \return Eine Liste von \c Child -Objekten.
         */
        List<Child> LoadChildren();
    }

}