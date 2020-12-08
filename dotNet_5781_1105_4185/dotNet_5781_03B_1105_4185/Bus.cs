using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	public enum Status { Ready, Driving, Refueling, InTreatment };
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
		public void StartTreatment()
		{
			if (Status == Status.Driving)
				throw new Exception();
			Status = Status.InTreatment;
		}
		public void DoneTreatment()
		{
			maintance.Date = DateTime.Now;
			maintance.Km = Kilometrage;
			Status = Status.Ready;
		}
		public void StartRefueling()
		{
			if (Status == Status.Driving)
				throw new Exception();
			Status = Status.Refueling;
		}
		public void DoneRefueling()
		{
			KmToRefuel = 1200;
			Status = Status.Ready;
		}
		public void StartDriving()
		{
			if (TreatmentNeeded)
				throw new Exception();
			Status = Status.Driving;
		}
		public void DoneDriving(uint km)
		{
			KmToRefuel -= km;
			Kilometrage += km;
			Status = Status.Ready;
		}
		/*public override string ToString()
		{
			string fmtReg = (DateRegistered.Year >= 2018) ? "000-00-000" : "00-000-00";
			/*if (DateRegistered.Year >= 2018)
				fmtReg = registration.ToString("000-00-000");
			//$"{registration.Substring(0, 3)}-{registration.Substring(3, 2)}-{ registration.Substring(5, 3)}";
			else
				fmtReg = registration.ToString("00-000-00");
			//$"{registration.Substring(0, 2)}-{registration.Substring(2, 3)}-{ registration.Substring(5, 2)}";
			return $"Registration Number: {registration:fmtReg}, Km since last treatment: {Kilometrage - maintance.Km}";
		}*/
		public event PropertyChangedEventHandler PropertyChanged;
		virtual protected void OnPropertyChanged(String propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
