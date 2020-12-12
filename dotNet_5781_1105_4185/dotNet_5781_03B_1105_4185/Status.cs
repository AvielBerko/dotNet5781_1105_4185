using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	public enum Stage { Ready, Driving, Refueling, InTreatment };
	public readonly struct Status
	{
		public Status(Stage stage, double progress = 0)
		{
			if (progress < 0 || progress > 1) throw new Exception();

			Stage = stage;
			Progress = progress;
		}
		public Stage Stage { get; }
		public double Progress { get; }
	}
}
