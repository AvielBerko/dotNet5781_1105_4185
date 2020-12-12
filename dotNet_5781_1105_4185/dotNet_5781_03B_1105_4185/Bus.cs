using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	public class Bus : INotifyPropertyChanged
	{
		public Bus(Registration reg)
		{
			Registration = reg;
			KmToRefuel = 1200;

			maintance = new Maintance();
			maintance.Date = DateTime.Now;
		}

		public uint Kilometrage { get; private set; }

		private Maintance maintance;
		private Status status;
		public Registration Registration { get; }
		public Status Status { get => status; private set { status = value; OnPropertyChanged(nameof(Status));} }
		public bool TreatmentNeeded => (Kilometrage - maintance.Km > 20000) || ((DateTime.Now - maintance.Date).TotalDays > 365);
		private uint kmToRefuel;
		public uint KmToRefuel { get => kmToRefuel; private set { kmToRefuel = value; OnPropertyChanged(nameof(KmToRefuel)); } }
		public double FuelLeft => ((double)KmToRefuel/1200)*100;
		public bool CanDrive(uint distance) => kmToRefuel - distance >= 0;
		public void UpdateStatusProgress(double progress)
		{
			Status = new Status(Status.Stage, progress);
		}
		public void StartTreatment()
		{
			if (Status.Stage != Stage.Ready)
				throw new Exception();
			Status = new Status(Stage.InTreatment);
		}
		public void DoneTreatment()
		{
			maintance.Date = DateTime.Now;
			maintance.Km = Kilometrage;
			Status = new Status(Stage.Ready);
		}
		public void StartRefueling()
		{
			if (Status.Stage != Stage.Ready)
				throw new Exception();
			Status = new Status(Stage.Refueling);
		}
		public void DoneRefueling()
		{
			KmToRefuel = 1200;
			Status = new Status(Stage.Ready);
		}
		public void StartDriving()
		{
			if (TreatmentNeeded || Status.Stage != Stage.Ready)
				throw new Exception();
			Status = new Status(Stage.Driving);
		}
		public void DoneDriving(uint km)
		{
			KmToRefuel -= km;
			Kilometrage += km;
			Status = new Status(Stage.Ready);
		}
		public event PropertyChangedEventHandler PropertyChanged;
		virtual protected void OnPropertyChanged(String propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
