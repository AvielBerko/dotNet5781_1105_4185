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
					throw new ArgumentException("No bus was found with this line number and direction");
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
		public void RemoveLine(uint lineNum) => buses.RemoveAll((bus) => bus.Line == lineNum);

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
		public List<Bus> BusesOfStation(Station station)
		{
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

		/// <summary>
		/// Adds the station to the bus.<br />
		/// Also, checks the opposite direction (Adds to both directions if needed).
		/// </summary>
		/// <param name="bus">The bus to add to.</param>
		/// <param name="station">The station to add.</param>
		/// <param name="afterStation">The station to add after.</param>
		/// <returns>True if added to the opposite direction. Else, false</returns>
		public bool AddStationToBus(Bus bus, BusStation station, Station afterStation)
		{
			var addedBothDir = false;
			if (afterStation == null || bus.LastStation.Station == afterStation)
			{
				try
				{
					var oppositeBus = this[bus.Line, 3 - bus.Direction];

					// Adding to the opposite position.
					if (afterStation == null)
						oppositeBus.InsertStation(station, oppositeBus.LastStation.Station);
					else
						oppositeBus.InsertStation(station);

					addedBothDir = true;
				}
				// In case the opposite bus doesn't exist.
				catch { }
			}

			bus.InsertStation(station, afterStation);
			return addedBothDir;
		}

		/// <summary>
		/// Removes the station from the bus.<br />
		/// Also, checks the opposite direction (Won't add if can't).
		/// </summary>
		/// <param name="bus">The bus to add to.</param>
		/// <param name="station">The station to add.</param>
		/// <param name="afterStation">The station to add after.</param>
		public void RemoveStationFromBus(Bus bus, Station station)
		{
			if (bus.FirstStation.Station == station || bus.LastStation.Station == station)
			{
				try
				{
					var oppositeBus = this[bus.Line, 3 - bus.Direction];
					throw new InvalidOperationException(
						"Can't remove the first or the last station a line that has go and return.\n" +
						"Try adding a station to the end or the begining of the bus' route.");
				}
				// In case the opposite bus doesn't exist.
				catch (ArgumentException)
				{
					bus.RemoveStation(station);
				}
			}
			else
			{
				bus.RemoveStation(station);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return buses.GetEnumerator();
		}
	}
}
