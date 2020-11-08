using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dotNet_5781_02_1105_4185
{
	class BusStation
	{
		public BusStation(Station station, double dist, double time)
		{
			Station = station;
			DistanceFromLastStation = dist;
			TimeFromLastStation = time;
		}
		public Station Station { get; private set; }
		public double DistanceFromLastStation { get; private set; }
		public double TimeFromLastStation { get; private set; }
	}
}
