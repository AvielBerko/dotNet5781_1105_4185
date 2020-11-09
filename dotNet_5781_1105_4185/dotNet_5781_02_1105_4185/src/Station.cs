using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185
{
	/// <summary>
	/// Class for storing a unique station.
	/// </summary>
	class Station : IDisposable
	{
		/// <summary>
		/// Creates new instance of a unique station.
		/// </summary>
		/// <param name="code">Unique 6 digit station identifier</param>
		/// <param name="address">Address name</param>
		public Station(uint code, string address)
		{
			Code = code;
			Location = Location.RandomizeIsraelLocation();
			Address = address;
			Stations.Add(this);
		}

		/// <summary>
		/// Stores all existing stations.
		/// </summary>
		public static List<Station> Stations { get; private set; } = new List<Station>();

		/// <summary>
		/// Unique 6 digit station's identifier
		/// </summary>
		public uint Code
		{ 
			get => code;
			private set
			{
				if (value > 99999 && value < 1000000) // 6 digits excluding 0 digit
				{
					// Checking that the station doesn't exist already.
					if (Stations.Any((station) => value == station.Code))
						throw new ArgumentException("Station code is not unique!");
					code = value;
				}
				else
					throw new ArgumentException("Station code should be 6 digits!");
			}
		}
		private uint code;

		/// <summary>
		/// Station's location.
		/// </summary>
		public Location Location { get; private set; }
		/// <summary>
		/// Station's address.
		/// </summary>
		public string Address { get; private set; }

		public override string ToString() => $"Bus Station Code: {Code}\nLocation: {Location}\nAddress: {Address} ";

		public void Dispose()
		{
			Stations.Remove(this);
		}
	}
}
