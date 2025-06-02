using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.Model
{
    public class PeerComparisonData
    {
        public string Metric { get; set; }
        public double ChildValue { get; set; }
        public double PeerAverage { get; set; }
        public string ChildName { get; set; }
    }

}
