using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_01_1105_4185
{
	class Bus
	{
		public DateTime DateRegistered { get; private set; }

		public string Registration
		{
			get => registration;
			set
			{
				if (DateRegistered.Year >= 2018 && value.Length == 8 || value.Length == 7)
				{
					registration = value;
				}
				else
				{
					throw new Exception("Invalid registration");
				}
			}
		}
		private string registration;

		public uint Kilometrage { get; private set; }

		private uint lastKmTreatment;
	}
}
