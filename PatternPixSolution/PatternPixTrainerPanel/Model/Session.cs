using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.Model
{
    public class Session
    {
        public string Child { get; set; }
        public string Symmetry { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeOfDay => Date.TimeOfDay;
        public double DurationSeconds { get; set; }
        public int Errors { get; set; }
    }

}
