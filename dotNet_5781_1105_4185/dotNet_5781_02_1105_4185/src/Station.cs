using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185
{
	class Station
	{
		public Station(uint code, string address)
		{
			Code = code;
			Location = Location.RandomizeLocation();
			Address = address;
			stations.Add(this);
		}
		private static List<Station> stations = new List<Station>();
		private uint code;
		public uint Code
		{ 
			get => code;
			private set
			{
				if (value > 99999 && value < 1000000) // 6 digits
				{
					if (stations.Any((station) => value == station.Code))
						throw new ArgumentException("Station code is not unique!", nameof(value));
					code = value;
				}
				throw new ArgumentOutOfRangeException("Station code should be 6 digits!", nameof(value));
			}
		}
		public Location Location { get; private set; }
		public string Address { get; private set; }

		public override string ToString() => $"Bus Station Code: {Code}\nLocation: {Location}\nAddress: {Address} ";
	}
}
