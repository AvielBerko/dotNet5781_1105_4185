using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// <br>Entity that represents a pysical bus</br>
    /// <br>Saves the bus' registration details, kilometrage, fuel and other data</br>
    /// </summary>
    public class Bus
    {
        // Registration number
        public int RegNum { get; set; }
        // Registration date
        public DateTime RegDate { get; set; }
        public int Kilometrage { get; set; }
        public int FuelLeft { get; set; }
        // Bus' status (Ready, Driving, ect...)
        public BusStatus Status { get; set; }
        // Bus' type (duplex, double, ect...)
        public BusTypes Type { get; set; }
    }
}
