using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	public enum Status { Ready, NeedRefueling ,NeedTreatment, Driving, Refueling, InTreatment };
	public class Bus : INotifyPropertyChanged
	{
		public static ObservableCollection<Bus> Buses { get; set; } = new ObservableCollection<Bus>();

		public Bus(Registration reg, uint kilometrage = 0, uint kmToRefuel = 1200, LastTreatment? lastTreatment = null)
		{
			this.lastTreatment = lastTreatment ?? new LastTreatment(0, DateTime.Now);

			Registration = reg;
			KmToRefuel = kmToRefuel;
			Kilometrage = kilometrage;

			UpdateStatus();

			Bus existing = Buses.FirstOrDefault((bus) => bus.Registration.Number == reg.Number);
			if (existing != null)
				throw new BusExistingException(existing);

			Buses.Add(this);
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
		public bool InOperation => Status != Status.Ready && Status != Status.NeedTreatment && Status != Status.NeedRefueling;

		private Operation operation;
		public Operation Operation
		{
			get => operation;
			set
			{
				operation = value;
				OnPropertyChanged(nameof(Operation));
			}
		}

		private LastTreatment lastTreatment;
		public LastTreatment LastTreatment
		{
			get => lastTreatment;
			private set
			{
				lastTreatment = value;
				OnPropertyChanged(nameof(LastTreatment));
			}
		}
		public bool TreatmentNeeded => (Kilometrage - LastTreatment.Km > 20000)
			|| ((DateTime.Now - LastTreatment.Date).TotalDays > 365);

		private uint kmToRefuel;
		public uint KmToRefuel
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
		public bool CanDriveDistance(uint distance) => kmToRefuel >= distance;

		public void UpdateOperationTimeLeft(TimeSpan timeLeft)
		{
			if (!InOperation)
				throw new BusException($"Cannot update TimeLeft when the bus status is {Status}", this);

			Operation.TimeLeft = timeLeft;
			OnPropertyChanged(nameof(Operation));
		}
		public void StartTreatment()
		{
			if (InOperation)
				throw new BusException($"Cannot start treatment if the bus status is {Status}", this);

			Status = Status.InTreatment;
			Operation = new TreatmentOperation();
		}
		public void DoneTreatment()
		{
			if (Status != Status.InTreatment)
				throw new BusException("Tried to finish treatment before started.", this);

			LastTreatment = new LastTreatment(Kilometrage, DateTime.Now);
			KmToRefuel = 1200;

			UpdateStatus();
		}
		public void StartRefueling()
		{
			if (InOperation)
				throw new BusException($"Cannot start refueling if the bus status is {Status}", this);

			Status = Status.Refueling;
			Operation = new RefuelingOperation();
		}
		public void DoneRefueling()
		{
			if (Status != Status.Refueling)
				throw new BusException("Tried to finish refueling before started.", this);

			KmToRefuel = 1200;

			UpdateStatus();
		}
		public void StartDriving(uint km)
		{
			if (Status != Status.Ready)
				throw new BusException("Cannot start driving if the bus is not ready", this);
			if (TreatmentNeeded)
				throw new BusException("Cannot start driving if the bus need a treatment", this);
			if (!CanDriveDistance(km))
				throw new BusException("The bus don't have enough fuel to drive this distance", this);

			uint kmPerHour = (uint)rnd.Next(40, 80);
			Status = Status.Driving;
			Operation = new DrivingOperation { Distance = km, Speed = kmPerHour };
		}
		public void DoneDriving()
		{
			if (Status != Status.Driving)
				throw new BusException("Tried to finish driving before started.", this);

			var drivingData = (DrivingOperation)Operation;
			KmToRefuel -= drivingData.Distance;
			Kilometrage += drivingData.Distance;

			UpdateStatus();
		}

		private void UpdateStatus()
		{
			Status = TreatmentNeeded ? Status.NeedTreatment :
				(KmToRefuel < 360 ? Status.NeedRefueling : Status.Ready);
			Operation = null;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		virtual protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public static Bus Random(bool needTreatmentForTime = false,
								bool needTreatmentForDistance = false,
								bool lowFuel = false)
		{
			int kilometrage = rnd.Next(20000, 250000);
			int lastTreatmentKm =
				needTreatmentForDistance
				? rnd.Next(0, kilometrage - 19999)
				: rnd.Next(kilometrage - 19999, kilometrage);

			return new Bus(
				Registration.Random(),
				(uint)kilometrage,
				(uint)(lowFuel ? rnd.Next(0, 361) : rnd.Next(361, 1200)),
				new LastTreatment
				(
					km: (uint)lastTreatmentKm,
					date: needTreatmentForTime
						? DateTime.Now.AddDays(rnd.Next(-1200, -365))
						: DateTime.Now.AddDays(rnd.Next(-364, 1))
				));
		}

		public static void Shuffle()
		{
			Bus.Buses = new ObservableCollection<Bus>(Bus.Buses.OrderBy(bus => rnd.NextDouble()));
		}

		private static Random rnd = new Random();
	}
}
