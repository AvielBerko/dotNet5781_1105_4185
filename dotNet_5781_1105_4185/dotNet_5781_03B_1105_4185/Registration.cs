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
			if (!(date.Year >= 2018 && (number < 100000000 && number > 9999999) ||
					date.Year < 2018 && (number < 10000000 && number > 999999)))
				throw new Exception();
			Number = number;
			Date = date;
		}
		public override string ToString()
		{
			string fmtReg = (Date.Year >= 2018) ? "000-00-000" : "00-000-00";
			return Number.ToString(fmtReg);
		}
	}
}
