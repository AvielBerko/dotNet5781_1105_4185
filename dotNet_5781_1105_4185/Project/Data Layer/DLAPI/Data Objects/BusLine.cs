using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusLine
    {
        public Guid ID { get; set; }
        public int LineNum { get; set; }
        public Regions Region { get; set; }
        public int? StartStationCode { get; set; }
        public int? EndStationCode { get; set; }
        public int RouteLength { get; set; }
        public bool HasFullRoute { get; set; }
    }
}
