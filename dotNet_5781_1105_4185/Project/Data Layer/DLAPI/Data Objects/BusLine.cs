using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// <br>Entity that represents a bus line</br>
    /// <br>Every bus-line has a uniqe id</br>
    /// </summary>
    public class BusLine
    {
        // Uniqe ID
        public Guid ID { get; set; }
        // The line number
        public int LineNum { get; set; }
        // The region that the line is associated with
        public Regions Region { get; set; }
        // The first station's code
        public int? StartStationCode { get; set; }
        // The last station's code
        public int? EndStationCode { get; set; }
        // The route length (how many stations in the route)
        public int RouteLength { get; set; }
        // Is the route full or missing
        public bool HasFullRoute { get; set; }
    }
}
