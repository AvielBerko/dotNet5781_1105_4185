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
        private ObservableCollection<StationViewModel> _stations;
        public ObservableCollection<StationViewModel> Stations
        {
            get => _stations;
            private set
            {
                _stations = value;
                OnPropertyChanged(nameof(Stations));
            }
        }

        public RelayCommand AddStation { get; }
        public RelayCommand RemoveAllStations { get; }

        public StationListViewModel()
        {
            Stations = new ObservableCollection<StationViewModel>();
            _ = GetStationsFromBL();

            AddStation = new RelayCommand(obj => _AddStation());
            RemoveAllStations = new RelayCommand(async obj => await _RemoveAllStations(), obj => Stations.Count > 0);
        }

        private async Task GetStationsFromBL()
        {
            await Load(async () =>
            {
                var blStations = await BlWorkAsync(bl => bl.GetAllStationsWithoutAdjacents());
                Stations = new ObservableCollection<StationViewModel>(
                    from station in (IEnumerable<BO.Station>)blStations
                    select CreateStationViewModel(station)
                );
            });
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
            if (DialogService.ShowAddUpdateStationDialog(addStationVM) == DialogResult.Ok)
            {
                Stations.Add(CreateStationViewModel(addStationVM.Station));
            }
        }

        private async Task _RemoveAllStations()
        {
            if (DialogService.ShowYesNoDialog("Are you sure you want to remove all stations?", "Remove all stations") == DialogResult.Yes)
            {
                await Load(async () =>
                {
                    await BlWorkAsync(bl => bl.DeleteAllStations());
                    Stations.Clear();
                });
            }
        }
    }
}
