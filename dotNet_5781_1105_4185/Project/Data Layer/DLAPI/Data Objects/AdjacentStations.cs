using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class AdjacentStations
    {
        public int Station1Code { get; set; }
        public int Station2Code { get; set; }
        public int Distance { get; set; }
        public TimeSpan DrivingTime { get; set; }
    }
}
