using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusLine
    {
        public Guid ID { get; set; }
        public int LineNum { get; set; }
        public Regions Region { get; set; }
        public IEnumerable<LineStation> Route { get; set; }
        public IEnumerable<Trip> Trips;
    }
}
