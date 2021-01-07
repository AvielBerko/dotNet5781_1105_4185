using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	public struct Location
	{
		public double Longitude { get; set; }
		public double Latitude { get; set; }

		public Location(double lon, double lat)
		{
			Longitude = lon;
			Latitude = lat;
		}
		public override string ToString() => $"{Latitude:n5}°N, {Longitude:n5}°E";

	}
}
