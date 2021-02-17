using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTiming
    {
        public Guid LineID { get; set; }
        public int LineNum { get; set; }
        public string EndStationName { get; set; }
        public TimeSpan TripStartTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
    }
}
