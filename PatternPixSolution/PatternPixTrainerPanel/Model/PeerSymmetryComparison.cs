using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.Model
{
    /**
     * \brief Repräsentiert einen Vergleich der Symmetrie-Statistiken eines Kindes mit dem Durchschnitt.
     * 
     * Diese Klasse erweitert \c SymmetryStats und fügt zusätzliche Metriken hinzu, um die Leistung
     * eines Kindes mit dem Durchschnitt zu vergleichen.
     */
    public class PeerSymmetryComparison : SymmetryStats
    {
        /**
         * \brief Durchschnittliche Zeit.
         * 
         * Repräsentiert die durchschnittliche Zeit (in Sekunden).
         */
        public double PeerAvgTime { get; set; }

        /**
         * \brief Durchschnittliche Fehleranzahl.
         * 
         * Repräsentiert die durchschnittliche Anzahl an Fehlern.
         */
        public double PeerAvgErrors { get; set; }

        /**
         * \brief Beschriftung für den Vergleich zwischen Kind und Peers.
         * 
         * Gibt eine Zeichenkette im Format "<Symmetry> (Child vs Peers)" zurück.
         */
        public string ComparisonLabel => $"{Symmetry} (Child vs Peers)";
    }
}
