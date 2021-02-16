using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTrip
    {
        public DateTime StartTime { get; set; }
        public FrequnciedTrip? Frequncied { get; set; }
    }

    public struct FrequnciedTrip
    {
        public DateTime FinishTime { get; set; }
        public TimeSpan Frequency { get; set; }
    }
}
