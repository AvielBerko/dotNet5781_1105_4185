using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// <br>Entity that represents a line station</br>
    /// <br>Every line-station has a uniqe id</br>
    /// </summary>
    public class LineStation
    {
        public Guid LineID { get; set; }
        // The station code
        public int StationCode { get; set; }
        // The station index in the line route
        public int RouteIndex { get; set; }
    }
}
