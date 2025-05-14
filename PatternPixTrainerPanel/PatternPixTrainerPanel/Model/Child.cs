using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PatternPixTrainerPanel.Model
{
  
    public class Child
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [NotMapped]
        public int Age => CalculateAge(DateOfBirth);

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation property for child's training sessions
        public virtual ICollection<Training> Trainings { get; set; } = new List<Training>();

        [NotMapped]
        public DateTime? LastTrainingDate => Trainings?.Count > 0 ?
            Trainings.Max(t => t.Date) : null;

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            
            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}