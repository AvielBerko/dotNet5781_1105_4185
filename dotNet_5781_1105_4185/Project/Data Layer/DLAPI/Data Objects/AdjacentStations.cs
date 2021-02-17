using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// <br>Entity that represents two adjacent stations</br>
    /// <br>Saves the stations' codes and the distance and time from one to the other</br>
    /// </summary>
    public class AdjacentStations
    {
        public int Station1Code { get; set; }
        public int Station2Code { get; set; }
        public double Distance { get; set; }
        public TimeSpan DrivingTime { get; set; }
    }
}
