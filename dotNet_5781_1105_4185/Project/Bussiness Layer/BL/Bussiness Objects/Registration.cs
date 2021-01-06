using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{ 
	public readonly struct Registration
	{
		public int Number { get; }
		public DateTime Date { get; }

		public Registration(int number, DateTime date)
		{
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
