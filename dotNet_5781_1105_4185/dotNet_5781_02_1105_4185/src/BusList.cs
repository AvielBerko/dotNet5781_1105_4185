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

		public Bus this[uint line, Direction dir = Direction.Go]
		{
			get
			{
				Bus result = buses.Find((bus) => bus.BusLine == line && bus.Direction == dir);
				if (result == null)
					throw new ArgumentException("No bus was found with this line number");
				return result;
			}
		}

		public void AddBus(Bus bus)
		{
			var all = buses.FindAll((value) => value.BusLine == bus.BusLine);
			if (all.Count > 1)
				throw new ArgumentException("2 Buses already exists");
			if (all.Count == 1)
			{
				Bus exists = all[0];
				if (exists.Direction == bus.Direction)
					throw new ArgumentException($"Bus is already defined for this direction ({exists.Direction})");
				if (exists.FirstStation != bus.LastStation || exists.LastStation != bus.FirstStation)
					throw new ArgumentException("Bus exists but not the opposite route");
			}

			buses.Add(bus);
		}

		public void RemoveBus(Bus bus) => buses.Remove(bus);
		public void RemoveLine(uint lineNum)
		{
			var busesToRemove = buses.FindAll((bus) => bus.BusLine == lineNum);
			foreach (var bus in busesToRemove)
				RemoveBus(bus);
		}
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
