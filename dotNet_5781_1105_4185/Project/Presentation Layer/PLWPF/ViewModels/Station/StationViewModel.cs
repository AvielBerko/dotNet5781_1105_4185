using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class StationViewModel : BaseViewModel
    {
        private BO.Station station;
        public BO.Station Station
        {
            get => station;
            set
            {
                station = value;
                OnPropertyChanged(nameof(Station));
            }
        }
        public RelayCommand StationDetails { get; }
        public RelayCommand RemoveStation { get; }
        public RelayCommand UpdateStation { get; }

        public StationViewModel(BO.Station station)
        {
            Station = station;

            StationDetails = new RelayCommand(obj => _Details());
            RemoveStation = new RelayCommand(obj => _Remove());
            UpdateStation = new RelayCommand(obj => _Update());
        }
        private void _Update()
        {
            var updateVM = new AddUpdateStationViewModel(Station.Code);
            if (DialogService.ShowAddUpdateStationDialog(updateVM) == DialogResult.Ok)
            {
                Station = (BO.Station)BlWork(bl => bl.GetStation(Station.Code));
                OnPropertyChanged(nameof(Station));
            }
        }

        private void _Details()
        {
            var detailsVM = new StationDetailsViewModel(Station.Code);
            if (DialogService.ShowStationDetailsDialog(detailsVM) == DialogResult.Cancel)
            {
                OnPropertyChanged(nameof(Station));
            }
        }

        private void _Remove()
        {
            BlWork(bl => bl.DeleteStation(Station.Code));
            OnRemove();
        }

        public delegate void RemoveStationEventHandler(object sender);
        public event RemoveStationEventHandler Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);
    }
}
