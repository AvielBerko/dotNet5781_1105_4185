﻿using System;
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
        public RelayCommand AddStation { get; }
        public RelayCommand RemoveAllStations { get; }

        public StationListViewModel()
        {

            Stations = new ObservableCollection<StationViewModel>(
                from station in (IEnumerable<BO.Station>)BlWork(bl => bl.GetAllStationsWithoutAdjacents())
                select CreateStationViewModel(station));

            AddStation = new RelayCommand(obj => _AddStation());
            RemoveAllStations = new RelayCommand(obj => _RemoveAllStations(), obj => Stations.Count > 0);
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

        private void _RemoveAllStations()
        {
            if (DialogService.ShowYesNoDialog("Are you sure you want to removea all stations?", "Remove all stations") == DialogResult.Yes)
            {
                BlWork(bl => bl.DeleteAllStations());
                Stations.Clear();
            }
        }
    }
}
