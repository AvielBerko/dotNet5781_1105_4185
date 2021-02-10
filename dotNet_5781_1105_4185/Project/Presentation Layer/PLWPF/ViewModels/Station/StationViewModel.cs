using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class StationViewModel : BaseViewModel
    {
        private BO.Station _station;
        public BO.Station Station
        {
            get => _station;
            set
            {
                _station = value;
                OnPropertyChanged(nameof(Station));
            }
        }
        public RelayCommand StationDetails { get; }
        public RelayCommand RemoveStation { get; }
        public RelayCommand UpdateStation { get; }

        public StationViewModel(BO.Station station)
        {
            Station = station;

            UpdateStation = new RelayCommand(obj => _Update());
            StationDetails = new RelayCommand(obj => _Details());
            RemoveStation = new RelayCommand(async obj => await _Remove());
        }

        private void _Update()
        {
            var updateVM = new AddUpdateStationViewModel(Station.Code);
            if (DialogService.ShowAddUpdateStationDialog(updateVM) == DialogResult.Ok)
            {
                Station = updateVM.Station;
                OnPropertyChanged(nameof(Station));
            }
        }

        private void _Details()
        {
            var detailsVM = new StationDetailsViewModel(Station.Code);
            DialogService.ShowStationDetailsDialog(detailsVM);
        }

        private async Task _Remove()
        {
            await Load(async () =>
            {
                await BlWorkAsync(bl => bl.DeleteStation(Station.Code));
                OnRemove();
            });
        }

        public delegate void RemoveStationEventHandler(object sender);
        public event RemoveStationEventHandler Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);
    }
}
