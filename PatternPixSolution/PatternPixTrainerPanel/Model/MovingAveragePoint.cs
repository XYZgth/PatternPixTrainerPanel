using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.Model
{
    /**
     * \brief Repräsentiert einen Datenpunkt für die Visualisierung gleitender Durchschnitte.
     * 
     * Diese Klasse wird verwendet, um Kennzahlen wie durchschnittliche Zeit und durchschnittliche Fehler
     * pro Trainingseinheit (Session) darzustellen - Diagramme mit Syncfusion.
     */
    public class MovingAveragePoint
    {
        /**
         * \brief Nummer der Trainingseinheit (Session)
         */
        public int SessionNumber { get; set; }

        /**
         * \brief Durchschnittlich benötigte Zeit
         * 
         * Diese Kennzahl ergibt sich aus mehreren Trainings
         */
        public double AverageTime { get; set; }

        /**
         * \brief Durchschnittliche Fehleranzahl in der entsprechenden Zeit.
         * 
         * Darstellung von Fehlern in der Trainingsleistung.
         */
        public double AverageErrors { get; set; }
    }
}
