using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dotNet_5781_02_1105_4185
{
	class BusStation : Station
	{
		public BusStation(uint code, string address, double dist, double time) : base(code, address)
		{
			DistanceFromLastStation = dist;
			TimeFromLastStation = time;
		}
		public double DistanceFromLastStation { get; private set; }
		public double TimeFromLastStation { get; private set; }
	}
}
