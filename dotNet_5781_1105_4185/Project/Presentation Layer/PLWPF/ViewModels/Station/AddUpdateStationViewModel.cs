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
        private ObservableCollection<AdjacentStationViewModel> _adjacentStations;
        public ObservableCollection<AdjacentStationViewModel> AdjacentStations
        {
            get => _adjacentStations;
            set
            {
                _adjacentStations = value;
                OnPropertyChanged(nameof(AdjacentStations));
            }
        }

        BO.Station _station;
        public BO.Station Station
        {
            get => _station;
            set
            {
                _station = value;
                OnPropertyChanged(nameof(Station));
                OnPropertyChanged(nameof(Longitude));
                OnPropertyChanged(nameof(Latitude));
            }
        }
        public double Longitude
        {
            get => _station.Location.Longitude;
            set
            {
                _station.Location = new BO.Location(value, _station.Location.Latitude);
                OnPropertyChanged(nameof(Longitude));
            }
        }
        public double Latitude
        {
            get => _station.Location.Latitude;
            set
            {
                _station.Location = new BO.Location(_station.Location.Longitude, value);
                OnPropertyChanged(nameof(Latitude));
            }
        }
        public bool IsUpdate { get; }

        public RelayCommand Ok { get; }
        public RelayCommand Cancel { get; }
        public RelayCommand AddAdjacent { get; }


        public AddUpdateStationViewModel(int? stationCode = null)
        {
            _station = new BO.Station();
            AdjacentStations = new ObservableCollection<AdjacentStationViewModel>();

            IsUpdate = stationCode != null;
            if (IsUpdate)
            {
                _ = GetStationFromBL(stationCode ?? 0);
            }

            Ok = new RelayCommand(async obj => await _Ok(obj),
                obj => ValidateStationCode().IsValid &&
                       ValidateStationName().IsValid &&
                       ValidateStationAddress().IsValid &&
                       ValidateStationLatitude().IsValid &&
                       ValidateStationLongitude().IsValid);
            Cancel = new RelayCommand(_Cancel);
            AddAdjacent = new RelayCommand(obj => _AddAdjacent());
        }

        private async Task GetStationFromBL(int stationCode)
        {
            await Load(async () =>
            {
                Station = (BO.Station)(await BlWorkAsync(bl => bl.GetStation(stationCode)));
                AdjacentStations = new ObservableCollection<AdjacentStationViewModel>(
                    from ad in Station.AdjacentStations
                    select _CreateAdjacentVM(ad)
                );
            });
        }

        private AdjacentStationViewModel _CreateAdjacentVM(BO.AdjacentStations adjacents)
        {
            var result = new AdjacentStationViewModel(adjacents, Station.Code);
            result.Remove += (sender) => AdjacentStations.Remove(result);

            return result;
        }

        private void _AddAdjacent()
        {
            var currentStations = (from adj in AdjacentStations
                                   select adj.Adjacents.GetOtherStation(Station.Code))
                                   .Append(_station);

            var vm = new SelectStationsViewModel(currentStations);
            if (DialogService.ShowSelectStationsDialog(vm) == DialogResult.Ok)
            {
                var addedAdjacents = from st in vm.SelectedStations
                                     select new BO.AdjacentStations
                                     {
                                         Station1 = Station,
                                         Station2 = st
                                     };
                foreach (var ad in addedAdjacents)
                {
                    AdjacentStations.Add(_CreateAdjacentVM(ad));
                }
            }
        }

        private async Task _Ok(object window)
        {
            await Load(async () =>
            {
                _station.AdjacentStations = from ad in AdjacentStations
                                            select ad.Adjacents;
                if (IsUpdate)
                    await BlWorkAsync(bl => bl.UpdateStation(_station));
                else
                    await BlWorkAsync(bl => bl.AddStation(_station));

                DialogService.CloseDialog(window, DialogResult.Ok);
            });
        }

        private void _Cancel(object window)
        {
            DialogService.CloseDialog(window, DialogResult.Cancel);
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Station.Code):
                        return ValidateStationCode().ErrorContent as string;
                    case nameof(Station.Name):
                        return ValidateStationName().ErrorContent as string;
                    case nameof(Station.Address):
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
                BlWork(bl => bl.ValidateNewStationCode(_station.Code));
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
                BlWork(bl => bl.ValidateNewStationName(_station.Name));
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
                BlWork(bl => bl.ValidateNewStationAddress(_station.Address));
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
                BlWork(bl => bl.ValidateNewStationLatitude(_station.Location));
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
                BlWork(bl => bl.ValidateNewStationLongitude(_station.Location));
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
