using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class AdjacentStation
    {
        public Station ToStation { get; set; }
        public double Distance { get; set; }
        public TimeSpan DrivingTime { get; set; }
    }
}
