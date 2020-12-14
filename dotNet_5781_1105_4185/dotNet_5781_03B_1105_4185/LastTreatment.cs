using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	public readonly struct LastTreatment
	{
		public LastTreatment(uint km, DateTime date)
		{
			Km = km;
			Date = date;
		}

		public uint Km { get; }
		public DateTime Date { get; }

		public override string ToString() => $"The last treatment was at {Date:dd MMMM yyyy} with kilometrage {Km} km";
	}
}
