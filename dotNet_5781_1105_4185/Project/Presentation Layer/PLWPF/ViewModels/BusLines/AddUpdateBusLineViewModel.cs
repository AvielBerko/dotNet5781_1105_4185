﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class AddUpdateBusLineViewModel : BaseViewModel
    {
        private ObservableCollection<LineStationViewModel> _lineStations;
        public ObservableCollection<LineStationViewModel> LineStations
        {
            get => _lineStations;
            private set
            {
                _lineStations = value;
                LineStations.CollectionChanged += (sender, e) => UpdateIsLast();
                UpdateIsLast();
                OnPropertyChanged(nameof(LineStations));
            }
        }

        private BO.BusLine _busLine;
        public BO.BusLine BusLine
        {
            get => _busLine;
            set
            {
                _busLine = value;
                OnPropertyChanged(nameof(BusLine));
            }
        }

        public string Title => (IsUpdate ? "Update" : "Add") + " Bus Line";
        public bool IsUpdate { get; private set; }

        public IEnumerable<BO.Regions> Regions => Enum.GetValues(typeof(BO.Regions)).Cast<BO.Regions>();

        public RelayCommand Ok { get; }
        public RelayCommand Cancel { get; }
        public RelayCommand InsertStation { get; }
        public RelayCommand Reverse { get; }
        public RelayCommand Clear { get; }

        public AddUpdateBusLineViewModel(Guid? updateId = null)
        {
            BusLine = new BO.BusLine() { ID = Guid.Empty };
            LineStations = new ObservableCollection<LineStationViewModel>();

            IsUpdate = updateId != null;
            if (IsUpdate)
            {
                _ = GetBusLineFromBL(updateId ?? Guid.Empty);
            }
            else
            {
                BusLine.ID = Guid.NewGuid();
            }

            Cancel = new RelayCommand(_Cancel);
            Ok = new RelayCommand(async obj => await _Ok(obj));
            InsertStation = new RelayCommand(obj => _AddRoute());
            Reverse = new RelayCommand(obj => _Reverse(), obj => LineStations.Count > 0);
            Clear = new RelayCommand(obj => _Clear(), obj => LineStations.Count > 0);
        }

        private async Task GetBusLineFromBL(Guid id)
        {
            await Load(async () =>
            {
                BusLine = (BO.BusLine)await BlWorkAsync(bl => bl.GetBusLine(id));
                LineStations = new ObservableCollection<LineStationViewModel>
                    (from ls in BusLine.Route select _CreateLineStationViewModel(ls));
            });
        }

        private void _Reverse()
        {
            var reversedStations = (IEnumerable<BO.LineStation>)BlWork(
                bl => bl.ReverseLineStations(from vm in LineStations select vm.LineStation)
            );

            LineStations = new ObservableCollection<LineStationViewModel>(
                from reversed in reversedStations
                select _CreateLineStationViewModel(reversed)
            );
        }

        private void _Clear()
        {
            if (DialogService.ShowYesNoDialog(
                "Are you sure you want to clear the route?",
                "Clear Route") == DialogResult.Yes)
            {
                LineStations.Clear();
            }
        }

        private void _AddRoute(LineStationViewModel after = null)
        {
            var currentStations = (from ls in LineStations select ls.LineStation.Station);
            var vm = new SelectStationsViewModel(currentStations);
            if (DialogService.ShowSelectStationsDialog(vm) == DialogResult.Ok)
            {
                var addedStations = from st in vm.SelectedStations
                                    select new BO.LineStation { Station = st };
                int index = after == null ? 0 : LineStations.IndexOf(after) + 1;
                foreach (var st in addedStations)
                {
                    LineStations.Insert(index++, _CreateLineStationViewModel(st));
                }
            }
        }

        private LineStationViewModel _CreateLineStationViewModel(BO.LineStation lineStation, BO.LineStation nextStation = null)
        {
            var vm = new LineStationViewModel(lineStation);
            vm.InsertStation += (sender) => _AddRoute(sender);
            vm.RemoveLineStation += (sender) => LineStations.Remove(sender);
            return vm;
        }

        private void UpdateIsLast()
        {
            if (LineStations.Count > 0)
            {
                foreach (var vm in LineStations)
                {
                    vm.IsLast = false;
                }
                LineStations.Last().IsLast = true;
            }
        }

        private async Task _Ok(object window)
        {
            await Load(async () =>
            {
                BusLine.Route = from vm in LineStations
                                select vm.LineStation;
                if (IsUpdate)
                    await BlWorkAsync(bl => bl.UpdateBusLine(BusLine));
                else
                    await BlWorkAsync(bl => bl.AddBusLine(BusLine));

                DialogService.CloseDialog(window, DialogResult.Ok);
            });
        }

        private void _Cancel(object window)
        {
            DialogService.CloseDialog(window, DialogResult.Cancel);
        }
    }
}
