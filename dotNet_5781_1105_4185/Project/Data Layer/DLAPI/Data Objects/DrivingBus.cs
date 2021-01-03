using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class DrivingBus
    {
        public Guid ID { get; set; }
        public int RegNum { get; set; }
        public Guid LineID { get; set; }
        public DateTime PlannedTakeOff { get; set; }
        public DateTime ActualTakeOff { get; set; }
        public int PrevStationCode { get; set; }
        public DateTime PrevStationTime { get; set; }
        public DateTime NextStationTime { get; set; }
    }
}
