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
    public class AddUpdateStationViewModel : BaseViewModel, IDataErrorInfo
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

        public int Code
        {
            get => station.Code;
            set
            {
                station.Code = value;
                OnPropertyChanged(nameof(Code));
            }
        }
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
		public bool IsUpdate { get; set; }

		public RelayCommand Ok { get; }
        public RelayCommand Cancel { get; }
        public RelayCommand AddAdjacent { get; }


        public AddUpdateStationViewModel(int? stationCode = null)
        {
            if (stationCode == null)
            {
                station = new BO.Station();
                AdjacentStations = new ObservableCollection<AdjacentStationViewModel>();
                IsUpdate = false;
            }
            else
			{
                Station = (BO.Station)BlWork(bl => bl.GetStation(stationCode ?? 0));
                AdjacentStations = new ObservableCollection<AdjacentStationViewModel>
                    (from ad in Station.AdjacentStations select _CreateAdjacentVM(ad));
                IsUpdate = true;
            }
            Ok = new RelayCommand(_Ok, obj => ValidateStationCode().IsValid &&
                                              ValidateStationName().IsValid &&
                                              ValidateStationAddress().IsValid &&
                                              ValidateStationLatitude().IsValid &&
                                              ValidateStationLongitude().IsValid);
            Cancel = new RelayCommand(_Cancel);
            AddAdjacent = new RelayCommand(obj => _AddAdjacent());
        }
        private AdjacentStationViewModel _CreateAdjacentVM(BO.AdjacentStation adjacents)
        {
            var result = new AdjacentStationViewModel(adjacents);
            result.Remove += (sender) => AdjacentStations.Remove(result);

            return result;
        }
        private void _AddAdjacent()
        {
            var currentStations = (from ad in AdjacentStations select ad.Adjacent.ToStation).Append(station);
            var restStations = (IEnumerable<BO.Station>)BlWork(bl => bl.GetRestOfStations(currentStations));
            var vm = new SelectStationsViewModel(restStations);
            if (DialogService.ShowSelectStationsDialog(vm) == DialogResult.Ok)
            {
                var addedAdjacents = from st in vm.SelectedStations select new BO.AdjacentStation { ToStation = st };
                foreach (var ad in addedAdjacents)
                    AdjacentStations.Add(_CreateAdjacentVM(ad));
            }
        }
        private void _Ok(object window)
        {
            if (IsUpdate)
            {
                Station.AdjacentStations = from ad in AdjacentStations select ad.Adjacent;
                BlWork(bl => bl.UpdateStation(station));
            }
            else
            {
                station.AdjacentStations = from ad in AdjacentStations select ad.Adjacent;
                BlWork(bl => bl.AddStation(station));
                OnAddedStation(station);
            }
            DialogService.CloseDialog(window, DialogResult.Ok);
        }

        private void _Cancel(object window)
        {
            DialogService.CloseDialog(window, DialogResult.Ok);
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
                    case nameof(Code):
                        return ValidateStationCode().ErrorContent as string;
                    case nameof(Name):
                        return ValidateStationName().ErrorContent as string;
                    case nameof(Address):
                        return ValidateStationAddress().ErrorContent as string;
                    case nameof(Longitude):
                        return ValidateStationLongitude().ErrorContent as string;
                    case nameof(Latitude):
                        return ValidateStationLatitude().ErrorContent as string;
                    default:
                        return null;
                }
            }
        }
        public string Error => throw new NotImplementedException();


        private ValidationResult ValidateStationCode()
        {
            if (IsUpdate) return ValidationResult.ValidResult;
            try
            {
                BlWork(bl => bl.ValidateStationCode(station.Code));
                return ValidationResult.ValidResult;
            }
            catch (BO.BadStationCodeException ex)
            {
                return new ValidationResult(false, ex.Message);
            }
        }
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
        private ValidationResult ValidateStationLatitude()
        {
            try
            {
                BlWork(bl => bl.ValidateStationLatitude(station.Location));
                return ValidationResult.ValidResult;
            }
            catch (BO.BadLocationLatitudeException ex)
            {
                return new ValidationResult(false, ex.Message);
            }
            catch (BO.BadLocationLongitudeException)
            {
                return ValidationResult.ValidResult;
            }
        }
        private ValidationResult ValidateStationLongitude()
        {
            try
            {
                BlWork(bl => bl.ValidateStationLongitude(station.Location));
                return ValidationResult.ValidResult;
            }
            catch (BO.BadLocationLatitudeException)
            {
                return ValidationResult.ValidResult;
            }
            catch (BO.BadLocationLongitudeException ex)
            {
                return new ValidationResult(false, ex.Message);
            }
        }
    }
}
