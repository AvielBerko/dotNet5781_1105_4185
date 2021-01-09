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
            RemoveStation = new RelayCommand(obj => { BlWork(bl => bl.DeleteStation(Station)); OnRemove(); });
            UpdateStation = new RelayCommand(obj => _Update());
        }
        private void _Update()
		{
            var updateVM = new UpdateStationViewModel(Station.Code);
            if (DialogService.ShowUpdateStationDialog(updateVM) == true) 
            {
                OnPropertyChanged(nameof(Station));
            }
        }
        private void _Details()
		{

		}

        public delegate void RemoveStationEventHandler(object sender);
        public event RemoveStationEventHandler Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);
    }
}
