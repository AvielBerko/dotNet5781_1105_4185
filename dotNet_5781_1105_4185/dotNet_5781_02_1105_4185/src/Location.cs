using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185
{
	struct Location
	{
		public Location(double lat, double lon) 
		{
			if (lat < -90 && lat > 90)
				throw new ArgumentOutOfRangeException("Latitude should be between -90 and 90", nameof(lat));
			if (lon < -180 && lon > 180)
				throw new ArgumentOutOfRangeException("Longitude should be between -180 and 180", nameof(lon));

			Latitude = lat;
			Longitude = lon;
		}

		public double Latitude { get; }
		public double Longitude { get; }

		private static Random rand = new Random();
		public static Location RandomizeLocation()
		{
			return new Location(31 + 2.3 * rand.NextDouble(), 34.3 + 1.2 * rand.NextDouble());
		}
		public override string ToString() => $"{Latitude}°N {Longitude}°E";
	}
}
