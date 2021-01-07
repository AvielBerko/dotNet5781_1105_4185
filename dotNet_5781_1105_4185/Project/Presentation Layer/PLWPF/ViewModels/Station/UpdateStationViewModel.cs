using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL
{
	public class UpdateStationViewModel : BaseDialogViewModel, IDataErrorInfo
	{
		BO.Station station;
		public BO.Station Station
		{
			get => station;
			set
			{
				station = value;
				OnPropertyChanged(nameof(Station));
				OnPropertyChanged(nameof(Code));
				OnPropertyChanged(nameof(Name));
				OnPropertyChanged(nameof(Address));
				OnPropertyChanged(nameof(Latitude));
				OnPropertyChanged(nameof(Longitude));
			}
		}

		public int Code => station.Code;

		public string Name
		{
			get => station.Name;
			set
			{
				station.Name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
		public string Address
		{
			get => station.Address;
			set
			{
				station.Address = value;
				OnPropertyChanged(nameof(Address));
			}
		}
		public double Longitude
		{
			get => station.Location.Longitude;
			set
			{
				station.Location = new BO.Location(value, station.Location.Latitude);
				OnPropertyChanged(nameof(Longitude));
			}
		}
		public double Latitude
		{
			get => station.Location.Latitude;
			set
			{
				station.Location = new BO.Location(station.Location.Longitude, value);
				OnPropertyChanged(nameof(Latitude));
			}
		}

		public RelayCommand Ok { get; }
		public RelayCommand Cancel { get; }

		public UpdateStationViewModel()
		{
			station = new BO.Station();
			Ok = new RelayCommand(_Ok, obj => ValidateStationName().IsValid &&
                                              ValidateStationAddress().IsValid &&
                                              ValidateStationLocation().IsValid);
			Cancel = new RelayCommand(_Cancel);
		}

		private void _Ok(object window)
		{
            BlWork(bl => bl.UpdateStation(station));
			CloseDialog(window, true);
		}

		private void _Cancel(object window)
		{
			CloseDialog(window, false);
		}

		public delegate void AddedStationEventHandler(object sender, BO.Station station);
		public event AddedStationEventHandler AddedStaion;
		protected virtual void OnAddedStation(BO.Station station) => AddedStaion?.Invoke(this, station);

		public string this[string columnName]
		{
			get
			{
				switch (columnName)
				{
					case nameof(Name):
						return ValidateStationName().ErrorContent as string;
					case nameof(Address):
						return ValidateStationAddress().ErrorContent as string;
					case nameof(Longitude):
					case nameof(Latitude):
						return ValidateStationLocation().ErrorContent as string;
					default:
						return null;
				}
			}
		}
		public string Error => throw new NotImplementedException();

		private ValidationResult ValidateStationName()
		{
			try
			{
				BlWork(bl => bl.ValidateStationName(station.Name));
				return ValidationResult.ValidResult;
			}
			catch (BO.BadStationNameException ex)
			{
				return new ValidationResult(false, ex.Message);
			}
		}
		private ValidationResult ValidateStationAddress()
		{
			try
			{
				BlWork(bl => bl.ValidateStationAddress(station.Address));
				return ValidationResult.ValidResult;
			}
			catch (BO.BadStationAddressException ex)
			{
				return new ValidationResult(false, ex.Message);
			}
		}
		private ValidationResult ValidateStationLocation()
		{
			try
			{
				BlWork(bl => bl.ValidateStationLocation(station.Location));
				return ValidationResult.ValidResult;
			}
			catch (BO.BadStationLocationException ex)
			{
				return new ValidationResult(false, ex.Message);
			}
		}
	}
}
