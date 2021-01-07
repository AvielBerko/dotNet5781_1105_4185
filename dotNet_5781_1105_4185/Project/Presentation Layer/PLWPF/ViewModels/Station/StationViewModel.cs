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
        public AddStationViewModel AddStationViewModel { get; }
        public RelayCommand RemoveStation { get; }
        public RelayCommand UpdateStation { get; }

        public StationViewModel(AddStationViewModel addStationVM)
        {
            AddStationViewModel = addStationVM;
            RemoveStation = new RelayCommand(obj => { BlWork(bl => bl.DeleteStation(Station)); OnRemove(); });
            UpdateStation = new RelayCommand(obj => _Update());
        }
        private void _Update()
		{
            AddStationViewModel.IsUpdate = true;
            AddStationViewModel.Station = Station;
            if (DialogService.ShowAddStationDialog(AddStationViewModel) == true) 
            {
                OnPropertyChanged(nameof(Station));
            }
        }

        public delegate void RemoveBusEventHandler(object sender);
        public event RemoveBusEventHandler Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);
    }
}
