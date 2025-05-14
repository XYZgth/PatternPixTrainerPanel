using PatternPixTrainerPanel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.Model
{
 
    public class Training
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ChildId { get; set; }

        [Required]
        public DateTime Date { get; set; }
     
        [Required]
        public TimeSpan TimeOfDay { get; set; }

        public char Symmetry { get; set; }

        public int Errors { get; set; }
   
        public int TimeNeeded { get; set; }

    
        [ForeignKey("ChildId")]
        public virtual Child Child { get; set; }
    }
}