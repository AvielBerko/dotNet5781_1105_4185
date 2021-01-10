using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL
{
	public class UpdateStationViewModel : BaseDialogViewModel, IDataErrorInfo
	{
		public ObservableCollection<AdjacentStationViewModel> AdjacentStations { get; }
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
		public RelayCommand AddAdjacent { get; }

		public UpdateStationViewModel(int stationCode)
		{
			Station = (BO.Station)BlWork(bl => bl.GetStation(stationCode));
			AdjacentStations = new ObservableCollection<AdjacentStationViewModel>
				(from ad in Station.AdjacentStations select _CreateAdjacentVM(ad));

			Ok = new RelayCommand(_Ok, obj => ValidateStationName().IsValid &&
                                              ValidateStationAddress().IsValid &&
                                              ValidateStationLocation().IsValid);
			Cancel = new RelayCommand(_Cancel);
			AddAdjacent = new RelayCommand(obj => _AddAdjacent());
		}

		private AdjacentStationViewModel _CreateAdjacentVM(BO.AdjacentStations adjacents)
		{
			var result = new AdjacentStationViewModel(adjacents);
			result.Remove += (sender) => AdjacentStations.Remove(result); 

			return result;
		}
		private void _AddAdjacent()
		{
			var currentStations = (from ad in AdjacentStations select ad.Adjacent.ToStation).Append(Station);
			var restStations = (IEnumerable<BO.Station>)BlWork(bl => bl.GetRestOfStations(currentStations));
			var vm = new AddAdjacentViewModel(restStations);
			if (DialogService.ShowAddAdjacentDialog(vm) == true)
			{
				var addedAdjacents = from st in vm.SelectedStations select new BO.AdjacentStations { FromStation = Station, ToStation = st };
				foreach (var ad in addedAdjacents)
					AdjacentStations.Add(_CreateAdjacentVM(ad));
			}
		}
		private void _Ok(object window)
		{
			Station.AdjacentStations = from ad in AdjacentStations select ad.Adjacent;
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
