using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// <br>Entity that represents line trip</br>
    /// <br>Can be a frequencied or one time trip</br>
    /// </summary>
    public class LineTrip
    {
        // Line's ID
        public Guid LineID { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? FinishTime { get; set; }
        // Trip frequency (nullable)
        public TimeSpan? Frequency { get; set; }
    }
}
