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
        public ObservableCollection<StationViewModel> Stations { get; }
        public AddStationViewModel AddStationViewModel { get; }
        public RelayCommand AddStation { get; }
        public RelayCommand RemoveAllStations { get; }

        public StationListViewModel()
        {
            Stations = new ObservableCollection<StationViewModel>(
                from station in (IEnumerable<BO.Station>)BlWork(bl => bl.GetAllStations())
                select CreateStationViewModel(station));

            AddStationViewModel = new AddStationViewModel();
            AddStationViewModel.AddedStaion += (sender, station) => Stations.Add(CreateStationViewModel(station));
            // AddStationViewModel.UpdatedStation += sender => 

            AddStation = new RelayCommand(obj => _AddStation());
            RemoveAllStations = new RelayCommand(obj => _RemoveAllStations(), obj => Stations.Count > 0);
        }

        private StationViewModel CreateStationViewModel(BO.Station station)
        {
            var vm = new StationViewModel(AddStationViewModel) { Station = station };
            vm.Remove += (sender) => Stations.Remove((StationViewModel)sender);
            return vm;
        }

        private void _AddStation()
        {
            DialogService.ShowAddStationDialog(AddStationViewModel);
        }

        private void _RemoveAllStations()
        {
            // Are You Sure ? 
            BlWork(bl => bl.DeleteAllStations());
            Stations.Clear();
        }
    }
}
