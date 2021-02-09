using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
	public class StationListViewModel : BaseViewModel
    {
        private ObservableCollection<StationViewModel> stations;
        public ObservableCollection<StationViewModel> Stations
        {
            get => stations;
            private set
            {
                stations = value;
                OnPropertyChanged(nameof(Stations));
            }
        }

        private bool loading;
        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged(nameof(Loading));
            }
        }

        public RelayCommand AddStation { get; }
        public RelayCommand RemoveAllStations { get; }

        public StationListViewModel()
        {
            Loading = false;
            Stations = new ObservableCollection<StationViewModel>();
            _ = GetStationsFromBL();

            AddStation = new RelayCommand(obj => _AddStation());
            RemoveAllStations = new RelayCommand(async obj => await _RemoveAllStations(), obj => Stations.Count > 0);
        }

        private async Task GetStationsFromBL()
        {
            Loading = true;
            var blStations = await BlWorkAsync(bl => bl.GetAllStationsWithoutAdjacents());
            Stations = new ObservableCollection<StationViewModel>(
                from station in (IEnumerable<BO.Station>)blStations
                select CreateStationViewModel(station));
            Loading = false;
        }

        private StationViewModel CreateStationViewModel(BO.Station station)
        {
            var vm = new StationViewModel(station);
            vm.Remove += (sender) => Stations.Remove((StationViewModel)sender);
            return vm;
        }

        private void _AddStation()
        {
            var addStationVM = new AddUpdateStationViewModel();
            addStationVM.AddedStaion += (sender, station) => Stations.Add(CreateStationViewModel(station));
            DialogService.ShowAddUpdateStationDialog(addStationVM);
        }

        private async Task _RemoveAllStations()
        {
            if (DialogService.ShowYesNoDialog("Are you sure you want to removea all stations?", "Remove all stations") == DialogResult.Yes)
            {
                Loading = true;
                await BlWorkAsync(bl => bl.DeleteAllStations());
                Stations.Clear();
                Loading = false;
            }
        }
    }
}
