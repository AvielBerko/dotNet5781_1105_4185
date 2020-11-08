﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185.src
{
	enum Areas { General, North, South, Center, Jerusalem, Eilat }
	class Bus
	{
		public Bus(uint line, Areas area, List<BusStation> route)
		{
			BusLine = line;
			Area = area;
			BusRoute = route;
			buses.Add(this);
		}
		private static List<Bus> buses = new List<Bus>();
		private List<BusStation> busRoute;
		public List<BusStation> BusRoute
		{
			get => busRoute;
			private set
			{
				if (value.Count < 2)
					throw new ArgumentException("Bus route should contain at least 2 stations", nameof(value));
				busRoute = value;
			}
		}

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

		public void InsertStation(BusStation newStation, BusStation afterStation)
		{
			var index = BusRoute.IndexOf(afterStation);
			if (index == -1)
				throw new ArgumentException("Couldn't find station to add after", nameof(afterStation));

			BusRoute.Insert(index + 1, newStation);
		}

		public void RemoveStation(BusStation station) => BusRoute.Remove(station);

		public bool InRoute(BusStation station) => BusRoute.Contains(station);

		public double RouteDistance(BusStation start, BusStation end)
		{
			var indices = GetIndex(start, end);
			double result = 0;
			for (int i = indices.Item1 + 1; i < indices.Item2; i++)
			{
				result += BusRoute[i].DistanceFromLastStation;
			}

			return result;
		}
		public double RouteTime(BusStation start, BusStation end)
		{
			var indices = GetIndex(start, end);
			double result = 0;
			for (int i = indices.Item1 + 1; i < indices.Item2; i++)
			{
				result += BusRoute[i].TimeFromLastStation;
			}

			return result;
		}

		private Tuple<int, int> GetIndex(BusStation first, BusStation second)
		{
			var firstIdx = BusRoute.IndexOf(first);
			if (firstIdx == -1)
				throw new ArgumentException("Couldn't find the first station", nameof(first));

			var secondIdx = BusRoute.IndexOf(second);
			if (secondIdx == -1)
				throw new ArgumentException("Couldn't find the second station", nameof(second));

			var temp = Math.Max(firstIdx, secondIdx);
			firstIdx = Math.Min(firstIdx, secondIdx);
			secondIdx = temp;

			return new Tuple<int, int>(firstIdx, secondIdx);
		}

		public override string ToString()
		{
			string result = $"Bus Line: {BusLine}\nArea: {Area}\n";
			result += string.Join("->", from station in BusRoute select station.Code);
			return result;
		}
	}
}