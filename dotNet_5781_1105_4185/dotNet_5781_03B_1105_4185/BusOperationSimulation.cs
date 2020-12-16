using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	/// <summary>
	/// Extension functions for the Bus in order to simulate its operations.
	/// </summary>
	public static class BusOperationSimulation
	{
		public static void Drive(this Bus bus, uint distance)
		{
			CreateOperation(bus, () => bus.StartDriving(distance), () => bus.DoneDriving());
		}
		public static void Refuel(this Bus bus)
		{
			CreateOperation(bus, () => bus.StartRefueling(), () => bus.DoneRefueling());
		}
		public static void Treate(this Bus bus)
		{
			CreateOperation(bus, () => bus.StartTreatment(), () => bus.DoneTreatment());
		}

		/// <summary>
		/// Starts a generic operation
		/// </summary>
		/// <param name="start">Action to start the generic operation</param>
		/// <param name="done">Action finish the generic operation</param>
		private static void CreateOperation(Bus bus, Action start, Action done)
		{
			// starts the operation.
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

					// sleeps for 1 minute in simulation time.
					Thread.Sleep(100);
				}
			};
			bg.ProgressChanged += (sender, e) =>
			{
				// updating the time left of the operation.
				var timeLeft = (TimeSpan)e.UserState;
				bus.UpdateOperationTimeLeft(timeLeft);
			};

			// finshing the operation.
			bg.RunWorkerCompleted += (sender, e) => done();

			// starting the simulation.
			bg.RunWorkerAsync(bus.Operation.Time);
		}
	}
}
