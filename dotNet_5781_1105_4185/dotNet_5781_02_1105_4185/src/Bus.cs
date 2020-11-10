using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185
{
	/// <summary>
	/// Buses' areas of operation.
	/// </summary>
	enum Areas { General, North, South, Center, Jerusalem, Eilat }
	/// <summary>
	/// Buses' driving direction.
	/// </summary>
	enum Direction { Go = 1, Return }

	/// <summary>
	/// Class for storing bus data.
	/// </summary>
	class Bus : IComparable<Bus>
	{
		/// <summary>
		/// Creates new instance of a bus.
		/// </summary>
		/// <param name="line">Bus' line.</param>
		/// <param name="area">Bus' operation area.</param>
		/// <param name="direction">Bus' direction.</param>
		/// <param name="route">Bus' route</param>
		/// <exception cref="ArgumentException">When route has less than 2 stations</exception>
		public Bus(uint line, Areas area, Direction direction, List<BusStation> route)
		{
			Line = line;
			Area = area;
			Direction = direction;
			Route = route;
		}

		/// <summary>
		/// Bus' route of stations.
		/// </summary>
		public List<BusStation> Route
		{
			get => route;
			private set
			{
				if (value.Count < 2)
					throw new ArgumentException("Bus route should contain at least 2 stations");
				route = value;
			}
		}
		private List<BusStation> route;

		/// <summary>
		/// Bus' line number.
		/// </summary>
		public uint Line { get; private set; }
		/// <summary>
		/// Bus' operation area.
		/// </summary>
		public Areas Area
		{
			get => area;
			private set
			{
				if (!Enum.IsDefined(typeof(Areas), value))
					throw new ArgumentOutOfRangeException();
				area = value;
			}
		}
		private Areas area;
		/// <summary>
		/// Bus' direction.
		/// </summary>
		public Direction Direction
		{
			get => direction;
			private set
			{
				if (!Enum.IsDefined(typeof(Direction), value))
					throw new ArgumentOutOfRangeException();
				direction = value;
			}
		}
		private Direction direction;
		/// <summary>
		/// Bus' first station in the route.
		/// </summary>
		public BusStation FirstStation => Route[0];
		/// <summary>
		/// Bus' last station in the route.
		/// </summary>
		public BusStation LastStation => Route[Route.Count - 1];

		/// <summary>
		/// Inserts a new bus station after an existing station in the route.
		/// <br>If afterStation is null, inserts the station at the begining of the route.</br>
		/// </summary>
		/// <param name="newStation">Station to add.</param>
		/// <param name="afterStation">Station to add after.</param>
		/// <exception cref="ArgumentException">When afterStation is not in the route.</exception>
		public void InsertStation(BusStation newStation, Station afterStation = null)
		{
			if (afterStation == null)
			{
				// Inserts at the begining.
				Route.Insert(0, newStation);
				return;
			}

			// Finds the index of afterStation.
			var index = Route.FindIndex((item) => item.Station == afterStation);
			if (index == -1)
				throw new ArgumentException("Couldn't find station to add after");

			// Inserts after the found station.
			Route.Insert(index + 1, newStation);
		}

		/// <summary>
		/// Removes from the route all bus stations with a given station.
		/// </summary>
		/// <param name="station">Station to remove.</param>
		/// <exception cref="InvalidOperationException" ></exception>
		public void RemoveStation(Station station)
		{
			if (Route.Count((item) => item.Station != station) < 2)
				throw new InvalidOperationException("Bus cannot contaion less than 2 stations");
			Route.RemoveAll((item) => item.Station == station);
		}

		/// <summary>
		/// Checks if a given station is in the bus' route.
		/// </summary>
		/// <param name="station">The station to check.</param>
		/// <returns>True if the station is in the route, else false.</returns>
		public bool InRoute(Station station) => Route.Any((item) => item.Station == station);

		/// <summary>
		/// Creates a new bus with a sub route between 2 given stations.
		/// </summary>
		/// <param name="start">Starting station.</param>
		/// <param name="end">Ending station.</param>
		/// <returns>The new bus.</returns>
		/// <exception cref="ArgumentException">When start station is after end station.</exception>
		public Bus GetSubRoute(Station start, Station end)
		{
			var indices = GetIndices(start, end, sort: false);

			// checks that the first station is before the second.
			if (indices.Item1 >= indices.Item2)
				throw new ArgumentException("start station shouldn't be after the end station");
			var count = indices.Item2 - indices.Item1 + 1;

			return new Bus(Line, Area, Direction, Route.GetRange(indices.Item1, count));
		}

		/// <summary>
		/// Calculates the distance between two given stations.
		/// <br>Also working when start is after end.</br>
		/// </summary>
		/// <param name="start">The starting station.</param>
		/// <param name="end">The ending station.</param>
		/// <returns>The calculated distance in meters.</returns>
		public double RouteDistance(Station start, Station end)
		{
			var indices = GetIndices(start, end);
			double result = 0;
			for (int i = indices.Item1 + 1; i <= indices.Item2; i++)
			{
				result += Route[i].DistanceFromLastStation;
			}

			return result;
		}

		/// <summary>
		/// Calculates the time between two given stations.
		/// <br>Also working when start is after end.</br>
		/// </summary>
		/// <param name="start">The starting station.</param>
		/// <param name="end">The ending station.</param>
		/// <returns>The calculated time in minutes.</returns>
		public double RouteTime(Station start, Station end)
		{
			var indices = GetIndices(start, end);
			double result = 0;
			for (int i = indices.Item1 + 1; i <= indices.Item2; i++)
			{
				result += Route[i].TimeFromLastStation;
			}

			return result;
		}
		/// <summary>
		/// Calculates the whole route time.
		/// </summary>
		/// <returns>The calculated time in minutes.</returns>
		public double RouteTime()
		{
			return RouteTime(FirstStation.Station, LastStation.Station);
		}

		/// <summary>
		/// Finds the indices of 2 given stations.
		/// </summary>
		/// <param name="sort">If true it sorts the found indices.</param>
		/// <returns>A tuple with the found indices.</returns>
		/// <exception cref="ArgumentException"></exception>
		private Tuple<int, int> GetIndices(Station first, Station second, bool sort = true)
		{
			var firstIdx = Route.FindIndex((item) => item.Station == first);
			if (firstIdx == -1)
				throw new ArgumentException("Couldn't find the first station");

			var secondIdx = Route.FindIndex((item) => item.Station == second);
			if (secondIdx == -1)
				throw new ArgumentException("Couldn't find the second station");

			if (sort)
			{
				// Make firstIdx to be the smaller and secondIdx to be the larger.
				var temp = Math.Min(firstIdx, secondIdx);
				firstIdx = Math.Max(firstIdx, secondIdx);
				secondIdx = temp;
			}

			return new Tuple<int, int>(firstIdx, secondIdx);
		}

		public override string ToString()
		{
			string result = $"Bus Line: {Line}\nArea: {Area}\nDirection: {Direction}\n";
			result += string.Join("->", from station in Route select station.Station.Code);
			return result;
		}

		public int CompareTo(Bus other)
		{
			return (int)(RouteTime() - other.RouteTime());
		}
	}
}
