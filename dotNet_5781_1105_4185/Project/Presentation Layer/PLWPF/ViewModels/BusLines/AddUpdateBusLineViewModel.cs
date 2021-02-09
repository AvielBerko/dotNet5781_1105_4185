using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class AddUpdateBusLineViewModel : BaseViewModel
    {
        private ObservableCollection<LineStationViewModel> lineStations;
        public ObservableCollection<LineStationViewModel> LineStations
        {
            get => lineStations;
            private set
            {
                lineStations = value;
                LineStations.CollectionChanged += (sender, e) => UpdateIsLast();
                UpdateIsLast();
                OnPropertyChanged(nameof(LineStations));
            }
        }

        public BO.BusLine BusLine { get; }

        public string Title => (IsUpdate ? "Update" : "Add") + " Bus Line";
        public bool IsUpdate { get; private set; }

        public IEnumerable<BO.Regions> Regions => Enum.GetValues(typeof(BO.Regions)).Cast<BO.Regions>();

        public RelayCommand Ok { get; }
        public RelayCommand Cancel { get; }
        public RelayCommand InsertStation { get; }
        public RelayCommand Reverse { get; }

        public AddUpdateBusLineViewModel(Guid? updateId = null)
        {
            if (updateId == null)
            {
                BusLine = new BO.BusLine() { ID = Guid.NewGuid() };
                LineStations = new ObservableCollection<LineStationViewModel>();
                IsUpdate = false;
            }
            else
            {
                BusLine = (BO.BusLine)BlWork(bl => bl.GetBusLine(updateId ?? Guid.Empty));
                LineStations = new ObservableCollection<LineStationViewModel>
                    (from ls in BusLine.Route select _CreateLineStationViewModel(ls));
                IsUpdate = true;
            }

            Ok = new RelayCommand(_Ok);
            Cancel = new RelayCommand(_Cancel);
            InsertStation = new RelayCommand(obj => _AddRoute());
            Reverse = new RelayCommand(obj => _Reverse(), obj => LineStations.Count > 0);
        }

        private void _Reverse()
        {
            var reversedStations = (IEnumerable<BO.LineStation>)BlWork(bl => bl.ReverseLineStations(from vm in LineStations select vm.LineStation));
            LineStations = new ObservableCollection<LineStationViewModel>(from reversed in reversedStations select _CreateLineStationViewModel(reversed));
        }

        private void _AddRoute(LineStationViewModel after = null)
        {
            var currentStations = (from ls in LineStations select ls.LineStation.Station);
            var restStations = (IEnumerable<BO.Station>)BlWork(bl => bl.GetRestOfStations(currentStations));
            var vm = new SelectStationsViewModel(restStations);
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

        private void _Ok(object window)
        {
            BusLine.Route = from vm in LineStations select vm.LineStation;
            if (IsUpdate)
            {
                BlWork(bl => bl.UpdateBusLine(BusLine));
            }
            else
            {
                BlWork(bl => bl.AddBusLine(BusLine));
            }
            DialogService.CloseDialog(window, DialogResult.Ok);
        }

        private void _Cancel(object window)
        {
            DialogService.CloseDialog(window, DialogResult.Cancel);
        }
    }
}
