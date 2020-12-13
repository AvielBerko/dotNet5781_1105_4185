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
		public Bus(Registration reg, uint kilometrage = 0)
		{
			Registration = reg;
			KmToRefuel = 1200;
			Kilometrage = kilometrage;

			lastTreatment = new LastTreatment() { Date = DateTime.Now };
		}

		public Registration Registration { get; }

		private uint kilometrage;
		public uint Kilometrage
		{
			get => kilometrage;
			private set
			{
				kilometrage = value;
				OnPropertyChanged(nameof(Kilometrage));
			}
		}

		private Status status;
		public Status Status
		{
			get => status;
			private set
			{
				status = value;
				OnPropertyChanged(nameof(Status));
			}
		}

		private Operation operation;
		public Operation Operation {
			get => operation;
			set
			{
				operation = value;
				OnPropertyChanged(nameof(Operation));
			}
		}

		private LastTreatment lastTreatment;
		public bool TreatmentNeeded => (Kilometrage - lastTreatment.Km > 20000)
			|| ((DateTime.Now - lastTreatment.Date).TotalDays > 365);

		private int kmToRefuel;
		public int KmToRefuel
		{
			get => kmToRefuel;
			private set
			{
				kmToRefuel = value;
				OnPropertyChanged(nameof(KmToRefuel));
				OnPropertyChanged(nameof(FuelLeft));
			}
		}
		public double FuelLeft => ((double)KmToRefuel / 1200) * 100;
		public bool CanDrive(uint distance) => kmToRefuel - distance >= 0;

		public void UpdateOperationTimeLeft(TimeSpan timeLeft)
		{
			if (Status == Status.Ready) throw new Exception();

			Operation.TimeLeft = timeLeft;
			OnPropertyChanged(nameof(Operation));
		}
		public void StartTreatment()
		{
			if (Status != Status.Ready)
				throw new Exception();

			Status = Status.InTreatment;
			Operation = new TreatmentOperation();
		}
		public void DoneTreatment()
		{
			lastTreatment.Date = DateTime.Now;
			lastTreatment.Km = Kilometrage;
			if (kmToRefuel == 0) KmToRefuel = 1200;

			Status = Status.Ready;
			Operation = null;
		}
		public void StartRefueling()
		{
			if (Status != Status.Ready)
				throw new Exception();

			Status = Status.Refueling;
			Operation = new RefuelingOperation();
		}
		public void DoneRefueling()
		{
			KmToRefuel = 1200;

			Status = Status.Ready;
			Operation = null;
		}
		public void StartDriving(uint km)
		{
			if (TreatmentNeeded || Status != Status.Ready)
				throw new Exception();

			uint kmPerHour = (uint)rnd.Next(40, 80);
			Status = Status.Driving;
			Operation = new DrivingOperation { Distance = km, Speed = kmPerHour };
		}
		public void DoneDriving()
		{
			var drivingData = (DrivingOperation)Operation;
			KmToRefuel -= (int)drivingData.Distance;
			Kilometrage += drivingData.Distance;
			Status = Status.Ready;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		virtual protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private static Random rnd = new Random();
	}
}
