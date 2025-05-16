using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PatternPixTrainerPanel.Model
{
    /**
     * \brief Repräsentiert ein Kind im System.
     * 
     * Diese Klasse enthält grundlegende Informationen zum Kind, inklusive Name,
     * Geburtsdatum sowie berechnete Eigenschaften wie Alter und letzter Trainingstag.
     */
    public class Child
    {
        /**
         * \brief Eindeutige ID des Kindes.
         */
        [Key]
        public int Id { get; set; }

        /**
         * \brief Vorname des Kindes.
         */
        [Required]
        public string FirstName { get; set; }

        /**
         * \brief Nachname des Kindes.
         */
        [Required]
        public string LastName { get; set; }

        /**
         * \brief Geburtsdatum des Kindes.
         */
        [Required]
        public DateTime DateOfBirth { get; set; }

        /**
         * \brief Berechnetes Alter des Kindes basierend auf dem Geburtsdatum.
         */
        [NotMapped]
        public int Age => CalculateAge(DateOfBirth);

        /**
         * \brief Vollständiger Name (Vorname + Nachname) des Kindes.
         */
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        /**
         * \brief Liste aller Trainingseinheiten des Kindes.
         */
        public virtual ICollection<Training> Trainings { get; set; } = new List<Training>();

        /**
         * \brief Datum der letzten Trainingseinheit (falls vorhanden).
         */
        [NotMapped]
        public DateTime? LastTrainingDate => Trainings?.Count > 0 ?
            Trainings.Max(t => t.Date) : null;

        /**
         * \brief Berechnet das Alter auf Basis des Geburtsdatums.
         * \param birthDate Das Geburtsdatum.
         * \return Das berechnete Alter in Jahren.
         */
        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            // Falls Geburtstag dieses Jahr noch nicht erreicht wurde, Alter um 1 reduzieren
            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
