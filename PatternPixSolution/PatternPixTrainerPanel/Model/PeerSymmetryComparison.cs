using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.Model
{
    public class PeerSymmetryComparison : SymmetryStats
    {
        public double PeerAvgTime { get; set; }
        public double PeerAvgErrors { get; set; }
        public string ComparisonLabel => $"{Symmetry} (Child vs Peers)";
    }
}
