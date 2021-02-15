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

        private ObservableCollection<TripViewModel> _trips;
        public ObservableCollection<TripViewModel> Trips
        {
            get => _trips;
            private set
            {
                _trips = value;
                OnPropertyChanged(nameof(Trips));
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
        public RelayCommand ClearTrips { get; }
        public RelayCommand AddTrip { get; }

        private List<BO.Trip> _collidingTrips;

        public AddUpdateBusLineViewModel(Guid? updateId = null)
        {
            BusLine = new BO.BusLine() { ID = Guid.Empty };
            LineStations = new ObservableCollection<LineStationViewModel>();
            Trips = new ObservableCollection<TripViewModel>();
            _collidingTrips = new List<BO.Trip>();

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

            ClearTrips = new RelayCommand(obj => _ClearTrips(), obj => Trips.Count > 0);
            AddTrip = new RelayCommand(obj => _AddTrip());

            Cancel = new RelayCommand(_Cancel);
            Ok = new RelayCommand(async obj => await _Ok(obj));
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

        private void _ClearRoute()
        {
            if (DialogService.ShowYesNoDialog(
                "Are you sure you want to clear the route?",
                "Clear Route") == DialogResult.Yes)
            {
                LineStations.Clear();
            }
        }

        private void _ClearTrips()
        {
            if (DialogService.ShowYesNoDialog(
                "Are you sure you want to clear the trips?",
                "Clear Trips") == DialogResult.Yes)
            {
                Trips.Clear();
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

        private void _AddTrip()
        {
            Trips.Add(_CreateTripViewModel(new BO.Trip()));
        }

        private TripViewModel _CreateTripViewModel(BO.Trip trip)
        {
            var vm = new TripViewModel(trip, _CheckIsColliding);
            vm.RemoveTrip += sender => Trips.Remove(sender);
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Trip") _ValidateTrips();
            };
            return vm;
        }

        private bool _CheckIsColliding(TripViewModel vm)
        {
            return _collidingTrips.Any(trip => trip == vm.Trip);
        }

        private void _ValidateTrips()
        {
            var trips = from vm in Trips select vm.Trip;
            _collidingTrips = ((IEnumerable<BO.Trip>)BlWork(bl => bl.CollidingTrips(trips))).ToList();

            DialogService.ShowYesNoDialog($"colliding: {_collidingTrips.Count}", "test");
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
