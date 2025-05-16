using PatternPixTrainerPanel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PatternPixTrainerPanel.Model
{
    /**
     * \brief Repräsentiert eine Trainingseinheit eines Kindes.
     * 
     * Diese Klasse enthält Informationen über ein einzelnes Training,
     * wie Datum, Uhrzeit, Art der Symmetrieübung, Fehleranzahl und benötigte Zeit.
     */
    public class Training
    {
        /**
         * \brief Eindeutige ID der Trainingseinheit.
         */
        [Key]
        public int Id { get; set; }

        /**
         * \brief Fremdschlüssel zur ID des zugehörigen Kindes.
         */
        [Required]
        public int ChildId { get; set; }

        /**
         * \brief Datum, an dem das Training stattgefunden hat.
         */
        [Required]
        public DateTime Date { get; set; }

        /**
         * \brief Tageszeit (Uhrzeit) des Trainings.
         */
        [Required]
        public TimeSpan TimeOfDay { get; set; }

        /**
         * \brief Art der geübten Symmetrie (z. B. 'H' für horizontal, 'V' für vertikal).
         */
        public char Symmetry { get; set; }

        /**
         * \brief Anzahl der Fehler während des Trainings.
         */
        public int Errors { get; set; }

        /**
         * \brief Benötigte Zeit für das Training (in Sekunden oder einer anderen Einheit).
         */
        public int TimeNeeded { get; set; }

        /**
         * \brief Navigationseigenschaft zum zugehörigen Kind.
         */
        [ForeignKey("ChildId")]
        public virtual Child Child { get; set; }
    }
}
