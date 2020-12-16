using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	public readonly struct Registration
	{
		public uint Number { get; }
		public DateTime Date { get; }

		public Registration(uint number, DateTime date)
		{
			// Validates registration date and number.
			if (!(date.Year >= 2018 && (number < 100000000 && number > 9999999) ||
					date.Year < 2018 && (number < 10000000 && number > 999999)))
				throw new RegistrationException(date.Year < 2018);

			Number = number;
			Date = date;
		}
		public override string ToString()
		{
			string fmtReg = (Date.Year >= 2018) ? "000-00-000" : "00-000-00";
			return Number.ToString(fmtReg);
		}

		public static Registration Random()
		{
			bool _7digit = rnd.Next(0, 2) == 0;

			if (_7digit)
			{
				return new Registration(
					(uint)rnd.Next(1000000, 10000000),
					new DateTime(rnd.Next(1948, 2018), rnd.Next(1, 13), rnd.Next(1, 29))
					);
			}

			return new Registration(
				(uint)rnd.Next(10000000, 100000000),
				new DateTime(rnd.Next(2018, 2021), rnd.Next(1, 13), rnd.Next(1, 29))
				);
		}

		private static Random rnd = new Random();
	}
}
