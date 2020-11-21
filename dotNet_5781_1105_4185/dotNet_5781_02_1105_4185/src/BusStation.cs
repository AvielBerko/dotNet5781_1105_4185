using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dotNet_5781_02_1105_4185
{
	/// <summary>
	/// A station wrapper for saving more bus specific information.
	/// </summary>
	public struct BusStation
	{
		/// <summary>
		/// Creates an instance of BusStation.
		/// </summary>
		/// <param name="station">The station</param>
		/// <param name="dist">The distance (m) from last station.</param>
		/// <param name="time">The time (s) from last station.</param>
		public BusStation(Station station, double dist, double time)
		{
			Station = station;
			DistanceFromLastStation = dist;
			TimeFromLastStation = time;
		}
		/// <summary>
		/// The station.
		/// </summary>
		public Station Station { get; private set; }
		/// <summary>
		/// Distance in meters from last station.
		/// </summary>
		public double DistanceFromLastStation { get; private set; }
		/// <summary>
		/// Time in minutes from last station.
		/// </summary>
		public double TimeFromLastStation { get; private set; }

		public static bool operator==(BusStation A, BusStation B) => A.Station == B.Station;
		public static bool operator !=(BusStation A, BusStation B) => !(A == B);
		public override string ToString() => $"{Station}\nDistance: {DistanceFromLastStation:n3} meters\nTime: {TimeFromLastStation:n3} minutes";

	}
}
