using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class AddBusLineViewModel : BaseDialogViewModel
    {
        public ObservableCollection<LineStationViewModel> LineStations { get; }

        public BO.BusLine BusLine { get; }

        public IEnumerable<BO.Regions> Regions => Enum.GetValues(typeof(BO.Regions)).Cast<BO.Regions>();

        public RelayCommand Ok { get; }
        public RelayCommand Cancel { get; }
        public RelayCommand AddRoute { get; }

        public AddBusLineViewModel()
        {
            BusLine = new BO.BusLine() { ID = Guid.NewGuid() };
            LineStations = new ObservableCollection<LineStationViewModel>();
            LineStations.CollectionChanged += (sender, e) =>
            {
                if (LineStations.Count > 0)
                {
                    foreach (var vm in LineStations)
                    {
                        vm.IsLast = false;
                    }
                    LineStations.Last().IsLast = true;
                }
            };

            Ok = new RelayCommand(_Ok);
            Cancel = new RelayCommand(_Cancel);
            AddRoute = new RelayCommand(obj => _AddRoute());
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

        private void _Ok(object window)
        {
            CloseDialog(window, true);
        }

        private void _Cancel(object window)
        {
            CloseDialog(window, false);
        }
    }
}
