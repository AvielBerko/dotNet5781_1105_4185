using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185.src
{
	class BusList : IEnumerable
	{
		private List<Bus> buses = new List<Bus>();

		public Bus this[uint line]
		{
			get
			{
				Bus result = buses.Find((bus) => bus.BusLine == line);
				if (result == null)
					throw new ArgumentException("No bus was found with this line number", nameof(line));
				return result;
			}
		}

		public void AddBus(Bus bus)
		{
			var all = buses.FindAll((value) => value.BusLine == bus.BusLine);
			if (all.Count > 1)
				throw new ArgumentException("2 Buses already exists", nameof(bus));
			if (all.Count == 1)
			{
				Bus exists = all[0];
				if (exists.FirstStation != bus.LastStation || exists.LastStation != bus.FirstStation)
					throw new ArgumentException("Bus exists but not the opposite route", nameof(bus));
			}

			buses.Add(bus);
		}

		public void RemoveBus(Bus bus) => buses.Remove(bus);

		public List<Bus> BusesOfStation(uint stationCode)
		{
			return (from bus
					in buses
					where bus.BusRoute.Any((station) => station.Station.Code == stationCode)
					select bus).ToList();
		}

		public List<Bus> BusesByTime()
		{
			return (from bus
					in buses
					orderby bus.RouteTime()
					select bus).ToList();
		}

		public IEnumerator GetEnumerator()
		{
			return buses.GetEnumerator();
		}
	}
}
