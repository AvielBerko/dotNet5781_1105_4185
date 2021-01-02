using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI.DO
{
    public class AdjacentStations
    {
        public Guid ID { get; set; }
        public int Station1Code { get; set; }
        public int Station2Code { get; set; }
        public int DistanceInMeters { get; set; }
        public TimeSpan DrivingTime { get; set; }
    }
}
