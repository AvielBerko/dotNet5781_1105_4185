using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	public class DrivingOperation : Operation
	{
		public uint Distance { get; set; }
		public uint Speed { get; set; }
		public override TimeSpan Time => TimeSpan.FromHours(Distance / Speed);
	}

	public class RefuelingOperation : Operation
	{
		public override TimeSpan Time => TimeSpan.FromHours(2);
	}

	public class TreatmentOperation : Operation
	{
		public override TimeSpan Time => TimeSpan.FromDays(1);
	}

	public abstract class Operation
	{
		public abstract TimeSpan Time { get; }
		public TimeSpan TimeLeft { get; set; }
		public double Progress => (Time.TotalSeconds - TimeLeft.TotalSeconds) / Time.TotalSeconds * 100;
	}
}
