using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_01_1105_4185
{
	class Bus
	{
		public Bus(string reg, DateTime date)
		{
			DateRegistered = date;
			Registration = reg;
			KmToRefuel = 1200;
		}
		public DateTime DateRegistered { get; private set; }

		public string Registration
		{
			get => registration;
			set
			{
				if (uint.TryParse(value, out _))
				{
					if (DateRegistered.Year >= 2018 && value.Length == 8 ||
						DateRegistered.Year < 2018 && value.Length == 7)
					{
						registration = value;
						//registration = string.Format("{0}-{1}-{2}", value.Substring(0, 3), value.Substring(3, 2), value.Substring(5, 3));
					}
					/*else if (value.Length == 7)
					{
						registration = string.Format("{0}-{1}-{2}", value.Substring(0, 2), value.Substring(2, 3), value.Substring(4, 2));
					}*/
					else
					{
						throw new Exception("Invalid registration");
					}
				}
				else
				{
					throw new Exception("Invalid registration");
				}
			}
		}
		private string registration;

		public uint Kilometrage { get; private set; }

		private uint lastTreatmentKm;
		private DateTime lastTreatmentDate;
		public bool Risky => (Kilometrage - lastTreatmentKm > 20000);
		public uint KmToRefuel { get; private set; }

		public void Treatment()
		{
			lastTreatmentDate = DateTime.Now;
			lastTreatmentKm = Kilometrage;
		}
		public void Refuel()
		{
			KmToRefuel = 1200;
		}
		public void Drive(uint km)
		{
			if (KmToRefuel >= km)
			{
				KmToRefuel -= km;
				Kilometrage += km;
			}
			else
			{
				throw new Exception("Cannot drive the distance, refuel needed");
			}
		}
		public override string ToString()
		{
			string fmtReg;
			if (registration.Length == 8)
				fmtReg = $"{registration.Substring(0, 3)}-{registration.Substring(3, 2)}-{ registration.Substring(5, 3)}";
			else
				fmtReg = $"{registration.Substring(0, 2)}-{registration.Substring(2, 3)}-{ registration.Substring(4, 2)}";

			return $"Registration Number: {fmtReg}, Km since last treatment: {Kilometrage - lastTreatmentKm}";
		}
	}
}
