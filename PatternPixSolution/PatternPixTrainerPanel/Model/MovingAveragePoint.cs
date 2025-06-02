using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.Model
{
    public class MovingAveragePoint
    {
        public int SessionNumber { get; set; }
        public double AverageTime { get; set; }
        public double AverageErrors { get; set; }
    }
}
