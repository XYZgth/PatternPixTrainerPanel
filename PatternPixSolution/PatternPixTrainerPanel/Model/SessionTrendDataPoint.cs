using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.Model
{
    public class SessionTrendDataPoint
    {
        public string Label { get; set; }
        public double AvgErrors { get; set; }
        public double AvgTime { get; set; }
    }

}
