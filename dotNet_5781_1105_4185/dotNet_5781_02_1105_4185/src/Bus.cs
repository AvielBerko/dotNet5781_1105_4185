using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185.src
{
	enum Areas { GENERAL, NORTH, SOUTH, CENTER, JERUSALEM, EILAT }
	class Bus
	{
		public Bus(List<BusStation> route, uint line, Areas area)
		{
			if (route.Count < 2)
				throw new ArgumentException("Bus route should contain at least 2 stations", nameof(route));
			BusRoute = route;
			BusLine = line;
			Area = area;
			buses.Add(this);
		}
		private List<Bus> buses = new List<Bus>();
		public List<BusStation> BusRoute { get; private set; }

		private uint busLine;
		public uint BusLine
		{
			get => busLine;
			private set
			{
				if (buses.Any((bus) => value == bus.busLine))
						throw new ArgumentException("BusLine number is not unique!", nameof(value));
				busLine = value;
			}
		}
		public BusStation FirstStation => BusRoute[0];
		public BusStation LastStation => BusRoute[BusRoute.Count - 1];
		public Areas Area { get; private set; }
	}
}
