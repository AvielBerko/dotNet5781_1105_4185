using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185
{
	enum Areas { General, North, South, Center, Jerusalem, Eilat }
	enum Direction { Go = 1, Return }
	class Bus : IComparable<Bus>
	{
		public Bus(uint line, Areas area, Direction direction,List<BusStation> route)
		{
			BusLine = line;
			Area = area;
			Direction = direction;
			BusRoute = route;
		}
		private List<BusStation> busRoute;
		public List<BusStation> BusRoute
		{
			get => busRoute;
			private set
			{
				if (value.Count < 2)
					throw new ArgumentException("Bus route should contain at least 2 stations");
				busRoute = value;
			}
		}

		public uint BusLine { get; set; }
		public Direction Direction { get; private set; }
		public BusStation FirstStation => BusRoute[0];
		public BusStation LastStation => BusRoute[BusRoute.Count - 1];
		public Areas Area { get; private set; }

		public void InsertStation(BusStation newStation, Station afterStation)
		{
			var index = BusRoute.FindIndex((item) => item.Station == afterStation);
			if (index == -1)
				throw new ArgumentException("Couldn't find station to add after");

			BusRoute.Insert(index + 1, newStation);
		}

		public void RemoveStation(Station station) => BusRoute.RemoveAll((item) => item.Station == station);

		public bool InRoute(Station station) => BusRoute.Any((item) => item.Station == station);

		public Bus GetSubRoute(Station start, Station end)
		{
			var indices = GetIndex(start, end);
			var length = indices.Item2 - indices.Item1;

			return new Bus(BusLine, Area, Direction, BusRoute.GetRange(indices.Item1, length));
		}

		public double RouteDistance(Station start, Station end)
		{
			var indices = GetIndex(start, end);
			double result = 0;
			for (int i = indices.Item1 + 1; i < indices.Item2; i++)
			{
				result += BusRoute[i].DistanceFromLastStation;
			}

			return result;
		}
		public double RouteTime(Station start, Station end)
		{
			var indices = GetIndex(start, end);
			double result = 0;
			for (int i = indices.Item1 + 1; i < indices.Item2; i++)
			{
				result += BusRoute[i].TimeFromLastStation;
			}

			return result;
		}
		public double RouteTime()
		{
			return RouteTime(FirstStation.Station, LastStation.Station);
		}

		private Tuple<int, int> GetIndex(Station first, Station second)
		{
			var firstIdx = BusRoute.FindIndex((item) => item.Station == first);
			if (firstIdx == -1)
				throw new ArgumentException("Couldn't find the first station");

			var secondIdx = BusRoute.FindIndex((item) => item.Station == second);
			if (secondIdx == -1)
				throw new ArgumentException("Couldn't find the second station");

			var temp = Math.Max(firstIdx, secondIdx);
			firstIdx = Math.Min(firstIdx, secondIdx);
			secondIdx = temp;

			return new Tuple<int, int>(firstIdx, secondIdx);
		}

		public override string ToString()
		{
			string result = $"Bus Line: {BusLine}\nArea: {Area}\nDirection: {Direction}\n";
			result += string.Join("->", from station in BusRoute select station.Station.Code);
			return result;
		}

		public int CompareTo(Bus other)
		{
			return (int)(RouteTime() - other.RouteTime());
		}
	}
}
