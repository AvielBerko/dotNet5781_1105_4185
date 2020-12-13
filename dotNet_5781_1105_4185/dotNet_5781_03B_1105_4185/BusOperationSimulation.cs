using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	public static class BusOperationSimulation
	{
		public static void Drive(this Bus bus, uint distance)
		{
			CreateOperation(bus, () => bus.StartDriving(distance), (sender, e) => bus.DoneDriving());
		}
		public static void Refuel(this Bus bus)
		{
			CreateOperation(bus, () => bus.StartRefueling(), (sender, e) => bus.DoneRefueling());
		}
		public static void Treate(this Bus bus)
		{
			CreateOperation(bus, () => bus.StartTreatment(), (sender, e) => bus.DoneTreatment());
		}

		private static void CreateOperation(Bus bus, Action start, RunWorkerCompletedEventHandler completed)
		{
			start();

			var bg = new BackgroundWorker();
			bg.WorkerReportsProgress = true;
			bg.DoWork += (sender, e) =>
			{
				var worker = (BackgroundWorker)sender;
				var time = (TimeSpan)e.Argument;
				var totalMinutes = time.TotalMinutes;
				for (int i = 0; i < totalMinutes; i++)
				{
					int progress = (int)(i / totalMinutes);
					worker.ReportProgress(progress, TimeSpan.FromMinutes(totalMinutes - i));
					Thread.Sleep(100);
				}
			};
			bg.ProgressChanged += (sender, e) =>
			{
				var timeLeft = (TimeSpan)e.UserState;
				bus.UpdateOperationTimeLeft(timeLeft);
			};
			bg.RunWorkerCompleted += completed;

			bg.RunWorkerAsync(bus.Operation.Time);
		}
	}
}
