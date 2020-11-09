using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185
{
	class Station : IDisposable
	{
		public Station(uint code, string address)
		{
			Code = code;
			Location = Location.RandomizeIsraelLocation();
			Address = address;
			Stations.Add(this);
		}
		public static List<Station> Stations { get; private set; } = new List<Station>();

		private uint code;
		public uint Code
		{ 
			get => code;
			private set
			{
				if (value > 99999 && value < 1000000) // 6 digits
				{
					if (Stations.Any((station) => value == station.Code))
						throw new ArgumentException("Station code is not unique!");
					code = value;
				}
				else
					throw new ArgumentException("Station code should be 6 digits!");
			}
		}
		public Location Location { get; private set; }
		public string Address { get; private set; }

		public override string ToString() => $"Bus Station Code: {Code}\nLocation: {Location}\nAddress: {Address} ";

		public void Dispose()
		{
			Stations.Remove(this);
		}
	}
}
