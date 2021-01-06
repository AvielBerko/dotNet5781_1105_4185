using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	public class Bus
	{
        public Registration Registration { get; set; }
        public int Kilometrage { get; set; }
        public int FuelLeft { get; set; }
        public BusStatus Status { get; set; }
        public BusTypes Type { get; set; }
    }
}
