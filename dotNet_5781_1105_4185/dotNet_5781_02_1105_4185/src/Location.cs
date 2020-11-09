using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185
{
	readonly struct Location
	{
		public Location(double lat, double lon) 
		{
			if (lat < -90 && lat > 90)
				throw new ArgumentException("Latitude should be between -90 and 90");
			if (lon < -180 && lon > 180)
				throw new ArgumentException("Longitude should be between -180 and 180");

			Latitude = lat;
			Longitude = lon;
		}

		public double Latitude { get; }
		public double Longitude { get; }

		private static Random rand = new Random();
		public static Location RandomizeIsraelLocation()
		{
			return new Location(31 + 2.3 * rand.NextDouble(), 34.3 + 1.2 * rand.NextDouble());
		}
		public override string ToString() => $"{Latitude:n3}°N, {Longitude:n3}°E";
	}
}
