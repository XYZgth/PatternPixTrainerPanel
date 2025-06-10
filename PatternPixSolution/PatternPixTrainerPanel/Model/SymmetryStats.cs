using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.Model
{
    /**
     * \brief Repräsentiert Daten zu einer bestimmten Symmetrie.
     * 
     * Diese Klasse enthält grundlegende Kennzahlen wie durchschnittliche Bearbeitungszeit
     * und Fehleranzahl für eine Symmetrie-Kategorie.
     */
    public class SymmetryStats
    {
        /**
         * \brief Bezeichnung der Symmetrie.
         * 
         * Typ der Symmetrie (z. B. „Horizontal“, „Vertikal“ usw.).
         */
        public string Symmetry { get; set; }

        /**
         * \brief Durchschnittliche Zeit.
         * 
         * Gibt an, wie lange im Durchschnitt für diese Symmetrie benötigt wurde.
         */
        public double AvgTime { get; set; }

        /**
         * \brief Durchschnittliche Fehleranzahl.
         * 
         * Gibt an, wie viele Fehler im Durchschnitt bei Aufgaben dieser Symmetrieform gemacht wurden.
         */
        public double AvgErrors { get; set; }
    }

}
