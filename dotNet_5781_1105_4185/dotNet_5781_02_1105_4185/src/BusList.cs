using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185
{
	/// <summary>
	/// Collection of buses.
	/// </summary>
	class BusList : IEnumerable
	{
		/// <summary>
		/// List of all existing buses.
		/// </summary>
		private List<Bus> buses = new List<Bus>();

		/// <summary>
		/// Indexer - gets a bus by line and direction.
		/// </summary>
		/// <param name="line">The bus' line number.</param>
		/// <param name="dir">The bus' direction.</param>
		/// <returns>The found bus.</returns>
		/// <exception cref="ArgumentException">Bus wasn't found.</exception>
		public Bus this[uint line, Direction dir = Direction.Go]
		{
			get
			{
				Bus result = buses.Find((bus) => bus.Line == line && bus.Direction == dir);
				if (result == null)
					throw new ArgumentException("No bus was found with this line number");
				return result;
			}
		}

		/// <summary>
		/// Adds a new bus to the collection.
		/// </summary>
		/// <param name="bus">The bus to add.</param>
		/// <exception cref="ArgumentException"></exception>
		public void AddBus(Bus bus)
		{
			var all = buses.FindAll((value) => value.Line == bus.Line);
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

		/// <summary>
		/// Removes a bus from the collection.
		/// </summary>
		/// <param name="bus">The bus to remove.</param>
		public void RemoveBus(Bus bus) => buses.Remove(bus);
		/// <summary>
		/// Removes all buses with the line number.
		/// </summary>
		/// <param name="lineNum">The buses line number</param>
		public void RemoveLine(uint lineNum)
		{
			var busesToRemove = buses.FindAll((bus) => bus.Line == lineNum);
			foreach (var bus in busesToRemove)
				RemoveBus(bus);
		}

		/// <summary>
		/// Finds all buses stopping at a station by a given code.
		/// </summary>
		/// <param name="stationCode">The station code.</param>
		/// <returns>List of the found buses.</returns>
		/// <exception cref="ArgumentException">Couldn't find a station with this code.</exception>
		public List<Bus> BusesOfStation(uint stationCode)
		{
			var station = Station.Stations.Find((item) => item.Code == stationCode);
			if (station == null)
				throw new ArgumentException("Couldn't find station");

			return BusesOfStation(station);
		}
		/// <summary>
		/// Finds all buses stopping at a given station
		/// </summary>
		/// <param name="station">The station.</param>
		/// <returns>List of the found buses.</returns>
		/// <exception cref="ArgumentNullException">station is null.</exception>
		public List<Bus> BusesOfStation(Station station)
		{
			if (station == null)
				throw new ArgumentNullException(nameof(station));

			return (from bus
					in buses
					where bus.InRoute(station)
					group bus by bus.Line into lines
					select lines.First()).ToList();
		}

		/// <summary>
		/// Creates a list of all buses sorted by route time.
		/// </summary>
		/// <returns>The created list.</returns>
		public List<Bus> BusesByTime()
		{
			var result = (from bus in buses select bus).ToList();
			result.Sort(); // using ICompareable.
			return result;
		}

		public IEnumerator GetEnumerator()
		{
			return buses.GetEnumerator();
		}
	}
}
