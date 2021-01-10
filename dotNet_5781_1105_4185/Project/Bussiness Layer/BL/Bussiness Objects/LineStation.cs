using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	public class LineStation
	{
		public Station Station { get; set; }
		public double? Distance { get; set; }
		public TimeSpan? DrivingTime { get; set; }
	}
}
