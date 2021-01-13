using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class AddUpdateBusLineViewModel : BaseDialogViewModel
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
        public RelayCommand AddRoute { get; }
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
            AddRoute = new RelayCommand(obj => _AddRoute());
            Reverse = new RelayCommand(obj => _Reverse(), obj => LineStations.Count > 0);
        }

        private void _Reverse()
        {
            //TODO: Not working well...
            LineStations = new ObservableCollection<LineStationViewModel>(LineStations.Reverse());
        }

        private void _AddRoute(LineStationViewModel after = null)
        {
            var currentStations = (from ls in LineStations select ls.LineStation.Station);
            var restStations = (IEnumerable<BO.Station>)BlWork(bl => bl.GetRestOfStations(currentStations));
            var vm = new SelectStationsViewModel(restStations);
            if (DialogService.ShowSelectStationsDialog(vm) == true)
            {
                var addedStations = from st in vm.SelectedStations select new BO.LineStation { Station = st };
                if (after == null)
                {
                    foreach (var st in addedStations)
                        LineStations.Add(_CreateLineStationViewModel(st));
                }
                else
                {
                    var index = LineStations.IndexOf(after);
                    foreach (var st in addedStations)
                        LineStations.Insert(++index, _CreateLineStationViewModel(st));
                }
            }
        }

        private LineStationViewModel _CreateLineStationViewModel(BO.LineStation lineStation)
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
            CloseDialog(window, true);
        }

        private void _Cancel(object window)
        {
            CloseDialog(window, false);
        }
    }
}
