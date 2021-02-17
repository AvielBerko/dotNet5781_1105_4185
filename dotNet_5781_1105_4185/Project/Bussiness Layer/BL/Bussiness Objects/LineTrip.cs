using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTrip
    {
        public Guid LineID { get; set; }
        public TimeSpan StartTime { get; set; }
        public FrequnciedTrip? Frequencied { get; set; }
    }

    public struct FrequnciedTrip
    {
        public TimeSpan FinishTime { get; set; }
        public TimeSpan Frequency { get; set; }
    }
}
