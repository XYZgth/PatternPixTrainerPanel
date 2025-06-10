using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.Model
{
    /**
     * \brief Repräsentiert Vergleichsdaten eines Kindes mit dem Durchschnitt.
     * 
     * Diese Klasse wird verwendet, um Kennwerte eines Kindes mit dem Durchschnittswert
     * der Alterssgruppe darzustellen.
     */
    public class PeerComparisonData
    {
        /**
         * \brief Gemessene Metrik.
         */
        public string Metric { get; set; }

        /**
         * \brief Wert des Kindes für die gegebene Metrik.
         */
        public double ChildValue { get; set; }

        /**
         * \brief Durchschnittlicher Wert der Peer-Gruppe für die gegebene Metrik.
         */
        public double PeerAverage { get; set; }

        /**
         * \brief Name des Kindes, dessen Werte dargestellt werden.
         */
        public string ChildName { get; set; }
    }

}
