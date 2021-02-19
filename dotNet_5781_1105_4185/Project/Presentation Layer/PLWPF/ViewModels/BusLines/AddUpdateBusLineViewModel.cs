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

        private ObservableCollection<LineTripViewModel> _lineTrips;
        public ObservableCollection<LineTripViewModel> LineTrips
        {
            get => _lineTrips;
            private set
            {
                _lineTrips = value;
                OnPropertyChanged(nameof(LineTrips));
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
        public RelayCommand ClearRoute { get; }
        public RelayCommand ClearLineTrips { get; }
        public RelayCommand AddLineTrip { get; }

        public AddUpdateBusLineViewModel(Guid? updateId = null)
        {
            BusLine = new BO.BusLine() { ID = Guid.Empty };
            LineStations = new ObservableCollection<LineStationViewModel>();
            LineTrips = new ObservableCollection<LineTripViewModel>();

            IsUpdate = updateId != null;
            if (IsUpdate)
            {
                _ = GetBusLineFromBL(updateId ?? Guid.Empty);
            }
            else
            {
                BusLine.ID = Guid.NewGuid();
            }

            InsertStation = new RelayCommand(obj => _AddRoute());
            Reverse = new RelayCommand(obj => _Reverse(), obj => LineStations.Count > 1);
            ClearRoute = new RelayCommand(obj => _ClearRoute(), obj => LineStations.Count > 0);

            ClearLineTrips = new RelayCommand(obj => _ClearLineTrips(), obj => LineTrips.Count > 0);
            AddLineTrip = new RelayCommand(obj => _AddLineTrip());

            Cancel = new RelayCommand(_Cancel);
            Ok = new RelayCommand(async window => await _Ok(window),
                obj => !_HasLineTripsValidationErrors());
        }

        private async Task GetBusLineFromBL(Guid id)
        {
            await Load(async () =>
            {
                BusLine = (BO.BusLine)await BlWorkAsync(bl => bl.GetBusLine(id));
                LineStations = new ObservableCollection<LineStationViewModel>
                    (from ls in BusLine.Route select _CreateLineStationViewModel(ls));
                LineTrips = new ObservableCollection<LineTripViewModel>
                    (from lt in BusLine.Trips select _CreateLineTripViewModel(lt));
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

        private void _ClearRoute()
        {
            if (DialogService.ShowYesNoDialog(
                "Are you sure you want to clear the route?",
                "Clear Route") == DialogResult.Yes)
            {
                LineStations.Clear();
            }
        }

        private void _ClearLineTrips()
        {
            if (DialogService.ShowYesNoDialog(
                "Are you sure you want to clear the trips?",
                "Clear Trips") == DialogResult.Yes)
            {
                LineTrips.Clear();
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
            vm.RemoveLineStation += sender => LineStations.Remove(sender);
            return vm;
        }

        private void _AddLineTrip()
        {
            LineTrips.Add(_CreateLineTripViewModel(new BO.LineTrip()));
            _ValidateLineTripsCollisions();
        }

        private LineTripViewModel _CreateLineTripViewModel(BO.LineTrip trip)
        {
            var vm = new LineTripViewModel(trip);
            vm.RemoveLineTrip += sender =>
            {
                LineTrips.Remove(sender);
                _ValidateLineTripsCollisions();
            };
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "LineTrip") _ValidateLineTripsCollisions();
            };
            return vm;
        }

        private void _ValidateLineTripsCollisions()
        {
            var trips = from vm in LineTrips select vm.LineTrip;
            var collidingLineTrips = ((IEnumerable<BO.LineTrip>)
                BlWork(bl => bl.CollidingTrips(trips))).ToList();

            foreach(var vm in LineTrips)
            {
                vm.IsColliding = collidingLineTrips.Contains(vm.LineTrip);
            }
        }

        private bool _HasLineTripsValidationErrors()
        {
            foreach (var vm in LineTrips)
            {
                if (vm.IsColliding) return true;
            }
            return false;
        }

        private void UpdateIsLast()
        {
            if (LineStations.Count > 0)
            {
                foreach (var vm in LineStations)
                {
                    vm.IsLast = false;
                }
                var last = LineStations.Last();
                last.IsConnected = false;
                last.IsLast = true;
            }
        }

        private async Task _Ok(object window)
        {
            await Load(async () =>
            {
                BusLine.Route = from vm in LineStations
                                select vm.LineStation;
                BusLine.Trips = from vm in LineTrips
                                select vm.LineTrip;
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
