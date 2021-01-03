using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineTrip
    {
        public Guid ID { get; set; }
        public Guid LineID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public TimeSpan? Frequency { get; set; }
    }
}
